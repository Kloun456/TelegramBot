using CoffeBot.States;
using SKitLs.Bots.Telegram.AdvancedMessages.AdvancedDelivery;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.Management.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.PageNavs.Prototype;


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
               .UseUsersManager(new UserManager())
               .UseMessageHandler(privateMessages);

            await BotBuilder.NewBuilder("6435771255:AAHigtm3iMfduaIBEJMRkZqlwzlL5Ndcp8o") // токен бота
               .EnablePrivates(privates)
               .AddService<IArgsSerializeService>(new DefaultArgsSerializeService())
               .AddService<IMenuManager>(GetMenuManager())
               .CustomDelivery(new AdvancedDeliverySystem())
               .Build()
               .Listen();
        }

        private static DefaultCommand StartCommand => new("start", Do_StartAsync);
        private static async Task Do_StartAsync(SignedMessageTextUpdate update)
        {
            IMenuManager mm = update.Owner.ResolveService<IMenuManager>();

            IBotPage page = mm.GetDefined("main");

            await mm.PushPageAsync(page, update);
        }

        private static IMenuManager GetMenuManager()
        {
            DefaultMenuManager menuManager = new ();

            OutputMessageText mainBoody = new ("Что выберешь?/n");
            PageNavMenu mainMenu = new ();
            WidgetPage mainPage = new ("main", "Главная", mainBoody, mainMenu);

            DynamicMessage savedBody = new(message => { return new OutputMessageText("Избранное ...");
            });

            WidgetPage savePage = new("save", "Избранное", savedBody);

            mainMenu.PathTo(savePage);
            mainMenu.AddAction(StartSearching);

            menuManager.Define(mainPage);
            menuManager.Define(savePage);

            return menuManager;
        }

        private static DefaultCallback StartSearching => new("startSearch", "Найти", Do_SearchAsync);
        private static async Task Do_SearchAsync(SignedCallbackUpdate update) { }

    }
}