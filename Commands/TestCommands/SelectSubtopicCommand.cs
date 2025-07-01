using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.KeyboardsMarkup;
using SmartCodeBot.Models;
using SmartCodeBot.Providers;
using SmartCodeBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SmartCodeBot.Commands.TestCommands
{
    public class SelectSubtopicCommand : IBotCommand
    {
        private readonly IUserSessionService _userSessionService;
        private readonly ITestProvider _testProvider;

        public SelectSubtopicCommand(IUserSessionService userSessionService, ITestProvider testProvider)
        {
            _userSessionService = userSessionService;
            _testProvider = testProvider;
        }

        public string CommandText => "Выбрать подтему теста";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            var category = _userSessionService.GetUserCategory(chatId);
            if (string.IsNullOrEmpty(category))
            {
                await botClient.SendTextMessageAsync(chatId, "⚠️ Сначала выберите категорию!", cancellationToken: cancellationToken);
                return;
            }

            var language = _userSessionService.GetUserLanguage(chatId);
            var subtopics = await _testProvider.GetAvailableTestsAsync(language, category);

            if (subtopics == null || subtopics.Count == 0 || subtopics[0] == "Тесты не найдены")
            {
                await botClient.SendTextMessageAsync(chatId, "❌ В этой категории нет доступных тестов.", cancellationToken: cancellationToken);
                _userSessionService.SetUserCategory(chatId, null);
                _userSessionService.SetUserSubtopic(chatId, null);
                return;
            }

            _userSessionService.SetUserCategory(chatId, messageText);

            var keyboardForSubtopics = LearningOptionsKeyboard.GetTopicKeyboardForLanguage(subtopics);
            _userSessionService.PushMenuState(chatId, new MenuState
            {
                Text = CommandText,
                Keyboard = LearningOptionsKeyboard.GetTopicKeyboardForLanguage(subtopics)
            });
            await botClient.SendTextMessageAsync(chatId, "Выберите подтему:", replyMarkup: keyboardForSubtopics, cancellationToken: cancellationToken);
        }
    }
}
