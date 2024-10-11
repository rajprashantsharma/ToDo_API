using MediatR;
using ToDo_API.Commands;
using ToDo_API.Repositories;
using ToDo_API.Responses;

namespace ToDo_API.Handlers
{
    public class DeleteToDoHandler : IRequestHandler<DeleteToDoCommand, Response<bool>>
    {
        private readonly ToDoRepository _repository;

        public DeleteToDoHandler(ToDoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<bool>> Handle(DeleteToDoCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null)
            {
                return Response<bool>.Failure("ToDo item not found", System.Net.HttpStatusCode.NotFound);
            }

            await _repository.DeleteAsync(request.Id);
            return Response<bool>.CreateSuccess(true);
        }
    }

}
