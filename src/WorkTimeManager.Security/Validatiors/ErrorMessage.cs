using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Security.Validatiors
{
    public static class ErrorMessage
    {
        public static string PasswordLength(int length) => $"Пароль должен содержать минимум {length} символов";
        public static string PasswordUppercase => $"Пароль должен содержать буквы верхнего регистра.";
        public static string PasswordLowercase => $"Пароль должен содержать буквы нижнего регистра.";
        public static string PasswordDigit => $"Пароль должен содержать цифры.";
        public static string UsernameLength(int length) => $"Логин должен содержать минимум {length} символов";
    }
}
