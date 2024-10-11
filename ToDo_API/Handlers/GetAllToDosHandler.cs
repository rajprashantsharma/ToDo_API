using MediatR;
using ToDo_API.Models;
using ToDo_API.Queries;
using ToDo_API.Repositories;
using ToDo_API.Responses;

namespace ToDo_API.Handlers
{
    public class GetAllToDosHandler : IRequestHandler<GetAllToDosQuery, Response<List<ToDoItem>>>
    {   
        private readonly ToDoRepository _repository;

        public GetAllToDosHandler(ToDoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<List<ToDoItem>>> Handle(GetAllToDosQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAllAsync();
            return Response<List<ToDoItem>>.CreateSuccess(items.ToList());
        }
    }
}
