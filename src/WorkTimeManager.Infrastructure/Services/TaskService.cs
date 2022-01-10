using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Core.Interfaces;
using WorkTimeManager.Core.Models;
using WorkTimeManager.Core.Models.Dtos;
using WorkTimeManager.Infrastructure.Data;

namespace WorkTimeManager.Infrastructure.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _appDbContext;
        //private readonly ILogger _logger;

        public TaskService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            //_logger = logger;
        }

        public async Task<TaskEntityDto> CreateTaskAsync(TaskEntityDto taskEntity)
        {
            if(taskEntity == null)
                return null;

            await _appDbContext.Tasks.AddAsync(taskEntity.Adapt<TaskEntity>());
            await _appDbContext.SaveChangesAsync();
            //_logger.LogDebug("----------Created task----------");

            return taskEntity;
        }

        public async Task<bool> DeleteTaskAsync(int taskId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TaskEntityDto>> GetAllTasksAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<TaskEntityDto> GetTaskByIdAsync(int taskId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<TaskEntityDto> UpdateTaskAsync(TaskEntityDto taskEntity, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
