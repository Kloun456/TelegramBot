using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Exceptions;

namespace CoffeBot.Handlers
{
    public class UpdateHandler : IUpdateHandler
    {
        private static IMessageHandler _messageHandler;
        private static ICallbackQueryHandler _callbackQueryHandler;

        public UpdateHandler(IMessageHandler messageHandler, ICallbackQueryHandler callbackQueryHandler) 
        {
            _messageHandler = messageHandler;
            _callbackQueryHandler = callbackQueryHandler;
        }

       
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case (UpdateType.Message):
                    {
                        await _messageHandler.HandleMessage(update, botClient);
                        return;
                    }
                    case (UpdateType.CallbackQuery):
                    {
                        await _callbackQueryHandler.HandleCallbackQuery(update, botClient);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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


       

       
    }
}
