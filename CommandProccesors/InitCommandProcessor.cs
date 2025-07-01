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
    public class InitCommandProcessor : ICommandProcessor
    {
        private readonly IUserSessionService _sessionService;
        private readonly IUserInitializerService _initializerService;
        private readonly IDBService _dbService;

        public InitCommandProcessor(IUserSessionService sessionService, IUserInitializerService initializerService, IDBService dbService)
        {
            _sessionService = sessionService;
            _initializerService = initializerService;
            _dbService = dbService;
        }

        public async Task<bool> TryProcessAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            if (!_sessionService.IsUserInitialized(chatId))
            {
                await _initializerService.InitializeUserSessionAsync(botClient, chatId, cancellationToken);
                _sessionService.SetUserInitialized(chatId);
                await _dbService.SaveUserSessionAsync(chatId, _sessionService.GetSession(chatId));
                return true;
            }
            return false;
        }
    }
}
