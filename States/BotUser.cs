using SKitLs.Bots.Telegram.Stateful.Model;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace CoffeBot.States
{
    internal class BotUser : IStatefulUser
    {
        public long TelegramId { get; set; }
        public IUserState State { get; set; }

        public BotUser(long telegramId)
        {
            this.TelegramId = telegramId;
            this.State = new DefaultUserState();
        }

        public void ResetState()
        {
            throw new NotImplementedException();
        }
    }
}
