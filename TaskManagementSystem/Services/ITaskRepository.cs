using TaskManagementSystem.Data.Models;

namespace TaskManagementSystem.Services
{
    public interface ITaskRepository
    {
        TaskEntity Get(int id);
        List<TaskEntity> GetAllTasks();
        TaskEntity AddTask(AddTaskDTO task);
        void UpdateTask(TaskEntity task);

        void DeleteTask(TaskEntity task);

    }
}
