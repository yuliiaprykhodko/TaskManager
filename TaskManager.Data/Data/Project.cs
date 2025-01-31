namespace TaskManager.Models
{
    public class Project
    {
        public required int Id { get; set; } 
        public required string Name { get; set; } 
        public required string Description { get; set; } 

        
        public ICollection<AppTask> Tasks { get; set; } = new List<AppTask>();
    }
}
