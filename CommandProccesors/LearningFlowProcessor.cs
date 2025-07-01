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
    public class LearningFlowProcessor : ICommandProcessor
    {
        private readonly Dictionary<string, IBotCommand> _commands;
        private readonly IUserSessionService _sessionService;
        private readonly IDBService _dbService;

        public LearningFlowProcessor(IEnumerable<IBotCommand> commands, IUserSessionService sessionService, IDBService dbService)
        {
            _commands = commands.ToDictionary(c => c.CommandText);
            _sessionService = sessionService;
            _dbService = dbService;
        }

        public async Task<bool> TryProcessAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            if (_sessionService.IsUserTakingTest(chatId))
            {
                if (string.IsNullOrEmpty(_sessionService.GetUserCategory(chatId)))
                {
                    _sessionService.SetUserCategory(chatId, messageText);
                    await _commands["Выбрать подтему теста"].ExecuteAsync(botClient, chatId, messageText, cancellationToken);
                }
                else
                {
                    await _commands["Начать тест"].ExecuteAsync(botClient, chatId, messageText, cancellationToken);
                }

                await _dbService.SaveUserSessionAsync(chatId, _sessionService.GetSession(chatId));
                return true;
            }

            if (!string.IsNullOrEmpty(_sessionService.GetUserCategory(chatId)))
            {
                await _commands["Выбрать материал"].ExecuteAsync(botClient, chatId, messageText, cancellationToken);
                await _dbService.SaveUserSessionAsync(chatId, _sessionService.GetSession(chatId));
                return true;
            }

            await _commands["Выбрать подтему"].ExecuteAsync(botClient, chatId, messageText, cancellationToken);
            await _dbService.SaveUserSessionAsync(chatId, _sessionService.GetSession(chatId));
            return true;
        }
    }
}
