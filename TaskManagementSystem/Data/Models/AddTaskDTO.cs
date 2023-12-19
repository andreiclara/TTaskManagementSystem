using TaskManagementSystem.Data.Enum;

namespace TaskManagementSystem.Data.Models
{
    public class AddTaskDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public StatusEnum? Status { get; set; }
    }
}
