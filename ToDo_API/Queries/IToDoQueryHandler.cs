using ToDo_API.Models;

namespace ToDo_API.Queries
{
    public interface IToDoQueryHandler
    {
        Task<IEnumerable<ToDoItem>> GetAllAsync();
        Task<ToDoItem> GetByIdAsync(Guid id);
    }
}
