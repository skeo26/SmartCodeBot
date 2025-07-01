using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.DataAccess;
using SmartCodeBot.KeyboardsMarkup;
using SmartCodeBot.Models;
using SmartCodeBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace SmartCodeBot.Commands.LaguageMenuCommands
{
    public class LanguageChoiceCommand : IBotCommand
    {
        private readonly IUserSessionService _userSessionService;
        private readonly IDBService _dbService;

        public LanguageChoiceCommand(IUserSessionService userSessionService, IDBService dbService)
        {
            _userSessionService = userSessionService;
            _dbService = dbService;
        }

        public string CommandText => "Выбор языка";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            _userSessionService.PushMenuState(chatId, new MenuState
            {
                Text = CommandText,
                Keyboard = LanguageSelectionKeyboard.GetLanguageSelectionKeyboard()
            });

            _userSessionService.SetUserLanguage(chatId, messageText);
            _userSessionService.SetUserCategory(chatId, null);
            _userSessionService.SetUserSubtopic(chatId, null);
            _userSessionService.SetUserTest(chatId, null);
            _userSessionService.SetUserTakingTest(chatId, false);
            _userSessionService.SetCurrentTopic(chatId, null);
            await _dbService.SaveUserSessionAsync(chatId, _userSessionService.GetSession(chatId));

            // Теперь показываем следующее меню (и можно его тоже сохранить, если нужно)
            var nextKeyboard = LearningOptionsKeyboard.GetLearningOptionsKeyboard();

            _userSessionService.PushMenuState(chatId, new MenuState
            {
                Text = "Выберите действие",
                Keyboard = nextKeyboard
            });

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Вы выбрали {messageText}",
                cancellationToken: cancellationToken);

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Выберите действие",
                replyMarkup: nextKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
