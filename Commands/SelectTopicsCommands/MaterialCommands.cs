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
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SmartCodeBot.Commands.SelectTopicsCommands
{
    public class ReadMaterialCommand : IBotCommand
    {
        private readonly ITopicProvider _topicProvider;
        private readonly IUserSessionService _userSessionService;

        public ReadMaterialCommand(ITopicProvider topicProvider, IUserSessionService userSessionService)
        {
            _topicProvider = topicProvider;
            _userSessionService = userSessionService;
        }

        public string CommandText => "Выбрать материал";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string fileName, CancellationToken cancellationToken)
        {
            string language = _userSessionService.GetUserLanguage(chatId);
            string category = _userSessionService.GetUserCategory(chatId);

            _userSessionService.PushMenuState(chatId, new MenuState
            {
                Text = CommandText,
                Keyboard = LearningOptionsKeyboard.GetReturnKeyboard()
            });

            if (string.IsNullOrEmpty(language) || string.IsNullOrEmpty(category))
            {
                await botClient.SendTextMessageAsync(chatId, "Сначала выберите язык и категорию!", cancellationToken: cancellationToken);
                return;
            }

            _userSessionService.SetCurrentTopic(chatId, fileName);
            string content = await _topicProvider.GetMaterialContentAsync(language, category, fileName);
            //await botClient.SendTextMessageAsync(chatId, $"📖 {fileName}\n\n{content}", parseMode: ParseMode.Html, cancellationToken: cancellationToken);


            //List<string> otherTopics = await _topicProvider.GetTopicsAsync(language, category);
            //otherTopics.Remove(fileName);

            var keyboard = LearningOptionsKeyboard.GetEndOfMaterialKeyboard();
            await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"📖 {fileName}\n\n{content}",
                    parseMode: ParseMode.Html,
                    replyMarkup: keyboard,
                    cancellationToken: cancellationToken);
        }
    }
}
