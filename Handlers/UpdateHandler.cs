using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using CoffeBot.Service;
using Telegram.Bot.Polling;
using Telegram.Bot.Exceptions;

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
                                            InlineKeyboardButton.WithCallbackData("Добавить напиток", "AddCupButton"),
                                            InlineKeyboardButton.WithCallbackData("Проверить количество чашек", "CheckCups")
                                        }
                                    });

                                var menu = new MenuButtonDefault();
                                //await botClient.SetChatMenuButtonAsync(chat?.Id, menu, cancellationToken: cancellationToken);
                                await botClient.SendTextMessageAsync(
                                    chatId: chat.Id, "Рад приветствовать в боте для лучшей кофейни!",
                                    replyMarkup: inlineKeyboard, cancellationToken: cancellationToken);
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
                                            if (!_botService.UserIsExistsInDb(user))
                                            {
                                                _botService.CreateUser(user);
                                            }
                                            if (_botService.UserIsAdmin(user))
                                            {
                                                if (!_botService.IsUserHaveCups(user))
                                                {
                                                    _botService.CreateCupForUser(user);
                                                }
                                                else
                                                {
                                                    _botService.AddCupForUser(user);
                                                }
                                            }
                                            else
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, "Вы не являетесь администратором!");
                                            }
                                            return;
                                        }
                                    case "CheckCups":
                                        {
                                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                            _botService.CheckCupsForUser(user);
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

       
    }
}
