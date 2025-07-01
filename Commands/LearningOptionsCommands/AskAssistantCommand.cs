using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.KeyboardsMarkup;
using SmartCodeBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace SmartCodeBot.Commands.LearningOptionsCommands
{
    public class AskAssistantCommand : IBotCommand
    {
        public string CommandText => "SmartCode помощник";

        private readonly IAssistantService _assistantService;
        private readonly IUserSessionService _userSessionService;
        private readonly IUserQuestionBuffer _questionBuffer;

        public AskAssistantCommand(
            IAssistantService assistantService,
            IUserSessionService userSessionService,
            IUserQuestionBuffer questionBuffer)
        {
            _assistantService = assistantService;
            _userSessionService = userSessionService;
            _questionBuffer = questionBuffer;
        }

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            // Если это первый вызов команды, просим ввести вопрос
            if (!_questionBuffer.HasPendingQuestion(chatId))
            {
                _questionBuffer.MarkAwaitingQuestion(chatId);
                await botClient.SendTextMessageAsync(chatId,
                    "Введите ваш вопрос по программированию. Я постараюсь ответить:",
                    cancellationToken: cancellationToken);
                return;
            }

            _questionBuffer.ClearPending(chatId);
            string language = _userSessionService.GetUserLanguage(chatId);
            if (string.IsNullOrEmpty(language))
            {
                await botClient.SendTextMessageAsync(
                    chatId,
                    "❌ Сначала выберите язык!",
                    replyMarkup: LanguageSelectionKeyboard.GetLanguageSelectionKeyboard(),
                    cancellationToken: cancellationToken);
            }
            string question = messageText;

            await botClient.SendTextMessageAsync(chatId,
                    "⏳ Пожалуйста, подождите, идёт обработка вопроса...",
                    cancellationToken: cancellationToken);


            try
            {
                string reply = await _assistantService.AskAsync(language, question);
                await botClient.SendTextMessageAsync(chatId, reply, parseMode: ParseMode.Markdown, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId,
                    "❌ Произошла ошибка при получении ответа. Попробуйте позже.",
                    cancellationToken: cancellationToken);
                Console.WriteLine($"[Assistant Error]: {ex.Message}");
            }
        }
    }
}
