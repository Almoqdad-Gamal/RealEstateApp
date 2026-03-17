using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Features.Users.Commands.UpdateProfile;
using RealEstateApp.Application.Features.Users.Queries.GetProfile;

namespace RealEstateApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _mediator.Send(new GetProfileQuery(userId));
            return Ok(result);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
        {
            command.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}