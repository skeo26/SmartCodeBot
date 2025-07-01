using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace SmartCodeBot.Handlers
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly IBotCommandHandler _commandHandler;

        public UpdateHandler(IBotCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message?.Text != null && update.Type == UpdateType.Message)
            {
                var chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;

                await _commandHandler.HandleCommandAsync(botClient, chatId, messageText, cancellationToken);
            }
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Ошибка: {exception.Message}");
            return Task.CompletedTask;
        }
    }

}
