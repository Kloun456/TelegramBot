using CoffeBot.DataBase;
using CoffeBot.Helpers;
using CoffeBot.Models;
using Telegram.Bot.Types;

namespace CoffeBot.Repositories
{
    public class RepositoryCup : IRepositoryCup
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositoryCup() 
        {
            _dbContext = new ApplicationDbContext();
        }

        public Cup? GetCupForUser(User user)
        {
            var cupUser = (from cup in _dbContext.Cups
                       where cup.UserDb.Id == Conversions.LongToGuid(user.Id)
                       select cup).FirstOrDefault();
            return cupUser;
        }

        public void AddCupForUser(User user)
        {
            var userCup = (from cup in _dbContext.Cups
                            where cup.UserDb.Id == Conversions.LongToGuid(user.Id)
                            select cup).ToList().FirstOrDefault();
            if (userCup == null) 
            {
                throw new Exception($"Не получилось добавить кружку для пользователя с id[{user.Id}]");
            }
            userCup.CountCups += 1;
            _dbContext.SaveChanges();
        }

        public void CreateCupForUser(User user)
        {
            
            _dbContext.Cups.Add(new Cup 
            { 
                CountCups = 1, 
                Id = Conversions.LongToGuid(user.Id) 
            });
            _dbContext.SaveChanges();
        }
    }
}
