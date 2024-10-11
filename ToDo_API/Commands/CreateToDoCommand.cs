using MediatR;
using ToDo_API.Models;
using ToDo_API.Responses;

namespace ToDo_API.Commands
{
    public record CreateToDoCommand(string Title, string Description, DateTime? DueDate, bool Completed) : IRequest<Response<ToDoItem>>;
}
