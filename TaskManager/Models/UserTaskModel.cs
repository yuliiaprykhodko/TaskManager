namespace TaskManager.Models
{
    public class UserTaskModel
    {
        public int Id { get; set; } 

        
        public required string UserId { get; set; } 

        
        public int TaskId { get; set; } 
        public required AppTaskModel Task { get; set; }
    }
}
