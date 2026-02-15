using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Features.Review.Commands.CreateReview;
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
            var query = new GetPropertyReviewsQuery(propertyId);
            var result = await _mediatr.Send(query);
            return Ok(result);
        }

        // Create new review
        [HttpPost]
        public async Task<IActionResult> CreateReview ([FromBody] CreateReviewCommand command)
        {
            try
            {
                var result = await _mediatr.Send(command);
                return CreatedAtAction(nameof(GetPropertyReviews), new {propertyId = result.PropertyId}, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
    }
}