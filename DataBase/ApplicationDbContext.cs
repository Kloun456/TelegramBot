using CoffeBot.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeBot.DataBase
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserDb> Users { get; set; }

        public DbSet<Cup> Cups { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions)
        : base(contextOptions)
        {
            
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
