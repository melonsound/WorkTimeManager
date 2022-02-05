using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkTimeManager.Core.Interfaces;
using WorkTimeManager.Core.Models;
using WorkTimeManager.Security.Models;
using WorkTimeManager.Web.Controllers.Base;

namespace WorkTimeManager.Web.Controllers
{
    public class AccountController : ApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IValidator<AppUser> _validator;

        public AccountController(UserManager<AppUser> userManager,
                                 IValidator<AppUser> validator)
        {
            _userManager = userManager;
            _validator = validator;
        }

        [HttpPost("register")]
        public async Task<Response> Register(AccountRegister account)
        {
            var accountResult = await _userManager.FindByNameAsync(account.Username);
            var response = new Response();

            if (accountResult != null)
            {
                response.Success = false;
                response.Message = "User already registered";
                return response;
            }

            var userForValidate = new AppUser()
            {
                UserName = account.Username,
                PasswordHash = account.Password
            };

            var validatorResult = await _validator.ValidateAsync(userForValidate);
            if (!validatorResult.IsValid)
            {
                response.Success = false;
                response.Message = validatorResult.ToString();
                return response;
            }

            var newAccount = new AppUser()
            {
                UserName = account.Username
            };


            var regResult = await _userManager.CreateAsync(newAccount, account.Password);
            if (regResult.Succeeded)
            {
                response.Success = true;
                response.Message = regResult.ToString();
                return response;
            }

            var errorText = regResult.Errors.Aggregate("Ошибка при регистрации пользователя. Errors: \n\r\n\r",
                (current, error) => current + (" - " + error.Description + "\n\r"));

            response.Success = false;
            response.Message = errorText;
            return response;
        }

    }
}
