using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Features.Users.Commands.Login;
using RealEstateApp.Application.Features.Users.Commands.RegisterUser;

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
    }
}