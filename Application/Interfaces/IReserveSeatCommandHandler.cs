using System;
using Application.Commands;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReserveSeatCommandHandler
    {
        Task<(bool Success, Guid? ReservationId)> HandlerAsync(ReserveSeatCommand request);
    }
}