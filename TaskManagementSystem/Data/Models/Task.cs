using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagementSystem.Data.Enum;

namespace TaskManagementSystem.Data.Models
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public StatusEnum Status { get; set; }  = StatusEnum.Pending;
    }
}
