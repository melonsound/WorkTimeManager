using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkTimeManager.Core.Interfaces;
using WorkTimeManager.Core.Models;
using WorkTimeManager.Core.Models.Dto;
using WorkTimeManager.Infrastructure.Data;

namespace WorkTimeManager.Infrastructure.Services
{
    // #TaskService
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<TaskService> _logger;
        private readonly IValidator<TaskEntity> _validator;

        public TaskService(AppDbContext appDbContext,
                           ILogger<TaskService> logger,
                           IValidator<TaskEntity> validator)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _validator = validator;

        }

        public async Task<Response> AddToFavoriteAsync(int taskId, Guid userId)
        {
            var task = await _appDbContext.Tasks
                .Include(s => s.Subtasks)
                .SingleOrDefaultAsync(x => x.Id == taskId && x.UserId == userId);

            if (task == null)
                return new Response() { Message = $"Не найдена задача [ID: {taskId}]." };

            task.IsFavorites = true;
            _appDbContext.Tasks.Update(task);
            await _appDbContext.SaveChangesAsync();

            return new Response() { Message = $"Задача [ID: {taskId}] добавлена в избранное.", Success = true };

        }

        public async Task<Response> CreateTaskAsync(PostTaskEntityDto taskEntity, Guid userId)
        {
            var taskEntityAdapt = taskEntity.Adapt<TaskEntity>();
            taskEntityAdapt.UserId = userId;
            var validateResult = _validator.Validate(taskEntityAdapt);

            if (!validateResult.IsValid)
                return new Response() { Message = validateResult.ToString() };

            var addResult = await _appDbContext.Tasks.AddAsync(taskEntityAdapt);
            await _appDbContext.SaveChangesAsync();
            _logger.LogDebug($"Created task {taskEntity.Title}");

            return new Response() { Message = $"Задача [{taskEntityAdapt.Title} ID: {taskEntityAdapt.Id}] добавлена", Success = true };
        }

        public async Task<Response> DeleteTaskAsync(int taskId, Guid userId)
        {
            var task = await _appDbContext.Tasks.FindAsync(taskId);
            if (task == null || task.UserId != userId)
                return new Response() { Message = "Не найдена задача" };

            _appDbContext.Tasks.Remove(task);
            await _appDbContext.SaveChangesAsync();
            return new Response() { Message = $"Задача [{task.Title}] удалена", Success = true };
        }

        public async Task<List<TaskEntityDto>?> GetAllTasksAsync(Guid userId)
        {
            var tasks = await _appDbContext.Tasks
                .Include(s => s.Subtasks)
                .Where(x => x.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            if(tasks.Count == 0)
                return null;

            var adaptTasks = tasks.Adapt<List<TaskEntityDto>>();

            return adaptTasks;
        }

        public async Task<TaskEntityDto?> GetTaskByIdAsync(int taskId, Guid userId)
        {
            var task = await _appDbContext.Tasks
                .Include(s => s.Subtasks)
                .SingleOrDefaultAsync(x => x.Id == taskId);
            if (task == null || task.UserId != userId)
                return null;

            return task.Adapt<TaskEntityDto>();
        }

        public async Task<Response> RemoveFromFavoriteAsync(int taskId, Guid userId)
        {
            var task = await _appDbContext.Tasks
                .Include(s => s.Subtasks)
                .SingleOrDefaultAsync(x => x.Id == taskId && x.UserId == userId);

            if (task == null)
                return new Response() { Message = $"Не найдена задача [ID: {taskId}]." };

            task.IsFavorites = false;
            _appDbContext.Tasks.Update(task);
            await _appDbContext.SaveChangesAsync();

            return new Response() { Message = $"Задача [ID: {taskId}] удалена из избранного.", Success = true };
        }

        public async Task<Response> UpdateTaskAsync(UpdateTaskEntityDto taskEntity, Guid userId)
        {
            var task = await _appDbContext.Tasks
                .Include(s => s.Subtasks)
                .SingleOrDefaultAsync(x => x.Id == taskEntity.Id && x.UserId == userId);

            if (task == null)
                return new Response() { Message = $"Не найдена задача [ID: {taskEntity.Id}]." };

            task.Title = taskEntity.Title;
            task.Description = taskEntity.Description;
            task.Deadline = task.Deadline;
            task.IsFavorites = taskEntity.IsFavorites;
            task.Completed = taskEntity.Completed;
            task.Subtasks = taskEntity.Subtasks.Adapt<List<Subtask>>();
            _appDbContext.Tasks.Update(task);
            await _appDbContext.SaveChangesAsync();

            return new Response() { Message = $"Задача [ID: {taskEntity.Id}] обновлена.", Success = true };
        }
    }
}
