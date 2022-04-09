using WorkTimeManager.Core.Models.Base;

namespace WorkTimeManager.Core.Models
{
    public class TaskEntity : BaseEntity
    {
        public TaskEntity()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        
        public bool IsFavorites { get; set; } = false;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool Completed { get; set; } = false;
        public DateTime Deadline { get; set; }
        public Guid UserId { get; set; }
        public List<Subtask>? Subtasks { get; set; }
    }
}
