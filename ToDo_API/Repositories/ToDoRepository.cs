using ToDo_API.Models;

namespace ToDo_API.Repositories
{
    public class ToDoRepository
    {
        private readonly List<ToDoItem> _toDoItems = new List<ToDoItem>();

        public async Task<IEnumerable<ToDoItem>> GetAllAsync()
        {
            return await Task.FromResult(_toDoItems);
        }

        public async Task<ToDoItem> GetByIdAsync(Guid id)
        {
            var item = _toDoItems.FirstOrDefault(t => t.Id == id);
            return await Task.FromResult(item);
        }

        public async Task AddAsync(ToDoItem item)
        {
            _toDoItems.Add(item);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(ToDoItem item)
        {
            var existingItem = _toDoItems.FirstOrDefault(t => t.Id == item.Id);
            if (existingItem != null)
            {
                existingItem.Title = item.Title;
                existingItem.Description = item.Description;
                existingItem.DueDate = item.DueDate;
                existingItem.Completed = item.Completed;
            }
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = _toDoItems.FirstOrDefault(t => t.Id == id);
            if (item != null)
            {
                _toDoItems.Remove(item);
            }
            await Task.CompletedTask;
        }
    }

}
