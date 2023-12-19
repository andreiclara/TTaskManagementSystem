using AutoMapper;
using TaskManagementSystem.Data.Enum;
using TaskManagementSystem.Data.Models;

namespace TaskManagementSystem.Tests.MapperProfiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskDTO, TaskEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<AddTaskDTO, TaskEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<TaskEntity, TaskDTO>();
        }
    }
}
