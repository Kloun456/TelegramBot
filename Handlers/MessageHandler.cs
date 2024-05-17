using CoffeBot.Helpers;
using CoffeBot.Service;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoffeBot.Handlers
{
    public interface IMessageHandler
    {
        Task HandleMessage(Update update, ITelegramBotClient botClient);
    }

    internal class MessageHandler : IMessageHandler
    {
        private IBotService _botService;
        public MessageHandler(IBotService botService) 
        {
            _botService = botService;
        }

        public async Task HandleMessage(Update update, ITelegramBotClient botClient)
        {
            var message = update.Message ?? throw new ArgumentException("Не удалось получить сообщение");
            var user = message.From ?? throw new ArgumentException("Не удалось получить отправителя сообщения");
            var chat = message.Chat ?? throw new ArgumentException("Не удалось получить чат");
            var textFromMessage = message.Text ?? throw new ArgumentNullException("Не удалось получить текст сообщения");

            Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

            if (textFromMessage == "/start")
            {
                var inlineKeyboard = new InlineKeyboardMarkup(
                    new List<InlineKeyboardButton[]>()
                    {
                        new InlineKeyboardButton[]
                        {
                            InlineKeyboardButton.WithCallbackData("Добавить напиток", "addCup"),
                            InlineKeyboardButton.WithCallbackData("Проверить количество чашек", "checkCups")
                        }
                    });

                await botClient.SendTextMessageAsync(
                    chatId: chat.Id,
                    "Рад приветствовать в боте для лучшей кофейни!",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: default);
            }
            else if (textFromMessage.Contains("/start"))
            {
                var userId = user.Id;
                if (_botService.UserIsAdmin(userId))
                {
                    var clientId = GetUserIdFromMessage(textFromMessage);

                    _botService.AddCupForUser(userId);

                    var userCup = _botService.GetCupForUser(userId) ?? throw new ArgumentNullException("");
                    if (userCup.CountCups == Promotions.CountCupsForFree)
                    {
                        _botService.ResetCupsForUser(userId);
                        await botClient.SendTextMessageAsync(chatId: clientId,
                            "Поздравляем вы получаете бесплатный напиток!",
                            cancellationToken: default);

                        await botClient.SendTextMessageAsync(chatId: chat.Id,
                            "У клиента с ником TEST бесплатный напиток!",
                            cancellationToken: default);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId: chat.Id,
                        "Чашка успешно добавлена!",
                        cancellationToken: default);

                        await botClient.SendTextMessageAsync(chatId: clientId,
                        "Чашка успешно добавлена!",
                        cancellationToken: default);

                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId: chat.Id,
                        "Вы не являетесь администратором!",
                        cancellationToken: default);
                }

            }
            else if (textFromMessage == "/addAdmin")
            {

            }
        }

        private static long GetUserIdFromMessage(string message)
        {
            var idString = message.Split(' ').Last();
            var longId = long.Parse(idString);
            return longId;
        }
    }
}
