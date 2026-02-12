using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Features.Users.Commands.RegisterUser;

namespace RealEstateApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register ([FromBody]RegisterUserCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
    }
}