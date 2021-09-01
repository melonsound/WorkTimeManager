using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkTimeManager.Api.Helpers;
using WorkTimeManager.Api.Models;

namespace WorkTimeManager.Api.Services
{
    public interface IAccountService
    {
        Account Authenticate(string username, string password);
        Account Register(string username, string password);
        IEnumerable<Account> GetAll();
        Account GetById(int id);
        string UploadImage(IFormFile files, string guid);
    }

    public class AccountService : IAccountService
    {
        private AccountContext _accountContext = null;
        private readonly IOptions<AuthOptions> _authOptions;
        private static IWebHostEnvironment _environment;

        public AccountService(AccountContext accountContext, IOptions<AuthOptions> authOptions, IWebHostEnvironment environment)
        {
            _accountContext = accountContext;
            _authOptions = authOptions;
            _environment = environment;
        }

        public Account Register (string username, string password)
        {
            //Проверка на наличие уже зарегистрированного аккаунта
            var currentAccount = _accountContext.Accounts.SingleOrDefault(x => x.Username == username);

            if (currentAccount != null)
                return null;

            var account = new Account();
            account.Username = username;
            account.Password = BCrypt.Net.BCrypt.HashPassword(password);
            account.Role = Role.User;
            account.AccountCreatedDate = DateTime.Now;

            _accountContext.Accounts.Add(account);
            _accountContext.SaveChanges();

            return account;
        }

        public Account Authenticate(string username, string password)
        {
            var account = _accountContext.Accounts.SingleOrDefault(x => x.Username == username);

            if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.Password))
                return null;

            var authParams = _authOptions.Value;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = authParams.GetSymmetricSecurityKey();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, account.Id.ToString()),
                    new Claim(ClaimTypes.Role, account.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            account.Token = tokenHandler.WriteToken(token);

            return account;
        }

        public IEnumerable<Account> GetAll()
        {
            throw new NotImplementedException();
        }

        public Account GetById(int id)
        {
            throw new NotImplementedException();
        }

        public string UploadImage(IFormFile files, string guid)
        {
            if (files.Length > 0)
            {
                var account = _accountContext.Accounts.SingleOrDefault(x => x.Id.ToString().Equals(guid));

                string folder = "/uploads/";
                string fileExt = Path.GetExtension(files.FileName);
                string fileName = Path.GetRandomFileName() + fileExt;
                string filePath = folder + fileName;
                if (!Directory.Exists(_environment.WebRootPath + folder))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + folder);

                }
                using (FileStream filestream = File.Create(_environment.WebRootPath + filePath))
                {
                    if (File.Exists(_environment.WebRootPath + account.ProfileImage))
                        File.Delete(_environment.WebRootPath + account.ProfileImage);

                    account.ProfileImage = filePath;
                    _accountContext.Accounts.Update(account);
                    _accountContext.SaveChanges();

                    files.CopyTo(filestream);
                    filestream.Flush();
                    return filePath;
                }
            }
            return null;
        }
    }
}
