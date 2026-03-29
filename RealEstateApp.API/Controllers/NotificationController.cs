using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Features.Notifications.Commands.MarkAllAsRead;
using RealEstateApp.Application.Features.Notifications.Queries.GetUnreadCount;
using RealEstateApp.Application.Features.Notifications.Queries.GetUserNotifications;

namespace RealEstateApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyNotifications ()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _mediator.Send(new GetUserNotificationsQuery(userId)));
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount ()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _mediator.Send(new GetUnreadCountQuery(userId));
            return Ok(new {unreadCount = result});
        }

        [HttpPut("mark-all-read")]
        public async Task<IActionResult> MarkAllAsRead ()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _mediator.Send(new MarkAllAsReadCommand(userId));
            return Ok(new {messege = "All notifications marked as read."});
        }
    }
}