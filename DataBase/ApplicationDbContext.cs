using CoffeBot.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeBot.DataBase
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserDb> Users { get; set; }

        public DbSet<Cup> Cups { get; set; }
        
        public ApplicationDbContext() : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO сделать получение строки подключения из файла
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=LatteLove;Username=postgres;Password=Nassa123");
        }


    }
}
