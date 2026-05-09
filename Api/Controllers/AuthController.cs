using Application.Commands;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController] 
    [Route("api/v1/auth")]
    public class AuthController: ControllerBase
    {
        private readonly IRegisterUserCommandHandler _registerHandler;
        private readonly ILoginUserCommandHandler _loginHandler;

        public AuthController(IRegisterUserCommandHandler registerHandler, ILoginUserCommandHandler loginHandler)
        {
            _registerHandler = registerHandler;
            _loginHandler = loginHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _registerHandler.HandleAsync(command);
            return StatusCode(201, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _loginHandler.HandleAsync(command);
            return Ok(result);
        }
    }
}