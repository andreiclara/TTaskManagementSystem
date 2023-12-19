using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Diagnostics;
using System.Net;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Data;
using TaskManagementSystem.Data.Models;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Tests
{
    public class Tests
    {
        IMapper _mapper;
        MapperConfiguration _config;
        TaskDbContext _dbContext;
        TaskRepository _repo;
        Mock<HttpMessageHandler> mockHttpMessageHandler;

        [SetUp]
        public void Setup()
        {
            _config = new MapperConfiguration(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
            _mapper = _config.CreateMapper();
            var config = new Mock<IConfiguration>();
            _dbContext = new TaskDbContext(config.Object);
            _repo = new TaskRepository(_dbContext, _mapper);

            
            var mockFactory = new Mock<IHttpClientFactory>();
            mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            

        }

        [Test]
        public void RepositoryTests()
        {
            AddTaskDTO addTaskDTO = new AddTaskDTO()
            {
                Description = "description",
                DueDate = new DateTime(2023, 12, 19, 0, 0, 0),
                Status = Data.Enum.StatusEnum.Pending,
                Title = "title"
            };

            var addedTask = _repo.AddTask(addTaskDTO);
            var addedTaskDTO = _mapper.Map<AddTaskDTO>(addedTask);

            Assert.AreEqual(addedTaskDTO.Description, addTaskDTO.Description);
            Assert.AreEqual(addedTaskDTO.Title, addTaskDTO.Title);
            Assert.AreEqual(addedTaskDTO.DueDate, addTaskDTO.DueDate);
            Assert.AreEqual(addedTaskDTO.Status, addTaskDTO.Status);

            addedTask.Status = Data.Enum.StatusEnum.InProgress;
            _repo.UpdateTask(addedTask);
            var updatedTask = _repo.Get(addedTask.Id);
            Assert.IsTrue(updatedTask.Status == Data.Enum.StatusEnum.InProgress);

            _repo.DeleteTask(updatedTask);
            var allTasks = _repo.GetAllTasks();
            Assert.IsEmpty(allTasks);
        }

        [Test]
        public void ControllerTests()
        {
            var logger = new Mock<ILogger<TaskManagementController>>();
            var controller = new TaskManagementController(logger.Object, _repo, _mapper);

            AddTaskDTO addTaskDTO = new AddTaskDTO()
            {
                Description = "description",
                DueDate = new DateTime(2023, 12, 19, 0, 0, 0),
                Status = Data.Enum.StatusEnum.Pending,
                Title = "title"
            };

            CreatedAtRouteResult linkToAddedTask = (CreatedAtRouteResult)controller.AddTask(addTaskDTO);
            var id = ((TaskDTO)linkToAddedTask.Value).Id;
            var addedTask = _mapper.Map<TaskEntity>(linkToAddedTask.Value);

            Assert.AreEqual(addedTask.Description, addTaskDTO.Description);
            Assert.AreEqual(addedTask.Title, addTaskDTO.Title);
            Assert.AreEqual(addedTask.DueDate, addTaskDTO.DueDate);
            Assert.AreEqual(addedTask.Status, addTaskDTO.Status);

            addedTask.Status = Data.Enum.StatusEnum.InProgress;
             
            controller.UpdateTask(id, _mapper.Map<TaskDTO>(addedTask));

            ActionResult<TaskDTO> updatedTask = controller.GetTask(id);
            Assert.IsTrue(((TaskDTO)((OkObjectResult)updatedTask.Result).Value).Status == Data.Enum.StatusEnum.InProgress);

            controller.DeleteTask(id);
            var allTasks = controller.GetTasks();
            Assert.IsEmpty(((List<TaskDTO>)((OkObjectResult)allTasks.Result).Value));
        }
    }
}