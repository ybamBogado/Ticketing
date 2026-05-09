using Application.Commands;

namespace Application.Interfaces{
    public interface ILoginUserCommandHandler
{
    Task<string> HandlerAsync(LoginUserCommand command);
}

}