namespace WorkTimeManager.Core.Models.Dto
{
    public class AccountDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid AppUserId { get; set; }
        public string? UserName { get; set; }
        public string? Image { get; set; }
    }
}
