using WorkTimeManager.Core.Models;

namespace WorkTimeManager.Core.Interfaces
{
    public interface IAccountService
    {
        Task<Token> GenerateTokenAsync(string username);
        Task<Response> RegisterUserAsync(AccountRegister account);
        Task<Response> LoginUserAsync(AccountRegister account);
        Task<Response> CreateRoleAsync(string roleName);
        Task<Response> DeleteRoleAsync(string roleName);
        Task<Response> AddToRoleAsync (string username, string roleName);

    }
}
