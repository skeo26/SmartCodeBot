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
using Telegram.Bot.Types.ReplyMarkups;

namespace SmartCodeBot.Commands.LearningOptionsCommands
{
    public class SelectTopicCommand : IBotCommand
    {
        private readonly ITopicProvider _topicProvider;
        private readonly IUserSessionService _userSessionService;

        public SelectTopicCommand(ITopicProvider topicProvider, IUserSessionService userSessionService)
        {
            _topicProvider = topicProvider;
            _userSessionService = userSessionService;

        }

        public string CommandText => "Выбрать тему";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            string language = _userSessionService.GetUserLanguage(chatId);  

            if (string.IsNullOrEmpty(language))
            {
                await botClient.SendTextMessageAsync(chatId, "Сначала выберите язык!", cancellationToken: cancellationToken);
                return;
            }

            string category = _userSessionService.GetUserCategory(chatId);

            if (string.IsNullOrEmpty(category))
            {
                List<string> categories = await _topicProvider.GetCategoriesAsync(language);

                if (categories.Count == 0 || (categories.Count == 1 && categories[0] == "Темы не найдены"))
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Темы не найдены для выбранного языка.",
                        replyMarkup: LearningOptionsKeyboard.GetLearningOptionsKeyboard(),
                        cancellationToken: cancellationToken);
                    return;
                }

                var keyboard = LearningOptionsKeyboard.GetTopicKeyboardForLanguage(categories);

                _userSessionService.PushMenuState(chatId, new MenuState
                {
                    Text = CommandText,
                    Keyboard = keyboard
                });

                await botClient.SendTextMessageAsync(chatId, "Выберите категорию:", replyMarkup: keyboard, cancellationToken: cancellationToken);
            }
            else
            {
                // Пользователь выбирает файл внутри категории
                List<string> files = await _topicProvider.GetTopicsAsync(language, category);

                if (files.Count == 0 || (files.Count == 1 && files[0] == "Файлы не найдены."))
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Файлы не найдены в выбранной категории.",
                        replyMarkup: LearningOptionsKeyboard.GetLearningOptionsKeyboard(),
                        cancellationToken: cancellationToken);
                    return;
                }

                var keyboard = LearningOptionsKeyboard.GetTopicKeyboardForLanguage(files);

                _userSessionService.PushMenuState(chatId, new MenuState
                {
                    Text = CommandText,
                    Keyboard = LearningOptionsKeyboard.GetTopicKeyboardForLanguage(files)
                 });

                await botClient.SendTextMessageAsync(chatId, "Выберите материал:", replyMarkup: keyboard, cancellationToken: cancellationToken);
            }
        }
    }
}
