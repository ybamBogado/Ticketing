using Application.Interfaces.Repositories;
using Application.Commands;
using Domain.Entities;
using Application.Interfaces;

namespace Application.Handlers
{
    public class RegisterUserCommandHandler:IRegisterUserCommandHandler
{
    private readonly IUserRepository _repository;
    public RegisterUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }
    public async Task<int> HandleAsync(RegisterUserCommand command)
    {
        var user = new User {
            Name = command.Name,
            Email = command.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Password)
        };
        await _repository.AddAsync(user);
        return user.Id;
    }

}
}