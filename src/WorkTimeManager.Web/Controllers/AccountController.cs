using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        [Authorize]
        [HttpGet]
        public async Task<AccountResponse> Get()
        {
            var accountResult = await _accountService.AccountAsync(GetUserId());
            return accountResult;
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

        private Guid GetUserId()
        {
            return new Guid(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [Authorize]
        [HttpPut("upload-image")]
        public async Task<Response> UploadUserImage(IFormFile imageFile)
        {
            var response = await _accountService.UploadUserImageAsync(imageFile, GetUserId());
            return response;
        }
    }
}
