using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo_API.Commands;
using ToDo_API.Commands.ToDo_API.Commands;
using ToDo_API.Models;
using ToDo_API.Queries;
using ToDo_API.Responses;

namespace ToDo_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Protect all endpoints by default
    public class ToDoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ToDoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous] // Publicly accessible endpoint
        [ProducesResponseType(typeof(Response<List<ToDoItem>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<List<ToDoItem>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _mediator.Send(new GetAllToDosQuery());
                if (result.Data != null && result.Data.Any())
                {
                    return Ok(Response<List<ToDoItem>>.CreateSuccess(result.Data.ToList(), "To Do items retrieved successfully"));
                }

                return NotFound(Response<List<ToDoItem>>.Failure("To Do items not found", System.Net.HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                // Handle unexpected errors and return 500 Internal Server Error
                return StatusCode(500, Response<string>.Failure("An error occurred while retrieving the To Do items", System.Net.HttpStatusCode.InternalServerError));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<ToDoItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ToDoItem>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<ToDoItem>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetToDoByIdQuery(id));

                if (result.Data != null)
                {
                    return Ok(Response<ToDoItem>.CreateSuccess(result.Data, "To Do item retrieved successfully"));
                }
                return NotFound(Response<ToDoItem>.Failure("To Do item not found", System.Net.HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.Failure("An error occurred while retrieving the To Do item", System.Net.HttpStatusCode.InternalServerError));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response<ToDoItem>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<ToDoItem>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ToDoItem>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateToDoCommand command)
        {
            try
            {
                if (command == null || string.IsNullOrEmpty(command.Title))
                {
                    return BadRequest(Response<ToDoItem>.Failure("Title is required"));
                }

                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, Response<ToDoItem>.CreateSuccess(result.Data, "To Do item created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.Failure("An error occurred while creating the To Do item", System.Net.HttpStatusCode.InternalServerError));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Response<ToDoItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ToDoItem>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ToDoItem>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateToDoCommand command)
        {
            try
            {
                if (command == null || string.IsNullOrEmpty(command.Title))
                {
                    return BadRequest(Response<ToDoItem>.Failure("Title is required"));
                }

                var result = await _mediator.Send(new UpdateToDoCommandWrapper(id, command));

                return Ok(Response<ToDoItem>.CreateSuccess(result.Data, "To Do item updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.Failure("An error occurred while updating the To Do item", System.Net.HttpStatusCode.InternalServerError));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteToDoCommand(id));

                if (result.Data)
                {
                    return Ok(Response<bool>.CreateSuccess(true, "To Do item deleted successfully"));
                }

                return NotFound(Response<bool>.Failure("To Do item not found", System.Net.HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.Failure("An error occurred while deleting the To Do item", System.Net.HttpStatusCode.InternalServerError));
            }
        }

        // Additional Features: Marking To-Do as Complete
        [HttpPatch("{id}/complete")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsComplete(Guid id)
        {
            var result = await _mediator.Send(new CompleteToDoCommand(id));

            if (result.Data)
            {
                return Ok(Response<bool>.CreateSuccess(true, "To Do item marked as complete"));
            }

            return NotFound(Response<bool>.Failure("To Do item not found", System.Net.HttpStatusCode.NotFound));
        }

        // Additional Features: Filtering and Sorting for To-Do Items
        [HttpGet("filtered")]
        [ProducesResponseType(typeof(Response<List<ToDoItem>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<List<ToDoItem>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<List<ToDoItem>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll([FromQuery] bool? completed, [FromQuery] string sortBy = "dueDate", [FromQuery] string sortOrder = "asc")
        {
            try
            {
                var result = await _mediator.Send(new GetFilteredToDosQuery(completed, sortBy, sortOrder));

                if (result.Data == null || !result.Data.Any())
                {
                    return NotFound(Response<List<ToDoItem>>.Failure("No ToDo items found", System.Net.HttpStatusCode.NotFound));
                }

                return Ok(Response<List<ToDoItem>>.CreateSuccess(result.Data.ToList(), "Filtered ToDo items retrieved successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<List<ToDoItem>>.Failure("An error occurred while retrieving the To Do items", System.Net.HttpStatusCode.BadRequest));
            }
        }
    }
}
