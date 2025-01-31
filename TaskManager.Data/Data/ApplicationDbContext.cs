using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppTask> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<UserTask>()
                .HasOne(ut => ut.User)
                .WithMany()
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            modelBuilder.Entity<UserTask>()
                .HasOne(ut => ut.Task)
                .WithMany()
                .HasForeignKey(ut => ut.TaskId)
                .IsRequired();

            
            modelBuilder.Entity<Project>().HasData(
                new Project { Id = 1, Name = "Domowy Budżet", Description = "Zarządzanie domowymi wydatkami" },
                new Project { Id = 2, Name = "Projekt IT", Description = "Tworzenie aplikacji Task Manager" }
            );

            modelBuilder.Entity<AppTask>().HasData(
                new AppTask { Id = 1, Title = "Zakupy", Description = "Kup mleko, chleb", DueDate = DateTime.Now.AddDays(1), IsCompleted = false, Priority = 3, ProjectId = 1 },
                new AppTask { Id = 2, Title = "Planowanie projektu", Description = "Sporządź plan pracy", DueDate = DateTime.Now.AddDays(2), IsCompleted = false, Priority = 2, ProjectId = 2 }
            );

            modelBuilder.Entity<IdentityUser>().HasData(
            new IdentityUser { Id = "user1", UserName = "tester", NormalizedUserName = "TESTER" }
);

            modelBuilder.Entity<AppTask>().HasData(
                new AppTask { Id = 3, Title = "Task 1", Description = "Do something", DueDate = DateTime.Now.AddDays(1) }
            );

        }

    }
}
