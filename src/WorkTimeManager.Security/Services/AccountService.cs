using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkTimeManager.Core.Interfaces;
using WorkTimeManager.Core.Models;
using WorkTimeManager.Security.Models;

namespace WorkTimeManager.Security.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppUserRole> _roleManager;
        private readonly IValidator<AppUser> _validator;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountService(UserManager<AppUser> userManager,
                              RoleManager<AppUserRole> roleManager,
                              IValidator<AppUser> validator,
                               SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _validator = validator;
            _signInManager = signInManager;
        }

        public async Task<Response> AddToRoleAsync(string username, string roleName)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return new Response() { Message = $"Не найден пользователь: {username}" };

            var roleResult = await _roleManager.FindByNameAsync(roleName);
            if (roleResult == null)
                return new Response() { Message = $"Не существует роли: {roleName}" };

            await _userManager.AddToRoleAsync(user, roleName);

            return new Response() { Message = $"Добавлена роль {roleName} для пользователя {username}", Success = true };
        }

        public async Task<Response> CreateRoleAsync(string roleName)
        {
            var response = new Response();

            if (string.IsNullOrEmpty(roleName))
            {
                response.Message = "Название роли не может быть пустой!";
                response.Success = false;
                return response;
            }

            var role = roleName.ToLower();

            var createRoleResult = await _roleManager.CreateAsync(new AppUserRole() { Name = role });

            if (createRoleResult.Succeeded)
            {
                response.Message = $"Роль {role} успешно создана";
                response.Success = true;
                return response;
            }


            response.Message = createRoleResult.Errors.Aggregate("Ошибка при создании роли. Errors: \n\r\n\r",
                (current, error) => current + (" - " + error.Description + "\n\r"));
            response.Success = false;
            return response;
        }

        public async Task<Response> DeleteRoleAsync(string roleName)
        {
            var response = new Response();
            if (string.IsNullOrEmpty(roleName))
            {
                response.Message = "Название роли не может быть пустой!";
                response.Success = false;
                return response;
            }

            var role = roleName.ToLower();

            var getRoleResult = await _roleManager.FindByNameAsync(role);
            if (getRoleResult == null)
            {
                response.Message = $"Не существует роли {role}!";
                response.Success = false;
                return response;
            }

            var deleteRoleResult = await _roleManager.DeleteAsync(getRoleResult);
            if (deleteRoleResult.Succeeded)
            {
                response.Message = $"Роль {role} удалена";
                response.Success = true;
                return response;
            }

            response.Message = deleteRoleResult.Errors.Aggregate("Ошибка при удалении роли. Errors: \n\r\n\r",
                (current, error) => current + (" - " + error.Description + "\n\r"));
            response.Success = false;
            return response;

        }

        /// <summary>
        /// Generates JWT for user with roles inside claim data
        /// </summary>
        /// <param name="username"></param>
        /// <returns>JSON Web Token</returns>
        public async Task<Token> GenerateTokenAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return null;

            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString())
            };

            foreach(var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var genToken = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECURITY_KEY"))),
                        SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            var token = new Token()
            {
                Username = user.UserName,
                Roles = userRoles.ToList(),
                Value = new JwtSecurityTokenHandler().WriteToken(genToken)
            };

            return token;

        }

        public async Task<Response> LoginUserAsync(AccountRegister account)
        {
            var user = await _userManager.FindByNameAsync(account.Username);
            if (user == null)
                return new Response() { Message = "Неверный логин или пароль" };

            var signinResult = await _signInManager.PasswordSignInAsync(user, account.Password, false, false);
            if (signinResult.Succeeded)
            {
                var tokenResult = await GenerateTokenAsync(user.UserName);
                return new Response() { Message = tokenResult.Value, Success = true };
            }

            return new Response() { Message = "Неверный логин или пароль" };
        }

        public async Task<Response> RegisterUserAsync(AccountRegister account)
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
