using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using CoffeBot.Service;
using Telegram.Bot.Polling;
using Telegram.Bot.Exceptions;
using QRCoder;
using static QRCoder.PayloadGenerator;

namespace CoffeBot.Handlers
{
    public class UpdateHandler : IUpdateHandler
    {
        private static IBotService _botService;
        public UpdateHandler(IBotService botService) 
        {
            _botService = botService;
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case (UpdateType.Message):
                        {
                            var message = update.Message;
                            if (message == null)
                            {
                                throw new ArgumentException("Не получилось получить сообщение");
                            }
                            var user = message.From;
                            if (user == null)
                            {
                                throw new ArgumentException("Не получилось получить отправителя сообщения");
                            }
                            var chat = message.Chat;
                            if (chat == null)
                            {
                                throw new ArgumentException("Не получилось получить чат");
                            }
                            Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");
                     
                            if (message.Text == "/start")
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

                                var menu = new MenuButtonDefault();
                                //await botClient.SetChatMenuButtonAsync(chat?.Id, menu, cancellationToken: cancellationToken);
                                await botClient.SendTextMessageAsync(
                                    chatId: chat.Id, "Рад приветствовать в боте для лучшей кофейни!",
                                    replyMarkup: inlineKeyboard, cancellationToken: cancellationToken);
                            }
                            else if (message.Text.Contains("/start") )
                            {
                                if (_botService.UserIsAdmin(user))
                                {
                                    var clientId = GetUserIdFromMessage(message.Text);
                                    user.Id = clientId;
                                    _botService.AddCupForUser(user);
                                    await botClient.SendTextMessageAsync(
                                    chatId: chat.Id,
                                    "Чашка успешно добавлена!");
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(
                                    chatId: chat.Id,
                                    "Вы не являетесь администратором!");
                                }
                                
                            }

                            return;
                        }

                    case (UpdateType.CallbackQuery):
                        {
                            var callbackQuery = update.CallbackQuery;

                            if (callbackQuery != null)
                            {
                                var user = callbackQuery.From;

                                Console.WriteLine($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");

                                var message = callbackQuery.Message;
                                var chat = message.Chat;

                                switch (callbackQuery.Data)
                                {
                                    case "addCup":
                                        {
                                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                            
                                            
                                            if (!_botService.UserIsExistsInDb(user))
                                            {
                                                _botService.CreateUser(user);
                                            }
                                            
                                            var qrGenerator = new QRCodeGenerator();
                                            
                                            Payload payload = new Url($"https://t.me/latteLoveBot?start={user.Id}");

                                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload);
                                            var qrCode = new BitmapByteQRCode(qrCodeData);
                                            var arCodeAsBitmap = qrCode.GetGraphic(20);


                                            using (var ms = new MemoryStream(arCodeAsBitmap))
                                            {
                                                await botClient.SendPhotoAsync(message.Chat.Id, new InputFileStream(ms, "image.png"));
                                            }

                                            return;
                                        }
                                    case "checkCups":
                                        {
                                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                            var userCup = _botService.GetCupForUser(user);
                                            if (userCup != null) 
                                            {
                                                await botClient.SendTextMessageAsync(
                                                    chatId: chat.Id,
                                                    $"Количество ваших чашек - {userCup.CountCups}");
                                            }
                                            else
                                            {
                                                await botClient.SendTextMessageAsync(
                                                    chatId: chat.Id,
                                                    "У вас нет ни одной чашки!");
                                            }
                                            return;
                                        }
                                }
                                return;
                            }
                            return;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private long GetUserIdFromMessage(string message)
        {
            var idString = message.Split(' ').Last();
            var longId = long.Parse(idString);
            return longId;
        }
    }
}
