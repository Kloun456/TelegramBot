using CoffeBot.DataBase;
using CoffeBot.Models;
using Telegram.Bot.Types;

namespace CoffeBot.Repositories
{
    public class RepositoryUser : IRepositoryUser
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositoryUser()
        {
            _dbContext = new ApplicationDbContext();
        }

        public UserDb? GetUser(long id)
        {
            var user = (
                from userDb in _dbContext.Users
                where userDb.Id == id
                select userDb)
                .ToList()
                .FirstOrDefault();
            return user;
        }

        public void CreateUser(User user)
        {
            _dbContext.Users.Add(new UserDb
            {
                Id = user.Id,
                IsAdmin = false,
                Name = user.FirstName
            });
            _dbContext.SaveChanges();
        }
    }
}
