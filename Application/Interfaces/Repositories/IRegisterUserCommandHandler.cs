using Application.Commands;

namespace Application.Interfaces.Repositories
{
    public interface IRegisterUserCommandHandler
    {
        Task<int> HandleAsync(RegisterUserCommand command);
    }
}

