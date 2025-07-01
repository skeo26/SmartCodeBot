using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.DataAccess;
using SmartCodeBot.KeyboardsMarkup;
using SmartCodeBot.Models;
using SmartCodeBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SmartCodeBot.Commands.OtherCommands
{
    public class BackToMainMenuCommand : IBotCommand
    {
        private readonly IUserSessionService _userSessionService;
        private readonly IDBService _dbService;

        public BackToMainMenuCommand(IUserSessionService userSessionService, IDBService dbService)
        {
            _userSessionService = userSessionService;
            _dbService = dbService;
        }
        

        public string CommandText => "Вернуться на главную";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            _userSessionService.SetUserLanguage(chatId, null);
            _userSessionService.SetUserCategory(chatId, null);
            _userSessionService.SetUserSubtopic(chatId, null);
            _userSessionService.SetUserTest(chatId, null);
            _userSessionService.SetUserTakingTest(chatId, false);
            _userSessionService.SetCurrentTopic(chatId, null);
            await _dbService.SaveUserSessionAsync(chatId, _userSessionService.GetSession(chatId));

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Вы вернулись в главное меню. Выберите действие:",
                replyMarkup: MainMenuKeyboard.GetMainKeyboard(),
                cancellationToken: cancellationToken);
        }
    }
}
