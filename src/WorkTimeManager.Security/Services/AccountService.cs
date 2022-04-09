using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkTimeManager.Core.Interfaces;
using WorkTimeManager.Core.Models;
using WorkTimeManager.Infrastructure.Data;
using WorkTimeManager.Infrastructure.Interfaces;
using WorkTimeManager.Security.Models;

namespace WorkTimeManager.Security.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppUserRole> _roleManager;
        private readonly IValidator<AppUser> _validator;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _appDbContext;
        private readonly IHostingEnvironment? _environment;
        private readonly IGoogleDriveCloudService _googleDriveService;

        public AccountService(UserManager<AppUser> userManager,
                              RoleManager<AppUserRole> roleManager,
                              IValidator<AppUser> validator,
                              SignInManager<AppUser> signInManager,
                              AppDbContext appDbContext,
                              IHostingEnvironment environment,
                              IGoogleDriveCloudService googleDriveService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _validator = validator;
            _signInManager = signInManager;
            _appDbContext = appDbContext;
            _environment = environment;
            //_googleDriveService = googleDriveService;
        }

        public async Task<AccountResponse?> AccountAsync(Guid userId)
        {
            var user = await _appDbContext.Accounts.SingleOrDefaultAsync(x => x.AppUserId == userId);

            return new AccountResponse { Image = user.Image, UserName = user.UserName };
        }

        /// <summary>
        /// Uploads the user's image. Length must be = 1
        /// </summary>
        /// <param name="files">User image</param>
        /// <returns>Image path</returns>
        public async Task<Response> UploadUserImageAsync(IFormFile files, Guid userId)
        {
            if(files.Length == 0)
                return new Response { Message = "Загружаемый файл отсутствует." };

            var account = await _appDbContext.Accounts.SingleOrDefaultAsync(x => x.AppUserId == userId);
            if (account == null)
                return new Response { Message = "Не найден пользователь." };

            var folderName = "/uploads/";
            var fileExtension = Path.GetExtension(files.FileName);
            var fileName = Path.GetRandomFileName() + fileExtension;
            var filePath = folderName + fileName;

            if (!Directory.Exists(_environment.WebRootPath + folderName))
                Directory.CreateDirectory(_environment.WebRootPath + folderName);
            
            using (FileStream filestream = File.Create(_environment.WebRootPath + filePath))
            {
                if (File.Exists(_environment.WebRootPath + account.Image))
                    File.Delete(_environment.WebRootPath + account.Image);

                await files.CopyToAsync(filestream);
                account.Image = filePath;
                _appDbContext.Accounts.Update(account);
                await _appDbContext.SaveChangesAsync();
                await filestream.FlushAsync();

                return new Response { Message = filePath, Success = true };
            }
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
        public async Task<Token?> GenerateTokenAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return null;

            var userRoles = await _userManager.GetRolesAsync(user);
            var userAccount = await _appDbContext.Accounts.SingleOrDefaultAsync(x => x.AppUserId == user.Id);
            if (userAccount == null)
                return null;

            string image = userAccount.Image == null ? "" : userAccount.Image;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("image", image),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString())
            };

            foreach(var role in userRoles)
            {
                claims.Add(new Claim("roles", role));
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
                if (tokenResult == null)
                    return new Response() { Message = "Ошибка во время генерации JWT токена." };

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
                var accountDto = new Account()
                {
                    AppUserId = newAccount.Id,
                    UserName = account.Username
                };
                await _appDbContext.Accounts.AddAsync(accountDto);
                await _appDbContext.SaveChangesAsync();

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
