namespace WorkTimeManager.Core.Models.Dto
{
    public class UpdateTaskEntityDto
    {
        public int Id { get; set; }
        public bool IsFavorites { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool Completed { get; set; }
        public DateTime Deadline { get; set; }
        public List<UpdateSubtaskDto>? Subtasks { get; set; }
    }
}
