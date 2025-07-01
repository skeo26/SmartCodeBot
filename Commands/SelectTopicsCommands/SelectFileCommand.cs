using SmartCodeBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.KeyboardsMarkup;
using SmartCodeBot.Providers;
using SmartCodeBot.Models;

namespace SmartCodeBot.Commands.SelectTopicsCommands
{
    public class SelectFileCommand : IBotCommand
    {
        private readonly ITopicProvider _topicProvider;
        private readonly IUserSessionService _userSessionService;

        public SelectFileCommand(ITopicProvider topicProvider, IUserSessionService userSessionService)
        {
            _topicProvider = topicProvider;
            _userSessionService = userSessionService;
        }

        public string CommandText => "Выбрать подтему";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string category, CancellationToken cancellationToken)
        {
            string language = _userSessionService.GetUserLanguage(chatId);

            if (string.IsNullOrEmpty(language))
            {
                await botClient.SendTextMessageAsync(chatId, "Сначала выберите язык!", cancellationToken: cancellationToken);
                return;
            }

            List<string> files = await _topicProvider.GetTopicsAsync(language, category);
            if (files.Count == 0 || (files.Count == 1 && files[0] == "Файлы не найдены."))
            {
                await botClient.SendTextMessageAsync(chatId, "Файлы не найдены в этой теме.",
                         replyMarkup: LearningOptionsKeyboard.GetReturnKeyboard(), 
                         cancellationToken: cancellationToken);

            }

            var keyboard = LearningOptionsKeyboard.GetTopicKeyboardForLanguage(files);

            _userSessionService.SetUserCategory(chatId, category);
            _userSessionService.PushMenuState(chatId, new MenuState
            {
                Text = CommandText,
                Keyboard = LearningOptionsKeyboard.GetTopicKeyboardForLanguage(files),
            });

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"📄Вы выбрали тему - {category}. Выберите подтему для изучения:",
                replyMarkup: keyboard,
                cancellationToken: cancellationToken);

        }
    }
}
