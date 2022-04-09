
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkTimeManager.Core.Interfaces;
using WorkTimeManager.Core.Models;
using WorkTimeManager.Security.Models;
using WorkTimeManager.Web.Controllers.Base;

namespace WorkTimeManager.Web.Controllers
{
    // #RolesController
    public class RolesController : ApiController
    {
        private readonly IAccountService _accountService;

        public RolesController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost("create")]
        public async Task<Response> CreateRole(string roleName)
        {
            var createResult = await _accountService.CreateRoleAsync(roleName);
            return createResult;

        }

        [Authorize(Roles ="admin")]
        [HttpPost("delete")]
        public async Task<Response> DeleteRole(string roleName)
        {
            var deleteResult = await _accountService.DeleteRoleAsync(roleName);
            return deleteResult;

        }

        [Authorize(Roles = "admin")]
        [HttpPost("add-to-role")]
        public async Task<Response> AddToRole(string username, string role)
        {
            var addToRoleResult = await _accountService.AddToRoleAsync(username, role);
            return addToRoleResult;

        }

    }
}
