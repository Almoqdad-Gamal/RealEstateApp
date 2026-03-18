using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Features.Booking.Commands.CancelBooking;
using RealEstateApp.Application.Features.Booking.Commands.CreateBooking;
using RealEstateApp.Application.Features.Booking.Commands.UpdateBookingStatus;
using RealEstateApp.Application.Features.Booking.Queries.GetBookingById;
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
        [HttpGet("my-bookings")]
        [Authorize]
        public async Task<IActionResult> GetUserBookings()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _mediator.Send(new GetUserBookingsQuery(userId));
            return Ok(result);
        }

        // Create a new booking
        [HttpPost]
        [Authorize(Roles = "Client,Admin")] // Only client can booking
        public async Task<IActionResult> CreateBooking ([FromBody] CreateBookingCommand command)
        {
            var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            command.ClientId = clientId;
            
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUserBookings), new {userId = result.ClientId}, result);
        }

        // Update the booking status
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Owner,Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateBookingStatusCommand command)
        {
                command.BookingId = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById (int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var role = User.FindFirst(ClaimTypes.Role)!.Value;

            var query = new GetBookingByIdQuery(id)
            {
                RequestingUserId = userId,
                RequestingRole = role
            };
            
            return Ok(await _mediator.Send(query));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Cancel (int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var command = new CancelBookingCommand(id);
            command.RequestingUserId = userId;
            await _mediator.Send(command);
            return NoContent();
        }

    }
}