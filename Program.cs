using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.Management.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace TelegramBotExperiments
{

    class Program
    {
        static async Task Main(string[] args)
        {
            var privateMessages = new DefaultSignedMessageUpdateHandler();
            var privateTexts = new DefaultSignedMessageTextUpdateHandler
            {
                CommandsManager = new DefaultActionManager<SignedMessageTextUpdate>()
            };
            privateTexts.CommandsManager.AddSafely(StartCommand);
            privateMessages.TextMessageUpdateHandler = privateTexts;

            ChatDesigner privates = ChatDesigner.NewDesigner()
               .UseMessageHandler(privateMessages);

            await BotBuilder.NewBuilder("6435771255:AAHigtm3iMfduaIBEJMRkZqlwzlL5Ndcp8o")
               .EnablePrivates(privates)
               .Build()
               .Listen();
        }

        private static DefaultCommand StartCommand => new("start", Do_StartAsync);
        private static async Task Do_StartAsync(SignedMessageTextUpdate update)
        {
            await update.Owner.DeliveryService.AnswerSenderAsync("Hello, world!", update);
        }
    }
}