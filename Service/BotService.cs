using CoffeBot.Models;
using CoffeBot.Repositories;
using Telegram.Bot.Types;

namespace CoffeBot.Service
{
    public interface IBotService
    {
        void CreateUser(User user);

        void CreateCupForUser(long userId);

        void AddCupForUser(long userId);

        bool UserIsAdmin(long userId);

        bool UserIsExistsInDb(long userId);

        Cup? GetCupForUser(long userId);

        void ResetCupsForUser(long userId);
    }

    public class BotService : IBotService
    {
        private readonly IRepositoryUser _repositoryUser;
        private readonly IRepositoryCup _repositoryCup;

        public BotService(IRepositoryUser repositoryUser, IRepositoryCup repositoryCup)
        {
            _repositoryUser = repositoryUser;
            _repositoryCup = repositoryCup;
        }

        public bool UserIsAdmin(long userId)
        {
            var checkUser = _repositoryUser.GetUser(userId) ?? 
                throw new Exception($"Пльзователя с id:[{userId}] в базе данных нет");
            return checkUser.IsAdmin;
        }

        public bool UserIsExistsInDb(long userId)
        {
            var checkUser = _repositoryUser.GetUser(userId);
            if (checkUser == null)
            {
                return false;
            }
            return true;
        }

        public void CreateUser(User user)
        {
            _repositoryUser.CreateUser(user);
        }

        public Cup? GetCupForUser(long userId)
        {
            var userCup = _repositoryCup.GetCupForUser(userId);
            return userCup;
        }

        public void CreateCupForUser(long userId)
        {
            _repositoryCup.CreateCupForUser(userId);
        }

        public void AddCupForUser(long userId)
        {
            _repositoryCup.AddCupForUser(userId);
        }

        public void ResetCupsForUser(long userId)
        {
            _repositoryCup.ResetCupForUser(userId);
        }

    }
}
