namespace TaskManager.Models
{
    public class ProjectModel
    {
        public required int Id { get; set; } 
        public required string Name { get; set; } 
        public required string Description { get; set; } 

        
        public ICollection<AppTaskModel> Tasks { get; set; } = new List<AppTaskModel>();
    }
}
