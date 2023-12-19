using AutoMapper;
using TaskManagementSystem.Data.Enum;

namespace TaskManagementSystem.Services
{
    public class StatusCheckService
    {
        private ITaskRepository _taskRepository;

        public StatusCheckService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public void CheckTasksStatuses()
        {
            var tasks = _taskRepository.GetAllTasks().Where(t => t.DueDate < DateTime.Now &&
                        (t.Status == StatusEnum.Pending || t.Status == StatusEnum.InProgress)).ToList();
            foreach (var task in tasks)
            {
                if (task.Status == StatusEnum.Pending)
                {
                    task.Status = StatusEnum.Overdue;
                    _taskRepository.UpdateTask(task);
                }
                else if (task.Status == StatusEnum.InProgress)
                {
                    task.Status = StatusEnum.Completed;
                    _taskRepository.UpdateTask(task);
                }
            }
        }
    }
}
