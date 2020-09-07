using System;
using BackendServiceStarter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackendServiceStarter.Databases
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        private readonly ILoggerFactory _consoleLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        
        public ApplicationContext(DbContextOptions<ApplicationContext> context) : base(context) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_consoleLoggerFactory).EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }
    }
}