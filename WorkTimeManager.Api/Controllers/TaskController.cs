using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using WorkTimeManager.Api.Data;
using WorkTimeManager.Api.Dtos;
using WorkTimeManager.Api.Models;
using WorkTimeManager.Api.Services;

namespace WorkTimeManager.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskRepository _taskRepository;
        private IMapper _mapper;

        public TaskController(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a task assigned to the user | 
        /// Создать задачу закрепленную за пользователем
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public IActionResult CreateTask([FromBody]Task[] tasks)
        {
            var guid = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            _taskRepository.CreateTask(tasks, new Guid(guid));
            _taskRepository.SaveChanges();
            
            return Ok();
        }

        /// <summary>
        /// Get all tasks assigned to the user | 
        /// Получить все задачи закрепленные за пользователем
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("all")]
        public IActionResult GetAllTasks(Guid userId)
        {
            var guid = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            var tasks = _taskRepository.GetAllTasks(new Guid(guid));

            if (tasks == null)
                return BadRequest(new { message = "нет задач" });

            return Ok(_mapper.Map<IEnumerable<TaskReadDto>>(tasks));
        }

        /// <summary>
        /// Get task by task id |
        /// Получить задачу по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetTask(int id)
        {
            var guid = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            var result = _taskRepository.GetTask(id, new Guid(guid));

            if (result == null)
                return BadRequest(new { message = "Не найдена задача" });

            return Ok(result);
        }

        /// <summary>
        /// Change task |
        /// Изменить задачу
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public IActionResult UpdateTask([FromBody]Task task)
        {
            var guid = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            var updateTaskResult = _taskRepository.UpdateTask(task, new Guid(guid));

            if (updateTaskResult == null)
                return BadRequest(new { message = "Не найдена задача" });

            _taskRepository.SaveChanges();

            return Ok(updateTaskResult);
        }

        /// <summary>
        /// Delete task |
        /// Удалить задачу
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPost("delete/{id}")]
        public IActionResult DeleteTask(int id)
        {
            var guid = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            var deleteTaskResult = _taskRepository.DeleteTask(id, new Guid(guid));

            if (!deleteTaskResult)
                return BadRequest( new { message = "не найдена задача" });

            _taskRepository.SaveChanges();
            return Ok(deleteTaskResult);
        }
    }
}
