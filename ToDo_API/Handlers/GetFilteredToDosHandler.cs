using MediatR;
using ToDo_API.Models;
using ToDo_API.Queries;
using ToDo_API.Repositories;
using ToDo_API.Responses;

namespace ToDo_API.Handlers
{
    public class GetFilteredToDosHandler : IRequestHandler<GetFilteredToDosQuery, Response<IEnumerable<ToDoItem>>>
    {
        private readonly ToDoRepository _repository;

        public GetFilteredToDosHandler(ToDoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<ToDoItem>>> Handle(GetFilteredToDosQuery request, CancellationToken cancellationToken)
        {
            // Get all items
            var items = await _repository.GetAllAsync();

            // Apply filtering by completion status if provided
            if (request.completed.HasValue) 
            {
                items = items.Where(item => item.Completed == request.completed.Value); // Use request.completed
            }

            // Apply sorting
            if (request.sortBy == "dueDate") // Use request.sortBy
            {
                if (request.sortOrder.ToLower() == "asc") // Use request.sortOrder
                {
                    items = items.OrderBy(item => item.DueDate);
                }
                else
                {
                    items = items.OrderByDescending(item => item.DueDate);
                }
            }

            return Response<IEnumerable<ToDoItem>>.CreateSuccess(items, "ToDo items retrieved successfully");
        }
    }
}
