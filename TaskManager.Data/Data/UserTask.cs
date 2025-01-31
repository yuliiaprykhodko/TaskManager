using Microsoft.AspNetCore.Identity;

namespace TaskManager.Models
{
    public class UserTask
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string UserId { get; set; }  
        public AppTask Task { get; set; }
        public IdentityUser User { get; set; }  
    }






}
