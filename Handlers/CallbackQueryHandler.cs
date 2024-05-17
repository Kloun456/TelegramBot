using CoffeBot.Service;
using QRCoder;
using Telegram.Bot;
using Telegram.Bot.Types;
using static QRCoder.PayloadGenerator;

namespace CoffeBot.Handlers
{
    public interface ICallbackQueryHandler
    {
        Task HandleCallbackQuery(Update update, ITelegramBotClient botClient);
    }

    internal class CallbackQueryHandler : ICallbackQueryHandler
    {
        private IBotService _botService;

        public CallbackQueryHandler(IBotService botService)
        {
            _botService = botService;
        }

        public async Task HandleCallbackQuery(Update update, ITelegramBotClient botClient)
        {
            var callbackQuery = update.CallbackQuery;

            if (callbackQuery != null)
            {
                User user = callbackQuery.From ?? throw new ArgumentNullException("Не удалось получить пользователя");
                Message message = callbackQuery.Message ?? throw new ArgumentNullException("Не удалось получить сообщение");
                Chat chat = message.Chat ?? throw new ArgumentNullException("Не удалось получить чат");

                Console.WriteLine($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");


                switch (callbackQuery.Data)
                {
                    case "addCup":
                        {
                            await botClient.AnswerCallbackQueryAsync(
                                callbackQuery.Id,
                                cancellationToken: default);

                            if (!_botService.UserIsExistsInDb(user.Id))
                            {
                                _botService.CreateUser(user);
                            }

                            var qrGenerator = new QRCodeGenerator();

                            Payload payload = new Url($"https://t.me/latteLoveBot?start={user.Id}");

                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload);
                            var qrCode = new BitmapByteQRCode(qrCodeData);
                            var arCodeAsBitmap = qrCode.GetGraphic(10);

                            using var ms = new MemoryStream(arCodeAsBitmap);

                            await botClient.SendPhotoAsync(message.Chat.Id,
                                new InputFileStream(ms, "image.png"),
                                cancellationToken: default);

                            return;
                        }
                    case "checkCups":
                        {
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id,
                                cancellationToken: default);
                            var userCup = _botService.GetCupForUser(user.Id);
                            if (userCup != null)
                            {
                                await botClient.SendTextMessageAsync(chatId: chat.Id,
                                    $"Количество ваших чашек - {userCup.CountCups}",
                                    cancellationToken: default);
                            }
                            else
                            {
                                await botClient.SendTextMessageAsync(chatId: chat.Id,
                                    "У вас нет ни одной чашки!",
                                    cancellationToken: default);
                            }
                            return;
                        }
                }
                return;
            }

        }
    }
}
