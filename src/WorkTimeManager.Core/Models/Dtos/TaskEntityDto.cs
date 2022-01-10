using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Core.Models.Dtos
{
    public class TaskEntityDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool Completed { get; set; }
        public DateTime Deadline { get; set; }
        public Guid? UserId { get; set; }
        public List<SubtaskDto>? Subtasks { get; set; }

    }
}
