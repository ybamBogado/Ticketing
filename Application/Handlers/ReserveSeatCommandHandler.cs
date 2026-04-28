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

        public async Task<bool> HandlerAsync(ReserveSeatCommand request)
        {
            var seat = await _seatRepository.GetSeatByIdAsync(request.SeatId);
            if (seat == null)
            {
                return false;
            }
            if (seat.Status != "Available")
            {
                return false;
            }
            seat.Status = "Reserved";

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                SeatId = request.SeatId,
                Status = "Active",
                ReservedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };       
            await _reservationRepository.AddReservationAsync(reservation);

            var auditEntry = new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Action = "ReserveSeat",
                EntityType = "Seat",
                EntityId = seat.Id.ToString(),
                Details = $"Reserva tentativa para la butaca ID: {seat.Id}. El estado pasó a Reserved.",
                CreatedAt = DateTime.UtcNow
            };
            await _auditLogRepository.AddAuditLogAsync(auditEntry);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}