using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.KeyboardsMarkup;
using SmartCodeBot.Models;
using SmartCodeBot.Providers;
using SmartCodeBot.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace SmartCodeBot.Commands.NavigationCommands
{
    public class ReturnToTopicSelectionCommand : IBotCommand
    {
        private readonly ITopicProvider _topicProvider;
        private readonly IUserSessionService _userSessionService;

        public ReturnToTopicSelectionCommand(ITopicProvider topicProvider, IUserSessionService userSessionService)
        {
            _topicProvider = topicProvider;
            _userSessionService = userSessionService;
        }

        public string CommandText => "Вернуться к выбору темы";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string _, CancellationToken cancellationToken)
        {
            string language = _userSessionService.GetUserLanguage(chatId);
            if (string.IsNullOrEmpty(language))
            {
                await botClient.SendTextMessageAsync(chatId, "Сначала выберите язык!",
                    replyMarkup: LearningOptionsKeyboard.GetReturnKeyboard(),
                    cancellationToken: cancellationToken);
                return;
            }

            var category = _userSessionService.GetUserCategory(chatId);
            if (string.IsNullOrEmpty(category))
            {
                await botClient.SendTextMessageAsync(chatId, "Сначала выберите категорию!",
                    replyMarkup: LearningOptionsKeyboard.GetReturnKeyboard(),
                    cancellationToken: cancellationToken);
                return;
            }

            List<string> topics = await _topicProvider.GetTopicsAsync(language, category);
            if (topics.Count == 0 || (topics.Count == 1 && topics[0] == "Файлы не найдены."))
            {
                await botClient.SendTextMessageAsync(chatId, "Файлы не найдены в этой категории.",
                    replyMarkup: LearningOptionsKeyboard.GetReturnKeyboard(),
                    cancellationToken: cancellationToken);
                return;
            }

            var keyboard = LearningOptionsKeyboard.GetTopicKeyboardForLanguage(topics);

            // Сохраняем текущее меню в историю
            _userSessionService.PushMenuState(chatId, new MenuState
            {
                Text = "📚 Выберите тему для изучения:",
                Keyboard = keyboard
            });

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "📚 Выберите тему для изучения:",
                replyMarkup: keyboard,
                cancellationToken: cancellationToken
            );
        }

    }
}
