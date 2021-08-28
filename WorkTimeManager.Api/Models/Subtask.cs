using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkTimeManager.Api.Models
{
    public class Subtask
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }

        public int TaskId { get; set; }
        public Task Task { get; set; }

    }
}
