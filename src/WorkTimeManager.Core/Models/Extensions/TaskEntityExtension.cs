using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Core.Models.Dto;

namespace WorkTimeManager.Core.Models.Extensions
{
    public static class TaskEntityExtension
    {
        public static TaskEntityDto ToDto(this TaskEntity taskEntity)
        {
            return new TaskEntityDto()
            {
                Id = taskEntity.Id,
                CreatedAt = taskEntity.CreatedAt,
                UpdatedAt = taskEntity.UpdatedAt,
                Title = taskEntity.Title,
                Description = taskEntity.Description,
                Completed = taskEntity.Completed,
                Deadline = taskEntity.Deadline,
                UserId = taskEntity.UserId,
                IsFavorites = taskEntity.IsFavorites,
                Subtasks = taskEntity.Subtasks.Select(x => x.ToDto()).ToList(),
            };
        }
    }
}
