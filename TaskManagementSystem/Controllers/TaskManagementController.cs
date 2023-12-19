using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManagementSystem.Data;
using TaskManagementSystem.Data.Enum;
using TaskManagementSystem.Data.Models;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskManagementController : ControllerBase
    {
        private readonly ILogger<TaskManagementController> _logger;
        private ITaskRepository _taskRepository;
        private IMapper _mapper;

        public TaskManagementController(ILogger<TaskManagementController> logger, ITaskRepository taskRepository, IMapper mapper)
        {
            _logger = logger;
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        [HttpGet("tasks")]
        public ActionResult<List<TaskDTO>> GetTasks()
        {
            return Ok(_mapper.Map<List<TaskDTO>>(_taskRepository.GetAllTasks()));
        }

        [HttpGet("task/{id}", Name = "GetTask")]
        public ActionResult<TaskDTO> GetTask(int id)
        {
            return Ok(_mapper.Map<TaskDTO>(_taskRepository.Get(id)));
        }

        [HttpPost("task")]
        public IActionResult AddTask([FromBody]AddTaskDTO task)
        {
            try
            {
                if (task is null)
                {
                    _logger.LogError("Task object sent from client is null.");
                    return BadRequest("Task object is null");
                }
                var newTask = _taskRepository.AddTask(task);
                return CreatedAtRoute("GetTask", new { id = newTask.Id }, _mapper.Map<TaskDTO>(newTask));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside AddTask action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("task/{id}")]
        public IActionResult UpdateTask(int id, [FromBody] TaskDTO task)
        {
            try
            {
                if (task is null)
                {
                    _logger.LogError("Task object sent from client is null.");
                    return BadRequest("Task object is null");
                }
                var oldTask = _taskRepository.Get(id);

                if (oldTask is null)
                {
                    _logger.LogError($"Task with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                
                if(task.Status != null)
                {
                    oldTask.Status = task.Status.Value;
                }
                if (task.Title != null)
                {
                    oldTask.Title = task.Title;
                }
                if (task.Description != null)
                {
                    oldTask.Description = task.Description;
                }
                if (task.DueDate != null)
                {
                    oldTask.DueDate= task.DueDate.Value;
                }

                _taskRepository.UpdateTask(oldTask);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateTask action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("task/{id}")]
        public IActionResult DeleteTask(int id)
        {
            try
            {
                var task = _taskRepository.Get(id);
                if (task == null)
                {
                    _logger.LogError($"Task with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var taskEnt = _mapper.Map<TaskEntity>(task);
                taskEnt.Id = id;

                _taskRepository.DeleteTask(taskEnt);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteTask action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}