using AutoMapper;
using WorkTimeManager.Api.Dtos;
using WorkTimeManager.Api.Models;

namespace WorkTimeManager.Api.Profiles
{
    public class TasksProfile : Profile
    {
        public TasksProfile()
        {
            CreateMap<Task, TaskReadDto>();
            CreateMap<Subtask, SubtaskReadDto>();
            CreateMap<TaskCreateDto, Task>();
        }
    }
}
