namespace TaskManager.Models
{
    public class AppTaskModel
    {
        public int Id { get; set; }

        public required string Title { get; set; } 
        public required string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public required int Priority { get; set; }

        public int ProjectId { get; set; }
        public ProjectModel? Project { get; set; }

        public string ProjectName { get; set; }
    }

}
