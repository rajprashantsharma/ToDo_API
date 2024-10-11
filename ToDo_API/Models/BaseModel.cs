namespace ToDo_API.Models
{
    public class BaseModel
    {
        public DateTime? DateCreated { get; set; } = System.DateTime.UtcNow;
        public DateTime? DateLastModified { get; set; } = System.DateTime.UtcNow;
    }
}
