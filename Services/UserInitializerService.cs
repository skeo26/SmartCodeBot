using SmartCodeBot.DataAccess;
using SmartCodeBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SmartCodeBot.Services
{
    public class UserInitializerService : IUserInitializerService
    {
        private readonly IDBService _databaseService;
        private readonly IUserSessionService _userSessionService;

        public UserInitializerService(IDBService databaseService, IUserSessionService userSessionService)
        {
            _databaseService = databaseService;
            _userSessionService = userSessionService;
        }

        public async Task InitializeUserSessionAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            var user = await botClient.GetChatAsync(chatId, cancellationToken);
            var userId = await _databaseService.GetOrCreateUserAsync(chatId, user.Username);

            var sessionData = await _databaseService.GetUserSessionAsync(userId);
            if (sessionData != null)
            {
                if (!string.IsNullOrEmpty(sessionData.Language))
                    _userSessionService.SetUserLanguage(chatId, sessionData.Language);

                if (!string.IsNullOrEmpty(sessionData.Category))
                    _userSessionService.SetUserCategory(chatId, sessionData.Category);

                if (!string.IsNullOrEmpty(sessionData.Topic))
                    _userSessionService.SetCurrentTopic(chatId, sessionData.Topic);

                _userSessionService.SetUserTakingTest(chatId, sessionData.IsTakingTest);

                // Создаём первоначальное состояние меню
                var initialMenuState = new MenuState
                {
                    Language = sessionData.Language,
                    Category = sessionData.Category,
                    CurrentTopic = sessionData.Topic,
                    IsTakingTest = sessionData.IsTakingTest,
                    Test = sessionData.Test
                };

                _userSessionService.PushMenuState(chatId, initialMenuState);
            }
        }
    }
}

