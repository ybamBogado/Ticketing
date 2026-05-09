using Application.Commands;
using Application.DTOs;

namespace Application.Interfaces{
    public interface ILoginUserCommandHandler
{
    Task<LoginResponse> HandleAsync(LoginUserCommand command);
}

}