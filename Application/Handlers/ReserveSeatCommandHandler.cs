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

        public async Task<(bool Success, string? ErrorMessage, Guid? ReservationId, DateTime? ExpiresAt)> HandlerAsync(ReserveSeatCommand request)
        {
            var activeReservationsCount = await _reservationRepository.GetActiveReservationsCountAsync(request.UserId);
            if (activeReservationsCount >= 6)
            {
                var auditLog = Domain.Factories.AuditLogFactory.CreateForFailedReservationAttempt(request.UserId, request.SeatId, "Límite máximo de reservas (6) alcanzado.");
                await _auditLogRepository.AddAuditLogAsync(auditLog);
                await _unitOfWork.SaveChangesAsync();
                return (false, "Has alcanzado el límite máximo de 6 asientos reservados.", null, null);
            }

            var oneMinuteAgo = DateTime.UtcNow.AddMinutes(-1);
            var isCooldownActive = await _reservationRepository.HasRecentReservationAsync(request.UserId, request.SeatId, oneMinuteAgo);
            if (isCooldownActive)
            {
                var auditLog = Domain.Factories.AuditLogFactory.CreateForFailedReservationAttempt(request.UserId, request.SeatId, "Cooldown de 1 minuto activo para el asiento.");
                await _auditLogRepository.AddAuditLogAsync(auditLog);
                await _unitOfWork.SaveChangesAsync();
                return (false, "Debes esperar 1 minuto antes de volver a reservar este asiento.", null, null);
            }

            var seat = await _seatRepository.GetSeatByIdAsync(request.SeatId);
            if (seat == null)
            {
                var auditLog = Domain.Factories.AuditLogFactory.CreateForFailedReservationAttempt(request.UserId, request.SeatId, "La butaca no existe.");
                await _auditLogRepository.AddAuditLogAsync(auditLog);
                await _unitOfWork.SaveChangesAsync();
                return (false, "La butaca no existe.", null, null);
            }
            if (seat.Status != "Available")
            {
                var auditLog = Domain.Factories.AuditLogFactory.CreateForFailedReservationAttempt(request.UserId, request.SeatId, "La butaca no está disponible.");
                await _auditLogRepository.AddAuditLogAsync(auditLog);
                await _unitOfWork.SaveChangesAsync();
                return (false, "La butaca no está disponible.", null, null);
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
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };   
            await _reservationRepository.AddReservationAsync(reservation);
            var auditEntry = Domain.Factories.AuditLogFactory.CreateForSeatReservation(request.UserId, seat.Id);
            await _auditLogRepository.AddAuditLogAsync(auditEntry);
            
            await _unitOfWork.SaveChangesAsync();
            return (true, null, reservation.Id, reservation.ExpiresAt);
        }
    }
}