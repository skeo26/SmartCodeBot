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
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SmartCodeBot.Commands.LearningMaterialCommand
{
    public class NextTopicCommand : IBotCommand
    {
        private readonly IUserSessionService _userSessionService;
        private readonly ITopicProvider _topicProvider;

        public NextTopicCommand(IUserSessionService userSessionService, ITopicProvider topicProvider)
        {
            _userSessionService = userSessionService;
            _topicProvider = topicProvider;
        }

        public string CommandText => "Следующая тема";

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

            if (currentIndex < topics.Count - 1)
            {
                _userSessionService.SetCurrentTopic(chatId, topics[currentIndex + 1]);
            }
            else
            {
                var categories = await _topicProvider.GetCategoriesAsync(language);
                var currentCatIndex = categories.IndexOf(category);

                if (currentCatIndex < categories.Count - 1)
                {
                    var nextCategory = categories[currentCatIndex + 1];
                    var nextTopics = await _topicProvider.GetTopicsAsync(language, nextCategory);

                    if (nextTopics.Any())
                    {
                        _userSessionService.SetUserCategory(chatId, nextCategory);
                        _userSessionService.SetCurrentTopic(chatId, nextTopics[0]);

                        category = nextCategory;
                        currentTopic = nextTopics[0];
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId, "В следующей категории нет тем.", cancellationToken: cancellationToken);
                        return;
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Вы завершили все доступные темы!", cancellationToken: cancellationToken);
                    return;
                }
            }

            var updatedTopic = _userSessionService.GetCurrentTopic(chatId);
            var updatedContent = await _topicProvider.GetMaterialContentAsync(language, category, updatedTopic);

            var keyboard = LearningOptionsKeyboard.GetEndOfMaterialKeyboard();
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"📖 {updatedTopic}\n\n{updatedContent}",
                replyMarkup: keyboard,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
    }
}
