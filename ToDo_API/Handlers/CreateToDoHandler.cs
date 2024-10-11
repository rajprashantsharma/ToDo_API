using MediatR;
using ToDo_API.Commands;
using ToDo_API.Models;
using ToDo_API.Repositories;
using ToDo_API.Responses;

namespace ToDo_API.Handlers
{
    public class CreateToDoHandler : IRequestHandler<CreateToDoCommand, Response<ToDoItem>>
    {
        private readonly ToDoRepository _repository;

        public CreateToDoHandler(ToDoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<ToDoItem>> Handle(CreateToDoCommand request, CancellationToken cancellationToken)
        {
            var newToDo = new ToDoItem
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Completed = request.Completed
            };

            await _repository.AddAsync(newToDo);
            return Response<ToDoItem>.CreateSuccess(newToDo);
        }
    }
}
