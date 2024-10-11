using MediatR;
using ToDo_API.Models;
using ToDo_API.Responses;

namespace ToDo_API.Commands
{
    namespace ToDo_API.Commands
    {
        // public record UpdateToDoCommand(Guid Id, string Title, string Description, DateTime? DueDate, bool Completed) : IRequest<Response<ToDoItem>>;

        // Wrapper update command to the actual update fields
        public record UpdateToDoCommandWrapper(Guid Id, UpdateToDoCommand Command) : IRequest<Response<ToDoItem>>;
        public record UpdateToDoCommand(string Title, string Description, DateTime? DueDate, bool Completed) : IRequest<Response<ToDoItem>>;
    }


}


