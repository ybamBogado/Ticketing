using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Commands;
using Application.DTOs;
using Domain.Entities;


namespace Application.Handlers
{
    public class LoginUserCommandHandler:ILoginUserCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService; 

    public LoginUserCommandHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> HandleAsync(LoginUserCommand command)
    {
       
        var user = await _userRepository.GetByEmailAsync(command.Email);
        
        if (user == null) 
            throw new Exception("Usuario no encontrado");

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash);

        if (!isPasswordValid)
            throw new Exception("Contraseña incorrecta");

        var token = _tokenService.GenerateToken(user);

        return new LoginResponse
        {
            Token = token,
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}

}