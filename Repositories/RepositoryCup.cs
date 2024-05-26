using CoffeBot.DataBase;
using CoffeBot.Helpers;
using CoffeBot.Models;

namespace CoffeBot.Repositories
{
    public interface IRepositoryCup
    {
        Cup? GetCupForUser(long userId);

        void CreateCupForUser(long userId);

        void AddCupForUser(long userId);

        void ResetCupForUser(long userId);
    }

    public class RepositoryCup : IRepositoryCup
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositoryCup(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Cup? GetCupForUser(long userId)
        {
            var cupUser = (from cup in _dbContext.Cups
                           where cup.UserDb.Id == Conversions.LongToGuid(userId)
                           select cup).FirstOrDefault();
            return cupUser;
        }

        public void AddCupForUser(long userId)
        {
            var userCup = (from cup in _dbContext.Cups
                           where cup.UserDb.Id == Conversions.LongToGuid(userId)
                           select cup).ToList().FirstOrDefault();
            if (userCup == null)
            {
                throw new Exception($"Не получилось добавить кружку для пользователя с id[{userId}]");
            }
            userCup.CountCups += 1;
            _dbContext.SaveChanges();
        }

        public void CreateCupForUser(long userId)
        {
            _dbContext.Cups.Add(new Cup
            {
                CountCups = 1,
                Id = Conversions.LongToGuid(userId)
            });
            _dbContext.SaveChanges();
        }

        public void ResetCupForUser(long userId)
        {
            var userCup = (from cup in _dbContext.Cups
                           where cup.UserDb.Id == Conversions.LongToGuid(userId)
                           select cup).ToList().FirstOrDefault();
            if (userCup == null)
            {
                throw new Exception($"Не получилось добавить кружку для пользователя с id[{userId}]");
            }
            userCup.CountCups = 0;
            _dbContext.SaveChanges();
        }

    }
}
