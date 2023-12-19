using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Data.Enum;

namespace TaskManagementSystem.Data.Models
{
    public class TaskDTO : AddTaskDTO
    {
        public int Id { get; set; }
    }
}
