using CoffeBot.Models;

namespace CoffeBot.Repositories.Interfaces
{
    public interface IRepositoryCup
    {
        Cup? GetCupForUser(long userId);

        void CreateCupForUser(long userId);

        void AddCupForUser(long userId);

        void ResetCupForUser(long userId);
    }
}
