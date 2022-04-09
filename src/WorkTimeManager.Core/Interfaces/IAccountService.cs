using Microsoft.AspNetCore.Http;
using WorkTimeManager.Core.Models;

namespace WorkTimeManager.Core.Interfaces
{
    public interface IAccountService
    {
        Task<AccountResponse?> AccountAsync(Guid userId);
        Task<Response> UploadUserImageAsync(IFormFile files, Guid userId);
        Task<Token?> GenerateTokenAsync(string username);
        Task<Response> RegisterUserAsync(AccountRegister account);
        Task<Response> LoginUserAsync(AccountRegister account);
        Task<Response> CreateRoleAsync(string roleName);
        Task<Response> DeleteRoleAsync(string roleName);
        Task<Response> AddToRoleAsync (string username, string roleName);

    }
}
