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
    public class GroupCommandProcessor : ICommandProcessor
    {
        private readonly IEnumerable<ICommandGroup> _commandGroups;
        private readonly IUserSessionService _sessionService;
        private readonly IDBService _dbService;

        public GroupCommandProcessor(IEnumerable<ICommandGroup> commandGroups, IUserSessionService sessionService, IDBService dbService)
        {
            _commandGroups = commandGroups;
            _sessionService = sessionService;
            _dbService = dbService;
        }

        public async Task<bool> TryProcessAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            foreach (var group in _commandGroups)
            {
                if (group.Commands.TryGetValue(messageText, out var command))
                {
                    await command.ExecuteAsync(botClient, chatId, messageText, cancellationToken);
                    await _dbService.SaveUserSessionAsync(chatId, _sessionService.GetSession(chatId));
                    return true;
                }
            }
            return false;
        }
    }
}
