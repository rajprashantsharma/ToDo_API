namespace ToDo_API.Models
{
    public class ToDoItem 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public bool Completed { get; set; }
    }
}
