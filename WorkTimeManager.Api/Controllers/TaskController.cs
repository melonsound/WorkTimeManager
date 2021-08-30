﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using WorkTimeManager.Api.Models;
using WorkTimeManager.Api.Services;

namespace WorkTimeManager.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskService _taskService = null;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost("create")]
        public IActionResult CreateTask([FromBody]Task task)
        {
            var guid = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            var taskResult = _taskService.Create(task, new Guid(guid));
            if (taskResult == null)
                return BadRequest(new { message = "Задача не создана (пустые значения)" });
            return Ok(taskResult);
        }

        [HttpGet("all")]
        public IActionResult GetAllTasks(Guid userId)
        {
            var guid = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            var tasks = _taskService.GetAll(guid);

            if (tasks == null)
                return BadRequest(new { message = "нет задач" });

            return Ok(tasks);
        }
    }
}