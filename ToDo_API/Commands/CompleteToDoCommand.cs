using MediatR;
using ToDo_API.Models;
using ToDo_API.Responses;

namespace ToDo_API.Commands
{
    public record CompleteToDoCommand(Guid Id) : IRequest<Response<bool>>;
}
