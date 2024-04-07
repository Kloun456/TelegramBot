using CoffeBot.Models;
using Telegram.Bot.Types;

namespace CoffeBot.Repositories
{
    public interface IRepositoryCup
    {
        Cup? GetCupForUser(User user);

        void CreateCupForUser(User user);

        void AddCupForUser(User user);
    }
}
