using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkTimeManager.Core.Interfaces;
using WorkTimeManager.Core.Models;
using WorkTimeManager.Web.Controllers.Base;

namespace WorkTimeManager.Web.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<Response> Register([FromBody]AccountRegister account)
        {
            var registerResult = await _accountService.RegisterUserAsync(account);
            return registerResult;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<Response> Login([FromBody]AccountRegister account)
        {
            var loginResult = await _accountService.LoginUserAsync(account);
            return loginResult;
        }
    }
}
