using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkTimeManager.Core.Interfaces;
using WorkTimeManager.Core.Models;
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
        [Authorize]
        public async Task<Response> CreateTask([FromBody] PostTaskEntityDto taskEntity)
        {
            var result = await _taskService.CreateTaskAsync(taskEntity, GetUserId());
            return result;
        }


        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<Response> DeleteTaskById(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id, GetUserId());
            return result;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<TaskEntityDto?> GetTaskById(int id)
        {
            var result = await _taskService.GetTaskByIdAsync(id, GetUserId());
            return result;
        }

        [HttpPatch("remove-from-favorite/{id}")]
        [Authorize]
        public async Task<Response> DeleteFromFavorite(int id)
        {
            var result = await _taskService.RemoveFromFavoriteAsync(id, GetUserId());
            return result;
        }

        [HttpPatch("add-to-favorite/{id}")]
        [Authorize]
        public async Task<Response> AddToFavorite(int id)
        {
            var result = await _taskService.AddToFavoriteAsync(id, GetUserId());
            return result;
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<Response> Update([FromBody] UpdateTaskEntityDto task)
        {
            var result = await _taskService.UpdateTaskAsync(task, GetUserId());
            return result;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<TaskEntityDto>?> GetTasks()
        {
            var result = await _taskService.GetAllTasksAsync(GetUserId());
            return result;

        }

        private Guid GetUserId()
        {
            return new Guid(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
