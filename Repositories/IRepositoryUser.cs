using CoffeBot.Models;
using Telegram.Bot.Types;

namespace CoffeBot.Repositories
{
    public interface IRepositoryUser
    {
        UserDb? GetUser(long id);
        void CreateUser(User user);
    }
}
