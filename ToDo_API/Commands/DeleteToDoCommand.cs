using MediatR;
using ToDo_API.Responses;

namespace ToDo_API.Commands
{
    public record DeleteToDoCommand(Guid Id) : IRequest<Response<bool>>;
}
