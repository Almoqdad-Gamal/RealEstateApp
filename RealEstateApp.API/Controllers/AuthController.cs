using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Features.Users.Commands.ForgetPassword;
using RealEstateApp.Application.Features.Users.Commands.Login;
using RealEstateApp.Application.Features.Users.Commands.RegisterUser;
using RealEstateApp.Application.Features.Users.Commands.ResetPassword;

namespace RealEstateApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(Register), result);
        }

        // Login and get jwt token
        [HttpPost("Login")]
        public async Task<IActionResult> Login ([FromBody] LoginCommand command)
        {
                var result = await _mediator.Send(command);
                return Ok(result);
        }

        [HttpPost("Forget-password")]
        public async Task<IActionResult> ForgetPassword ([FromBody] ForgetPasswordCommand command)
        {
            await _mediator.Send(command);

            // We always send the same message, whether the email exists or not
            return Ok(new {message = "If this email exists, a reset link has been sent."});
        }

        [HttpPost("Reset-password")]
        public async Task<IActionResult> ResetPassword ([FromBody] ResetPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok(new {message = "Password has been reset successfully."});
        }
    }
}