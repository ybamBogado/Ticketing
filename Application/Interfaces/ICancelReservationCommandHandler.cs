using Application.Commands;

namespace Application.Interfaces
{
    public interface ICancelReservationCommandHandler
    {
        Task<bool> HandlerAsync(CancelReservationCommand request);
    }
}