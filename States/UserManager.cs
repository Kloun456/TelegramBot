using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototype;
using Telegram.Bot.Types;

namespace CoffeBot.States
{
    internal class UserManager : IUsersManager
    {
        public UserDataChanged? SignedUpdateHandled => null;

        public List<BotUser> Users { get; } = [];

        public async Task<IBotUser?> GetUserByIdAsync(long telegramId) =>
            await Task.FromResult(Users.Find(x => x.TelegramId == telegramId));

        public async Task<bool> IsUserRegisteredAsync(long telegramId) =>
            await this.GetUserByIdAsync(telegramId) is not null;
        
        public async Task<IBotUser?> RegisterNewUserAsync(ICastedUpdate update)
        {
            User user = ChatScanner.GetSender(update.OriginalSource, this)!;
            BotUser newUser = new (user.Id);
            Users.Add(newUser);
            return await Task.FromResult(newUser);
        }
    }
}
