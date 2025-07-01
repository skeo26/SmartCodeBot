using SmartCodeBot.Commands.MainMenuCommands;
using SmartCodeBot.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SmartCodeBot.CommandProccesors
{
    public class FeedbackCommandProcessor : ICommandProcessor
    {
        private readonly IDBService _dbService;

        public FeedbackCommandProcessor(IDBService dbService)
        {
            _dbService = dbService;
        }

        public async Task<bool> TryProcessAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            if (FeedbackCommand.IsWaiting(chatId))
            {
                var command = new FeedbackCommand(_dbService);
                await command.ExecuteAsync(botClient, chatId, messageText, cancellationToken);
                return true;
            }
            return false;
        }
    }
}
