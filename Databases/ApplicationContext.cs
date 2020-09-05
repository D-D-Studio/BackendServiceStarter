using BackendServiceStarter.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendServiceStarter.Databases
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public ApplicationContext(DbContextOptions<ApplicationContext> context) : base(context) {}
    }
}