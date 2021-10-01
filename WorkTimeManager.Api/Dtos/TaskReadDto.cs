using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WorkTimeManager.Api.Models;

namespace WorkTimeManager.Api.Dtos
{
    public class TaskReadDto
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public DateTime Deadline { get; set; }

        public List<SubtaskReadDto> Subtasks { get; set; }
    }
}
