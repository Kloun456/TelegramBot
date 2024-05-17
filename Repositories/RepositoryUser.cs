using CoffeBot.DataBase;
using CoffeBot.Helpers;
using CoffeBot.Models;
using Telegram.Bot.Types;

namespace CoffeBot.Repositories
{
    public interface IRepositoryUser
    {
        UserDb? GetUser(long id);
        void CreateUser(User user);
    }

    public class RepositoryUser : IRepositoryUser
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositoryUser(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserDb? GetUser(long id)
        {
            var user = (
                from userDb in _dbContext.Users
                where userDb.Id == Conversions.LongToGuid(id)
                select userDb)
                .ToList()
                .FirstOrDefault();
            return user;
        }

        public void CreateUser(User user)
        {
            _dbContext.Users.Add(new UserDb
            {
                Id = Conversions.LongToGuid(user.Id),
                IsAdmin = false,
                Name = user.FirstName
            });
            _dbContext.SaveChanges();
        }
    }
}
