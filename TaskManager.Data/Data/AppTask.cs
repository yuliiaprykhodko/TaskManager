namespace TaskManager.Models
{
    public class AppTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int Priority { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!; 
    }



}
