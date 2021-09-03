using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkTimeManager.Api.Models
{
    public class RegisterRequired
    {
        [Required]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Логин должен содержать минимум 4 символа.")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Пароль должен содержать минимум 8 символов.")]
        public string Password { get; set; }
    }
}
