using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Core.Models
{
    public class Token
    {
        public string Value { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }

    }
}
