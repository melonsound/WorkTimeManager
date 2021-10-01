using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTimeManager.Api.Models;

namespace WorkTimeManager.Api.Dtos
{
    public class TaskCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }

        public List<SubtaskCreateDto> Subtasks { get; set; }
    }
}
