using Microsoft.EntityFrameworkCore;

namespace Activity.Models
{
    public class ActivityContext : DbContext
    {
        public ActivityContext(DbContextOptions options) : base(options) {}
        public DbSet<ActivityUser> Users {get;set;}
        public DbSet<Activity> Activities {get;set;}
        public DbSet<Response> Responses {get;set;}
    }
}