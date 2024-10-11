using MediatR;
using ToDo_API.Models;
using ToDo_API.Queries;
using ToDo_API.Repositories;
using ToDo_API.Responses;

namespace ToDo_API.Handlers
{
    public class GetFilteredToDosHandler : IRequestHandler<GetFilteredToDosQuery, Response<List<ToDoItem>>>
    {
        private readonly ToDoRepository _repository;

        public GetFilteredToDosHandler(ToDoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<List<ToDoItem>>> Handle(GetFilteredToDosQuery request, CancellationToken cancellationToken)
        {
            
            var items = await _repository.GetAllAsync();

            // Apply filtering by completion status if provided
            if (request.completed.HasValue) 
            {
                items = items.Where(item => item.Completed == request.completed.Value).ToList(); // Use request.completed
            }

            // Apply sorting
            if (request.sortBy == "dueDate") // Use request.sortBy
            {
                if (request.sortOrder.ToLower() == "asc") // Use request.sortOrder
                {
                    items = items.OrderBy(item => item.DueDate).ToList();
                }
                else
                {
                    items = items.OrderByDescending(item => item.DueDate).ToList();
                }
            }

            return Response<List<ToDoItem>>.CreateSuccess(items.ToList(), "ToDo items retrieved successfully");
        }
    }
}
