using MediatR;
using ToDo_API.Models;
using ToDo_API.Responses;

namespace ToDo_API.Queries
{
    public record GetFilteredToDosQuery(bool? completed, string sortBy, string sortOrder) : IRequest<Response<List<ToDoItem>>>;
}
