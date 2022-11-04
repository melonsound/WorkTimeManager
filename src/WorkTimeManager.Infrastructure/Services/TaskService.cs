using WorkTimeManager.Core.Interfaces;
using WorkTimeManager.Core.Models;
using WorkTimeManager.Core.Models.Dto;

namespace WorkTimeManager.Infrastructure.Services
{
    // #TaskService
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Response> AddToFavoriteAsync(int taskId, Guid userId) => await _taskRepository.AddToFavoriteAsync(taskId, userId);

        public async Task<Response> CreateTaskAsync(PostTaskEntityDto taskEntity, Guid userId) => await _taskRepository.CreateTaskAsync(taskEntity, userId);

        public async Task<Response> DeleteTaskAsync(int taskId, Guid userId) => await _taskRepository.DeleteTaskAsync(taskId, userId);

        public async Task<List<TaskEntityDto>?> GetAllTasksAsync(Guid userId) => await _taskRepository.GetAllTasksAsync(userId);

        public async Task<TaskEntityDto?> GetTaskByIdAsync(int taskId, Guid userId) => await _taskRepository.GetTaskByIdAsync(taskId, userId);

        public async Task<Response> RemoveFromFavoriteAsync(int taskId, Guid userId) => await _taskRepository.RemoveFromFavoriteAsync(taskId, userId);

        public async Task<Response> UpdateTaskAsync(UpdateTaskEntityDto taskEntity, Guid userId) => await _taskRepository.UpdateTaskAsync(taskEntity, userId);
    }
}
