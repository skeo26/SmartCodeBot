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
    public class AssistantCommandProcessor : ICommandProcessor
    {
        private readonly IUserQuestionBuffer _questionBuffer;
        private readonly IBotCommand _assistantCommand;
        private readonly IUserSessionService _sessionService;
        private readonly IDBService _dbService;

        public AssistantCommandProcessor(
            IUserQuestionBuffer questionBuffer,
            IEnumerable<IBotCommand> commands,
            IUserSessionService sessionService,
            IDBService dbService)
        {
            _questionBuffer = questionBuffer;
            _assistantCommand = commands.First(c => c.CommandText == "SmartCode помощник");
            _sessionService = sessionService;
            _dbService = dbService;
        }

        public async Task<bool> TryProcessAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            if (_questionBuffer.HasPendingQuestion(chatId))
            {
                await _assistantCommand.ExecuteAsync(botClient, chatId, messageText, cancellationToken);

                await _dbService.SaveUserSessionAsync(chatId, _sessionService.GetSession(chatId));

                return true;
            }
            return false;
        }
    }
}
