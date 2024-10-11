using MediatR;
using ToDo_API.Models;
using ToDo_API.Queries;
using ToDo_API.Repositories;
using ToDo_API.Responses;

namespace ToDo_API.Handlers
{
    public class GetToDoByIdHandler : IRequestHandler<GetToDoByIdQuery, Response<ToDoItem>>
    {
        private readonly ToDoRepository _repository;

        public GetToDoByIdHandler(ToDoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<ToDoItem>> Handle(GetToDoByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null)
            {
                return Response<ToDoItem>.Failure("ToDo item not found", System.Net.HttpStatusCode.NotFound);
            }
            return Response<ToDoItem>.CreateSuccess(item);
        }
    }
}
