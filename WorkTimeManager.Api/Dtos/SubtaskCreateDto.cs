using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkTimeManager.Api.Dtos
{
    public class SubtaskCreateDto
    {
        public string Title { get; set; }
        public bool Completed { get; set; }
    }
}
