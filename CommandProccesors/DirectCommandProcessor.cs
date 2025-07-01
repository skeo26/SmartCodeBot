using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.DataAccess;
using SmartCodeBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SmartCodeBot.CommandProccesors
{
    public class DirectCommandProcessor : ICommandProcessor
    {
        private readonly Dictionary<string, IBotCommand> _commands;
        private readonly IUserSessionService _sessionService;
        private readonly IDBService _dbService;

        public DirectCommandProcessor(IEnumerable<IBotCommand> commands, IUserSessionService sessionService, IDBService dbService)
        {
            _commands = commands.ToDictionary(c => c.CommandText);
            _sessionService = sessionService;
            _dbService = dbService;
        }

        public async Task<bool> TryProcessAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            if (_commands.TryGetValue(messageText, out var command))
            {
                await command.ExecuteAsync(botClient, chatId, messageText, cancellationToken);
                await _dbService.SaveUserSessionAsync(chatId, _sessionService.GetSession(chatId));
                return true;
            }
            return false;
        }
    }
}
