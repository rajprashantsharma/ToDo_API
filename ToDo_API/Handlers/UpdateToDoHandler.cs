using MediatR;
using ToDo_API.Commands;
using ToDo_API.Commands.ToDo_API.Commands;
using ToDo_API.Models;
using ToDo_API.Repositories;
using ToDo_API.Responses;

namespace ToDo_API.Handlers
{
    public class UpdateToDoHandler : IRequestHandler<UpdateToDoCommandWrapper, Response<ToDoItem>>
    {
        private readonly ToDoRepository _repository;

        public UpdateToDoHandler(ToDoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<ToDoItem>> Handle(UpdateToDoCommandWrapper request, CancellationToken cancellationToken)
        {
            
            var existingItem = await _repository.GetByIdAsync(request.Id);
            if (existingItem == null)
            {
                return Response<ToDoItem>.Failure("ToDo item not found", System.Net.HttpStatusCode.NotFound);
            }

           
            existingItem.Title = request.Command.Title;
            existingItem.Description = request.Command.Description;
            existingItem.DueDate = request.Command.DueDate;
            existingItem.Completed = request.Command.Completed;

           
            await _repository.UpdateAsync(existingItem);
            return Response<ToDoItem>.CreateSuccess(existingItem);
        }
    }
}

