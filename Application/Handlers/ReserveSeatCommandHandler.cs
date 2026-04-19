using Application.Commands;
using Application.Interfaces;
using Domain.Entities;
using System;

namespace Application.Handlers
{
    public class ReserveSeatCommandHandler : IReserveSeatCommandHandler
    {
        private readonly IAppDbContext _context;

        public ReserveSeatCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HandlerAsync(ReserveSeatCommand request)
        {
            var seat = await _context.Seats.FindAsync(request.SeatId);
            if (seat == null)
            {
                return false;
            }
            if (seat.Status != "Available")
            {
                return false;
            }

            seat.Status = "Reserved";

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

            _context.AuditLogs.Add(auditEntry);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}