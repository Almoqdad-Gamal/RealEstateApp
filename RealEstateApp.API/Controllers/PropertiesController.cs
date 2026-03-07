using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Features.Properties.Commands.AddPropertyImage;
using RealEstateApp.Application.Features.Properties.Commands.CreateProperty;
using RealEstateApp.Application.Features.Properties.Commands.DeleteProperty;
using RealEstateApp.Application.Features.Properties.Commands.UpdateProperty;
using RealEstateApp.Application.Features.Properties.Queries.GetAllProperties;
using RealEstateApp.Application.Features.Properties.Queries.GetAllProperties.GetPropertyById;
using RealEstateApp.Application.Features.Properties.Queries.SearchProperties;

namespace RealEstateApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get all available properties 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPropertiesQuery());
            return Ok(result);
        }
        
        // Get single property by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById (int id)
        {
            var result = await _mediator.Send(new GetPropertyByIdQuery(id));
            return Ok(result);
        }

        // Advanced search for properties
        [HttpGet("search")]
        public async Task<IActionResult> Search ([FromQuery] SearchPropertiesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // Creates a new property
        [HttpPost]
        [Authorize(Roles = "Owner, Admin")] // Only owner and admin can create property
        public async Task<IActionResult> Create ([FromBody] CreatePropertyCommand command)
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                command.OwnerId = ownerId;

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new {id = result.Id}, result);
        }

        // Updates an existing property
        [HttpPut("{id}")]
        [Authorize(Roles = "Owner, Admin")]
        public async Task<IActionResult> Update (int id, UpdatePropertyCommand command)
        {
            if (id != command.Id)
                return BadRequest(new {message = "ID mismatch"});

            command.RequestingUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            command.RequestingRole = User.FindFirst(ClaimTypes.Role)!.Value;
            
            var result = await _mediator.Send(command);
            return Ok(result);

        }

        // Deletes property by ID 
        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner, Admin")]
        public async Task<IActionResult> Delete (int id)
        {
            await _mediator.Send(new DeletePropertyCommand(id));
            return NoContent();

        }

        // Upload image for a property
        [HttpPost("{id}/images")]
        [Authorize(Roles = "Owner, Admin")]
        public async Task<IActionResult> UploadImage(int id, IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest(new {message = "No image provided"});

            using var stream = image.OpenReadStream();

            var command = new AddPropertyImageCommand
            {
                PropertyId = id,
                ImageStream = stream,
                FileName = image.FileName
            };

            var imageUrl = await _mediator.Send(command);
            return Ok(new { imageUrl });
        }
    }
}