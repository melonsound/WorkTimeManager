using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Core.Models.Dto
{
    public class UpdateSubtaskDto
    {
        public string? Title { get; set; }
        public bool Completed { get; set; }
    }
}
