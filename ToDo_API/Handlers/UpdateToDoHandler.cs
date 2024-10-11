using MediatR;
using ToDo_API.Commands;
using ToDo_API.Commands.ToDo_API.Commands;
using ToDo_API.Models;
using ToDo_API.Repositories;
using ToDo_API.Responses;

namespace ToDo_API.Handlers
{
    // Modify handler to handle the new wrapper command
    public class UpdateToDoHandler : IRequestHandler<UpdateToDoCommandWrapper, Response<ToDoItem>>
    {
        private readonly ToDoRepository _repository;

        public UpdateToDoHandler(ToDoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<ToDoItem>> Handle(UpdateToDoCommandWrapper request, CancellationToken cancellationToken)
        {
            // Get the existing item by id
            var existingItem = await _repository.GetByIdAsync(request.Id);
            if (existingItem == null)
            {
                return Response<ToDoItem>.Failure("ToDo item not found", System.Net.HttpStatusCode.NotFound);
            }

            // Update the fields from the body (request.Command)
            existingItem.Title = request.Command.Title;
            existingItem.Description = request.Command.Description;
            existingItem.DueDate = request.Command.DueDate;
            existingItem.Completed = request.Command.Completed;

            // Persist the updated item
            await _repository.UpdateAsync(existingItem);
            return Response<ToDoItem>.CreateSuccess(existingItem);
        }
    }
}

