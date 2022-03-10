using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Core.Models.Base;

namespace WorkTimeManager.Core.Models
{
    public class Account : BaseEntity
    {
        public Account()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Identity user ID
        /// </summary>
        public Guid AppUserId { get; set; }
        public string? UserName { get; set; }
        public string? Image { get; set; }
    }
}
