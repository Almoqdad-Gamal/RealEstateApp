using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Features.Favorites.Commands.AddFavorite;
using RealEstateApp.Application.Features.Favorites.Commands.RemoveFavorite;
using RealEstateApp.Application.Features.Favorites.Queries.CheckFavorite;
using RealEstateApp.Application.Features.Favorites.Queries.GetUserFavorites;

namespace RealEstateApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Client,Admin")]
    public class FavoritesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FavoritesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{propertyId}")]
        public async Task<IActionResult> AddFavorite (int propertyId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _mediator.Send(new AddFavoriteCommand
            {
                UserId = userId,
                PropertyId = propertyId
            });
            return Ok(new {message = "Property added to favorites."});
        }

        [HttpGet]
        public async Task<IActionResult> GetMyFavorites()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _mediator.Send(new GetUserFavoritesQuery(userId)));
        }

        [HttpGet("{propertyId}/check")]
        public async Task<IActionResult> CheckFavorite (int propertyId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _mediator.Send(new CheckFavoriteQuery(userId, propertyId));
            return Ok(new {isFavorute = result});
        }

        [HttpDelete("{propertyId}")]
        public async Task<IActionResult> RemoveFavorite(int propertyId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _mediator.Send(new RemoveFavoriteCommand
            {
                UserId = userId,
                PropertyId = propertyId
            });
            return NoContent();
        }
    }
}