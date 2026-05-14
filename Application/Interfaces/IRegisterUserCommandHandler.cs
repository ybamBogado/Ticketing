using Application.Commands;

namespace Application.Interfaces
{
    public interface IRegisterUserCommandHandler
    {
        Task<int> HandleAsync(RegisterUserCommand command);
    }
}