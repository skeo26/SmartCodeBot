using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.KeyboardsMarkup;
using SmartCodeBot.Models;
using SmartCodeBot.Providers;
using SmartCodeBot.Services;
using Telegram.Bot;

namespace SmartCodeBot.Commands.LearningOptionsCommands
{
    public class TakeTestCommand : IBotCommand
    {
        private readonly IUserSessionService _userSessionService;
        private readonly ITopicProvider _topicProvider;

        public TakeTestCommand(IUserSessionService userSessionService, ITopicProvider topicProvider)
        {
            _userSessionService = userSessionService;
            _topicProvider = topicProvider;
        }

        public string CommandText => "Пройти тестирование";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            var language = _userSessionService.GetUserLanguage(chatId);

            if (string.IsNullOrEmpty(language))
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Сначала выберите язык!",
                    replyMarkup: LanguageSelectionKeyboard.GetLanguageSelectionKeyboard(),
                    cancellationToken: cancellationToken);
                return;
            }

            List<string> categories = await _topicProvider.GetCategoriesAsync(language);
            if (categories.Count == 0 || categories.Count == 1 && categories[0] == "Темы не найдены")
            {
                await botClient.SendTextMessageAsync(chatId, "❌ Для данного языка нет доступных тем для тестирования", cancellationToken: cancellationToken);
                return;
            }

            _userSessionService.SetUserTakingTest(chatId, true);

            var keyboardForTopics = LearningOptionsKeyboard.GetTopicKeyboardForLanguage(categories);
            _userSessionService.PushMenuState(chatId, new MenuState
            {
                Text = CommandText,
                Keyboard = LearningOptionsKeyboard.GetTopicKeyboardForLanguage(categories)
            });
            await botClient.SendTextMessageAsync(chatId, "Выберите тему для тестирования:", replyMarkup: keyboardForTopics, cancellationToken: cancellationToken);
        }
    }
}
