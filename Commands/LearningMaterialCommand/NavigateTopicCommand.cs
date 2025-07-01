using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.KeyboardsMarkup;
using SmartCodeBot.Providers;
using SmartCodeBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SmartCodeBot.Commands.LearningMaterialCommand
{
    public class NavigateTopicCommand : IBotCommand
    {
        private readonly IUserSessionService _userSessionService;
        private readonly ITopicProvider _topicProvider;

        public NavigateTopicCommand(IUserSessionService userSessionService, ITopicProvider topicProvider)
        {
            _userSessionService = userSessionService;
            _topicProvider = topicProvider;
        }

        public string CommandText => "Навигация по темам";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            var language = _userSessionService.GetUserLanguage(chatId);
            var category = _userSessionService.GetUserCategory(chatId);
            var currentTopic = _userSessionService.GetCurrentTopic(chatId);

            if (string.IsNullOrEmpty(language) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(currentTopic))
            {
                await botClient.SendTextMessageAsync(chatId, "Начните с выбора языка и темы.", cancellationToken: cancellationToken);
                return;
            }

            var topics = await _topicProvider.GetTopicsAsync(language, category);
            var currentIndex = topics.IndexOf(currentTopic);

            if (messageText == "Следующая тема" && currentIndex < topics.Count - 1)
            {
                _userSessionService.SetCurrentTopic(chatId, topics[currentIndex + 1]);
            }
            else if (messageText == "Предыдущая тема" && currentIndex > 0)
            {
                _userSessionService.SetCurrentTopic(chatId, topics[currentIndex - 1]);
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId, "Нет доступных тем в данном направлении.", cancellationToken: cancellationToken);
                return;
            }

            var newTopic = _userSessionService.GetCurrentTopic(chatId);
            var content = await _topicProvider.GetMaterialContentAsync(language, category, newTopic);

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: content,
                replyMarkup: NavigationMenuCommands.NavigationMenu(),
                cancellationToken: cancellationToken);
        }
    }

}
