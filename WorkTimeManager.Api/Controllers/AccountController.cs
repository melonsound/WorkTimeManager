using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkTimeManager.Api.Models;
using WorkTimeManager.Api.Services;

namespace WorkTimeManager.Api.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private AccountContext _accountContext = null;
        private IAccountService _accountService = null;

        public AccountController(AccountContext accountContext, IAccountService accountService)
        {
            _accountContext = accountContext;
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequired account)
        {
            var accountResult = _accountService.Register(account.Username, account.Password);

            if (accountResult == null)
                return BadRequest(new { message = "Пользователь уже зарегистрирован с таким логином" });

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] RegisterRequired account)
        {
            var accountResult = _accountService.Authenticate(account.Username, account.Password);

            if (accountResult == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(accountResult);
        }

        [Authorize]
        [HttpPost("upload-image")]
        public IActionResult UploadImage(IFormFile files)
        {
            var guid = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var accountImageResult = _accountService.UploadImage(files, guid);

            if (accountImageResult == null)
                return BadRequest();

            return Ok(accountImageResult);
        }
    }
}
