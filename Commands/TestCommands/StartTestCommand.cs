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

namespace SmartCodeBot.Commands.TestCommands
{
    public class StartTestCommand : IBotCommand
    {
        private readonly ITopicProvider _topicProvider;
        private readonly IUserSessionService _userSessionService;
        private readonly ITestProvider _testProvider;

        public StartTestCommand(IUserSessionService userSessionService, ITopicProvider topicProvider, ITestProvider testProvider)
        {
            _userSessionService = userSessionService;
            _topicProvider = topicProvider;
            _testProvider = testProvider;
        }

        public string CommandText => "Начать тест";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string topicName, CancellationToken cancellationToken)
        {
            string language = _userSessionService.GetUserLanguage(chatId);
            var category = _userSessionService.GetUserCategory(chatId);

            if (string.IsNullOrEmpty(language) || string.IsNullOrEmpty(category))
            {
                await botClient.SendTextMessageAsync(chatId, "Сначала выберите язык и тему!", cancellationToken: cancellationToken);
                return;
            }

            List<string> topics = await _testProvider.GetAvailableTestsAsync(language, category);
            if (string.IsNullOrEmpty(topicName) || topicName == "Начать тест")
            {
                topicName = _userSessionService.GetCurrentTopic(chatId);
            }

            if (!topics.Contains(topicName))
            {
                await botClient.SendTextMessageAsync(chatId, "❌ Такой тест не найден. Выберите из списка.", cancellationToken: cancellationToken);
                return;
            }

            _userSessionService.SetUserTest(chatId, topicName);

            var questions = await _testProvider.GetTestQuestionsAsync(language, category, topicName);
            if (questions == null || questions.Count == 0 || questions[0].CorrectIndex == -1)
            {
                await botClient.SendTextMessageAsync(chatId, "⚠️ Вопросов для теста нет или они записаны некорректно.", cancellationToken: cancellationToken);
                return;
            }

            await botClient.SendTextMessageAsync(chatId, $"📝 Тест по теме: {topicName}", cancellationToken: cancellationToken);

            foreach (var testQuestion in questions)
            {
                if (!string.IsNullOrWhiteSpace(testQuestion.CodeBlock))
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"<pre>{testQuestion.CodeBlock}</pre>",
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken
                    );
                }

                var options = testQuestion.Options.Select(option => new Telegram.Bot.Types.InputPollOption { Text = option }).ToList();

                await botClient.SendPollAsync(
                    chatId: chatId,
                    question: testQuestion.Question,
                    options: options,
                    type: PollType.Quiz,
                    questionParseMode: ParseMode.Html,
                    correctOptionId: testQuestion.CorrectIndex,
                    isAnonymous: true,
                    explanation: testQuestion.Explanation,
                    cancellationToken: cancellationToken
                );

                await Task.Delay(700, cancellationToken);
            }

            _userSessionService.PushMenuState(chatId, new MenuState
            {
                Text = CommandText,
                Keyboard = LearningOptionsKeyboard.GetReturnKeyboard()
            });

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "✅ Тест завершен! Выберите следующее действие:",
                replyMarkup: LearningOptionsKeyboard.GetReturnKeyboard(),
                cancellationToken: cancellationToken
            );
        }
    }
}
