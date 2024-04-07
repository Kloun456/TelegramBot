/*using CoffeBot.DataBase;
using CoffeBot.Models;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using User = CoffeBot.Models.User;

// https://t.me/lattelovecoffee

namespace TelegramBotExperiments
{
    class Program
    {

        private static ITelegramBotClient? _botClient;
        private static ReceiverOptions? _receiverOptions;
        public static async Task Main()
        {

            _botClient = new TelegramBotClient("6435771255:AAHigtm3iMfduaIBEJMRkZqlwzlL5Ndcp8o"); // заменить получение токена из
            //appsetings.json

            _receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery],
                ThrowPendingUpdates = true
            };

            using var cancellationToken = new CancellationTokenSource();

            _botClient?.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cancellationToken.Token);

            var botUser = await _botClient.GetMeAsync();
            Console.WriteLine($"{botUser.FirstName} запущен!");

            await Task.Delay(-1);

        }

        private static async Task UpdateHandler(ITelegramBotClient botClient,
          Update update, CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case (UpdateType.Message):
                        {
                            var message = update.Message;
                            var user = message.From;
                            var chat = message.Chat;
                            Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");
                            if (message.Text == "/start")
                            {
                                var inlineKeyboard = new InlineKeyboardMarkup(
                                    new List<InlineKeyboardButton[]>()
                                    {
                                    new InlineKeyboardButton[]{
                                        InlineKeyboardButton.WithCallbackData("Добавить напиток", "AddCupButton"),
                                        InlineKeyboardButton.WithCallbackData("Проверить количество чашек", "CheckCups")
                                    }
                                    });

                                var menu = new MenuButtonDefault();
                                //await botClient.SetChatMenuButtonAsync(chat?.Id, menu, cancellationToken: cancellationToken);
                                await botClient.SendTextMessageAsync(
                                    chatId: chat?.Id, "Рад приветствовать в боте для лучшей кофейни!",
                                    replyMarkup: inlineKeyboard, cancellationToken: cancellationToken); // Все клавиатуры передаются в параметр replyMarkup

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
                                    case "AddCupButton":
                                        {
                                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                                            using (ApplicationDbContext db = new())
                                            {
                                                var checkUser = (from userDb in db.Users
                                                                 where userDb.Id == user.Id
                                                                 select userDb).ToList().FirstOrDefault();
                                                if (checkUser == null)
                                                {
                                                    db.Users.Add(new User { Id = user.Id, IsAdmin = false, Name = user.FirstName });
                                                    db.SaveChanges();
                                                }
                                                else if (checkUser.IsAdmin == true)
                                                {

                                                    var userCups = (from cup in db.Cups
                                                                    where cup.UserId == user.Id
                                                                    select cup).ToList().FirstOrDefault();
                                                    if (userCups == null)
                                                    {
                                                        db.Cups.Add(new Cup { CountCups = 1, Id = Guid.NewGuid(), UserId = user.Id });
                                                        db.SaveChanges();
                                                    }
                                                    else
                                                    {
                                                        userCups.CountCups += 1;
                                                        db.SaveChanges();
                                                    }
                                                }
                                                else
                                                {
                                                    await botClient.SendTextMessageAsync(chat.Id, "Вы не являетесь администратором!");
                                                }

                                            }
                                            return;
                                        }
                                    case "CheckCups":
                                        {
                                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                            using (ApplicationDbContext db = new())
                                            {
                                                var cups = db.Cups.ToList();
                                                Console.WriteLine("Users list:");
                                                foreach (Cup cup in cups)
                                                {
                                                    Console.WriteLine($"{cup.Id}.{cup.UserId}.{cup.CountCups}");
                                                }
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

        private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}


*/
/*
 await botClient.SendTextMessageAsync(chat.Id, message.Text, replyToMessageId: message.MessageId,
                            replyMarkup: inlineKeyboard);
                            await botClient.SendTextMessageAsync(chat.Id, "https://t.me/lattelovecoffee");
                            var qrGenerator = new QRCodeGenerator();
                            Payload payload = new Url("https://t.me/lattelovecoffee");

                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload);
                            var qrCode = new BitmapByteQRCode(qrCodeData);
                            var arCodeAsBitmap = qrCode.GetGraphic(20);


                            using (var ms = new MemoryStream(arCodeAsBitmap))
                            {
                                await botClient.SendPhotoAsync(message.Chat.Id, new InputFileStream(ms, "image.png"));
                            }

                            Console.WriteLine($"{user?.FirstName} ({user?.Id}) написал сообщение: {message?.Text}");
*/