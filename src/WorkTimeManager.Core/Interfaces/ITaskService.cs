using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Core.Interfaces
{
    public interface ITaskService
    {
        Task<TaskEntityDto> GetTaskByIdAsync(int taskId, Guid userId);
        Task<List<TaskEntityDto>> GetAllTasksAsync(Guid userId);
        Task<TaskEntityDto> CreateTaskAsync(TaskEntityDto taskEntity);
        Task<TaskEntityDto> UpdateTaskAsync(TaskEntityDto taskEntity, Guid userId);
        Task<bool> DeleteTaskAsync(int taskId, Guid userId);
    }
}
