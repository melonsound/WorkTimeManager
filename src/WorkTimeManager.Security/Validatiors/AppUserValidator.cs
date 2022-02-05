using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Security.Models;

namespace WorkTimeManager.Security.Validatiors
{
    public class AppUserValidator : AbstractValidator<AppUser>
    {
        private readonly IdentityOptions _identityOptions;

        public AppUserValidator(IOptions<IdentityOptions> identityOptions)
        {
            _identityOptions = identityOptions.Value;

            var minPasswordLength = _identityOptions.Password.RequiredLength;
            var minUsernameLength = 5;

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage(ErrorMessage.UsernameLength(minUsernameLength))
                .NotNull().WithMessage(ErrorMessage.PasswordLength(minUsernameLength))
                .MinimumLength(minUsernameLength);

            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage(ErrorMessage.PasswordLength(minPasswordLength))
                .NotNull().WithMessage(ErrorMessage.PasswordLength(minPasswordLength))
                .MinimumLength(minPasswordLength).WithMessage(ErrorMessage.PasswordLength(minUsernameLength))
                .Matches("[A-Z]").WithMessage(ErrorMessage.PasswordUppercase)
                .Matches("[a-z]").WithMessage(ErrorMessage.PasswordLowercase)
                .Matches("[0-9]").WithMessage(ErrorMessage.PasswordDigit);


        }
    }
}
