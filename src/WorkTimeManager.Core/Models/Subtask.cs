using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Core.Models.Base;

namespace WorkTimeManager.Core.Models
{
    public class Subtask : BaseEntity
    {
        public Subtask()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        public string? Title { get; set; }
        public bool Completed { get; set; }
        public int TaskEntityId { get; set; }
        public TaskEntity TaskEntity { get; set; }
    }
}
