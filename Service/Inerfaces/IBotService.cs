using CoffeBot.Models;
using Telegram.Bot.Types;

namespace CoffeBot.Service.Inerfaces
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
}
