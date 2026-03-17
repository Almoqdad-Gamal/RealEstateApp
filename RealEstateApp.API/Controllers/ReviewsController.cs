using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Features.Review.Commands.CreateReview;
using RealEstateApp.Application.Features.Review.Commands.DeleteReview;
using RealEstateApp.Application.Features.Review.Queries.GetPropertyReviews;

namespace RealEstateApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediatr;
        public ReviewsController(IMediator mediator)
        {
            _mediatr = mediator;
        }

        // Get reviews of a specific property
        [HttpGet("property/{propertyId}")]
        public async Task<IActionResult> GetPropertyReviews(int propertyId)
        {
            var result = await _mediatr.Send(new GetPropertyReviewsQuery(propertyId));
            return Ok(result);
        }

        // Create new review
        [HttpPost]
        [Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> CreateReview ([FromBody] CreateReviewCommand command)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            command.UserId = userId;

            var result = await _mediatr.Send(command);
            return CreatedAtAction(nameof(GetPropertyReviews), new {propertyId = result.PropertyId}, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Delete (int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var command = new DeleteReviewCommand(id);
            command.RequestingUserId = userId;
            await _mediatr.Send(command);
            return NoContent();
        }
    }
}