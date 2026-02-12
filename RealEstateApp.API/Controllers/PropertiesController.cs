using MediatR;
using Microsoft.AspNetCore.Mvc;
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
            var query = new GetAllPropertiesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        // Get single property by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById (int id)
        {
            var query = new GetPropertyByIdQuery(id);
            var result = await _mediator.Send(query);

            if(result == null)
                return NotFound(new {message = "Property not found"});

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
        public async Task<IActionResult> Create ([FromBody] CreatePropertyCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new {id = result.Id}, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        // Updates an existing property
        [HttpPut("{id}")]
        public async Task<IActionResult> Update (int id, UpdatePropertyCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest(new {message = "ID mismatch"});

                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        // Deletes property by ID 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            try
            {
                var command = new DeletePropertyCommand(id);
                var result = await _mediator.Send(command);

                if(!result)
                    return NotFound(new {messege = "Property not found"});

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
    }
}