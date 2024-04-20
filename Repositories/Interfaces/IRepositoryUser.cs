using CoffeBot.Models;
using Telegram.Bot.Types;

namespace CoffeBot.Repositories.Interfaces
{
    public interface IRepositoryUser
    {
        UserDb? GetUser(long id);
        void CreateUser(User user);
    }
}
