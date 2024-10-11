using ToDo_API.Models;
using ToDo_API.Repositories;

namespace ToDo_API.Queries
{
    public class ToDoQueryHandler : IToDoQueryHandler
    {
        private readonly ToDoRepository _repository;

        public ToDoQueryHandler(ToDoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ToDoItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ToDoItem> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
