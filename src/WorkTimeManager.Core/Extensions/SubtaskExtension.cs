using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Core.Models;
using WorkTimeManager.Core.Models.Dtos;

namespace WorkTimeManager.Core.Extensions
{
    public static class SubtaskExtension
    {
        public static SubtaskDto ToDto(this Subtask subtask)
        {
            return new SubtaskDto()
            {
                Id = subtask.Id,
                Title = subtask.Title,
                Completed = subtask.Completed,
            };
        }
    }
}
