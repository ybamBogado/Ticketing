using Application.Commands;
using Application.Interfaces;
using Domain.Entities;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class ReserveSeatCommandHandler : IReserveSeatCommandHandler
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReserveSeatCommandHandler(ISeatRepository seatRepository, IReservationRepository reservationRepository, IAuditLogRepository auditLogRepository, IUnitOfWork unitOfWork)
        {
            _seatRepository = seatRepository;
            _reservationRepository = reservationRepository;
            _auditLogRepository = auditLogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<(bool Success, string ErrorMessage, Guid? ReservationId)> HandlerAsync(ReserveSeatCommand request)
        {
            var activeReservationsCount = await _reservationRepository.GetActiveReservationsCountAsync(request.UserId);
            if (activeReservationsCount >= 6)
            {
                return (false, "Has alcanzado el límite máximo de 6 asientos reservados.", null);
            }

            var oneMinuteAgo = DateTime.UtcNow.AddMinutes(-1);
            var isCooldownActive = await _reservationRepository.HasRecentReservationAsync(request.UserId, request.SeatId, oneMinuteAgo);
            if (isCooldownActive)
            {
                return (false, "Debes esperar 1 minuto antes de volver a reservar este asiento.", null);
            }

            var seat = await _seatRepository.GetSeatByIdAsync(request.SeatId);
            if (seat == null)
            {
                return (false, "La butaca no existe.", null);
            }
            if (seat.Status != "Available")
            {
                return (false, "La butaca no está disponible.", null);
            }
            seat.Status = "Reserved";
            seat.Version++;

            var existingReservations = await _reservationRepository.GetReservationsBySeatIdAsync(request.SeatId);
            foreach (var oldRes in existingReservations)
            {
                if (oldRes.ExpiresAt < DateTime.UtcNow)
                {
                    await _reservationRepository.DeleteReservationAsync(oldRes);
                }
            }

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                SeatId = request.SeatId,
                Status = "Reserved",
                ReservedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(1)
            };       
            await _reservationRepository.AddReservationAsync(reservation);

            var auditEntry = Domain.Factories.AuditLogFactory.CreateForSeatReservation(request.UserId, seat.Id);
            await _auditLogRepository.AddAuditLogAsync(auditEntry);
            
            await _unitOfWork.SaveChangesAsync();
            return (true, null, reservation.Id);
        }
    }
}