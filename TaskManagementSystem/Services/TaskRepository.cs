using AutoMapper;
using TaskManagementSystem.Data;
using TaskManagementSystem.Data.Models;

namespace TaskManagementSystem.Services
{
    public class TaskRepository : ITaskRepository
    {
        private TaskDbContext _taskDbContext;
        private readonly IMapper _mapper;

        public TaskRepository(TaskDbContext taskDbContext, IMapper mapper) 
        {
            _taskDbContext = taskDbContext;
            _mapper = mapper;
        }
        public TaskEntity Get(int id)
        {
            return _taskDbContext.Tasks.Where(t => t.Id == id).FirstOrDefault();
        }

        public List<TaskEntity> GetAllTasks()
        {
            var tasks = _taskDbContext.Tasks.ToList();
            return tasks;
        }

        public TaskEntity AddTask(AddTaskDTO task)
        {
            var taskEnt = _mapper.Map<TaskEntity>(task);
            var newTask = _taskDbContext.Tasks.Add(taskEnt);
            _taskDbContext.SaveChanges();
            
            return newTask.Entity;
        }

        public void UpdateTask(TaskEntity task)
        {
            _taskDbContext.Tasks.Update(task);
            _taskDbContext.SaveChanges();
        }

        public void DeleteTask(TaskEntity task)
        {
            _taskDbContext.Tasks.Remove(task);
            _taskDbContext.SaveChanges();
        }
    }
}
