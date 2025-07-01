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
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SmartCodeBot.Commands.LearningOptionsCommands
{
    public class StartLearningCommand : IBotCommand
    {
        private readonly IUserSessionService _userSessionService;
        private readonly ITopicProvider _topicProvider;

        public StartLearningCommand(IUserSessionService userSessionService, ITopicProvider topicProvider)
        {
            _userSessionService = userSessionService;
            _topicProvider = topicProvider;
        }

        public string CommandText => "Начать изучение с 0";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            var language = _userSessionService.GetUserLanguage(chatId);

            if (string.IsNullOrEmpty(language))
            {
                await botClient.SendTextMessageAsync(chatId, "Выберите язык перед началом изучения!",
                    replyMarkup: LanguageSelectionKeyboard.GetLanguageSelectionKeyboard(),
                    cancellationToken: cancellationToken);
                return;
            }

            var categories = await _topicProvider.GetCategoriesAsync(language);
            if (!categories.Any(c => c != "Темы не найдены"))
            {
                await botClient.SendTextMessageAsync(chatId, "Нет доступных тем для изучения.", cancellationToken: cancellationToken);
                return;
            }

            var firstCategory = categories.First();
            _userSessionService.SetUserCategory(chatId, firstCategory);

            var topics = await _topicProvider.GetTopicsAsync(language, firstCategory);
            if (!topics.Any())
            {
                await botClient.SendTextMessageAsync(chatId, "Нет доступных материалов в данной категории.", cancellationToken: cancellationToken);
                return;
            }

            var firstTopic = topics.First();
            _userSessionService.SetCurrentTopic(chatId, firstTopic);

            var content = await _topicProvider.GetMaterialContentAsync(language, firstCategory, firstTopic);

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"📖 {firstTopic}\n\n{content}",
                replyMarkup: NavigationMenuCommands.NavigationMenu(),
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
    }
}

