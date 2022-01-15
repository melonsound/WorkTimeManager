using Microsoft.AspNetCore.Mvc;
using WorkTimeManager.Core.Interfaces;
using WorkTimeManager.Core.Models.Dto;
using WorkTimeManager.Web.Controllers.Base;

namespace WorkTimeManager.Web.Controllers
{
    public class TaskController : ApiController
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost("create")]
        public async Task<TaskEntityDto> CreateTask([FromBody] TaskEntityDto taskEntityDto)
        {
            var result = await _taskService.CreateTaskAsync(taskEntityDto);
            if (result == null)
                throw new Exception("Error");

            return result;
        }
    }
}
