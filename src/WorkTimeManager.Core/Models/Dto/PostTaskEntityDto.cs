using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Core.Models.Dto
{
    public class PostTaskEntityDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime Deadline { get; set; }
        public List<PostSubtaskDto>? Subtasks { get; set; }
    }
}
