using Application.Commands;

namespace Application.Interfaces
{
    public interface IReserveSeatCommandHandler
    {
        Task<bool> HandlerAsync(ReserveSeatCommand request);
    }
}