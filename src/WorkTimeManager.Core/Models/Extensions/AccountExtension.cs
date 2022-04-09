using WorkTimeManager.Core.Models.Dto;

namespace WorkTimeManager.Core.Models.Extensions
{
    public static class AccountExtension
    {
        public static AccountDto ToDto(this Account account)
        {
            return new AccountDto
            {
                Id = account.Id,
                CreatedAt = account.CreatedAt,
                UpdatedAt = account.UpdatedAt,
                AppUserId = account.AppUserId,
                Image = account.Image,
                UserName = account.UserName
            };
        }
    }
}
