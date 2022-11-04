using WorkTimeManager.Core.Models;
using WorkTimeManager.Core.Models.Dto;

namespace WorkTimeManager.Core.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskEntityDto?> GetTaskByIdAsync(int taskId, Guid userId);
        Task<List<TaskEntityDto>?> GetAllTasksAsync(Guid userId);
        Task<Response> CreateTaskAsync(PostTaskEntityDto taskEntity, Guid userId);
        Task<Response> UpdateTaskAsync(UpdateTaskEntityDto taskEntity, Guid userId);
        Task<Response> DeleteTaskAsync(int taskId, Guid userId);
        Task<Response> AddToFavoriteAsync(int taskId, Guid userId);
        Task<Response> RemoveFromFavoriteAsync(int taskId, Guid userId);
        Task<bool> SaveChangesAsync();
    }
}
