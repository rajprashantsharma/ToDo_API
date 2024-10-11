using MediatR;
using ToDo_API.Commands;
using ToDo_API.Repositories;
using ToDo_API.Responses;

namespace ToDo_API.Handlers
{
    public class CompleteToDoHandler : IRequestHandler<CompleteToDoCommand, Response<bool>>
    {
        private readonly ToDoRepository _repository;

        public CompleteToDoHandler(ToDoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<bool>> Handle(CompleteToDoCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the ToDo item by ID
            var existingItem = await _repository.GetByIdAsync(request.Id);

            // If the item is not found, return failure
            if (existingItem == null)
            {
                return Response<bool>.Failure("ToDo item not found", System.Net.HttpStatusCode.NotFound);
            }

            // Mark the item as completed
            existingItem.Completed = true;

            // Update the changes in the repository
            await _repository.UpdateAsync(existingItem);

            return Response<bool>.CreateSuccess(true, "ToDo item marked as complete");
        }
    }
}
