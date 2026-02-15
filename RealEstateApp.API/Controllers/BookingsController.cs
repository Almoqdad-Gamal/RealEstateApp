using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Features.Booking.Commands.CreateBooking;
using RealEstateApp.Application.Features.Booking.Commands.UpdateBookingStatus;
using RealEstateApp.Application.Features.Booking.Queries.GetUserBookings;

namespace RealEstateApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Getting the user bookings
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserBookings(int userId)
        {
            var query = new GetUserBookingsQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // Create a new booking
        [HttpPost]
        public async Task<IActionResult> CreateBooking ([FromBody] CreateBookingCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetUserBookings), new {userId = result.ClientId}, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Update the booking status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateBookingStatusCommand command)
        {
            try
            {
                command.BookingId = id;
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}