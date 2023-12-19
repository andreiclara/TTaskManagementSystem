using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data.Models;

namespace TaskManagementSystem.Data
{
    public class TaskDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public TaskDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("TaskManagementDb");
        }
        public DbSet<TaskEntity> Tasks
        {
            get;
            set;
        }
    }
}
