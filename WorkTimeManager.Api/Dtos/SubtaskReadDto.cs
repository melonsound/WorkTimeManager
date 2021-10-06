using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WorkTimeManager.Api.Models;

namespace WorkTimeManager.Api.Dtos
{
    public class SubtaskReadDto
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        
        public TaskReadDto Task { get; set; }
    }
}
