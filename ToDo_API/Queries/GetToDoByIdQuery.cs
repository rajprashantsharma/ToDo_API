using MediatR;
using ToDo_API.Models;
using ToDo_API.Responses;

namespace ToDo_API.Queries
{
    //public class GetToDoByIdQuery : IRequest<Response<ToDoItem>>
    //{
    //    public Guid Id { get; set; }
    //}
    public record GetToDoByIdQuery(Guid Id) : IRequest<Response<ToDoItem>>;
}
