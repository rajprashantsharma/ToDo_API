using MediatR;
using ToDo_API.Models;
using ToDo_API.Responses;

namespace ToDo_API.Queries
{
   public record GetAllToDosQuery : IRequest<Response<List<ToDoItem>>>;
}
