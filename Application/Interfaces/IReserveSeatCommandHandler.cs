using System;
using Application.Commands;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReserveSeatCommandHandler
    {
        Task<(bool Success, string? ErrorMessage, Guid? ReservationId, DateTime? ExpiresAt)> HandlerAsync(ReserveSeatCommand request);
    }
}