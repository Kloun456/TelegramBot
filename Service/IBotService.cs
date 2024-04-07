using Telegram.Bot.Types;

namespace CoffeBot.Service
{
    public interface IBotService
    {
        void AddCupForUser(User user);

        bool UserIsAdmin(User user);

        bool UserIsExistsInDb(User user);

        void CreateUser(User user);

        void CheckCupsForUser(User user);

        bool IsUserHaveCups(User user);

        void CreateCupForUser(User user);
    }
}
