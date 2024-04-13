using CoffeBot.Models;
using CoffeBot.Repositories;
using Telegram.Bot.Types;

namespace CoffeBot.Service
{
    public class BotService : IBotService
    {
        private readonly IRepositoryUser _repositoryUser;
        private readonly IRepositoryCup _repositoryCup;

        public BotService(IRepositoryUser repositoryUser, IRepositoryCup repositoryCup)
        {
            _repositoryUser = repositoryUser;
            _repositoryCup = repositoryCup;
        }

        public bool UserIsAdmin(User user)
        {
            var checkUser = _repositoryUser.GetUser(user.Id);
            if (checkUser == null)
            {
                throw new Exception($"Пльзователя с id:[{user.Id}] в базе данных нет");
            }
            return checkUser.IsAdmin;
        }

        public bool UserIsExistsInDb(User user)
        {
            var checkUser = _repositoryUser.GetUser(user.Id );
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

        public void CheckCupForUser(User user)
        {
            var userCup = _repositoryCup.GetCupForUser(user);
            if (userCup != null) 
            {
                Console.WriteLine($"User with id[{user.Id}] have {userCup.CountCups} cups");
            }
            else
            {
                Console.WriteLine($"User with id[{user.Id}] does not exists in BD");
            }
        }

        public Cup? GetCupForUser(User user)
        {
            var userCup = _repositoryCup.GetCupForUser(user);
            return userCup;
        }

        public User? GetUser(User user) 
        {

            return null;
        }

        public bool IsUserHaveCups(User user)
        {
            var userCup = _repositoryCup.GetCupForUser(user);
            if (userCup != null)
            {
                return true;
            }
            return false;
        }

        public void CreateCupForUser(User user)
        {
            _repositoryCup.CreateCupForUser(user);
        }

        public void AddCupForUser(User user)
        {
            _repositoryCup.AddCupForUser(user);
        }

    }
}
