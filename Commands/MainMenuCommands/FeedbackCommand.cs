using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.DataAccess;
using SmartCodeBot.KeyboardsMarkup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SmartCodeBot.Commands.MainMenuCommands
{
    public class FeedbackCommand : IBotCommand
    {
        private static readonly Dictionary<long, bool> WaitingForFeedback = new();

        private readonly IDBService _dbService;

        public FeedbackCommand(IDBService dbService)
        {
            _dbService = dbService;
        }

        public string CommandText => "Обратная связь";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            // Если мы уже ждём ввод от пользователя — значит это его отзыв
            if (WaitingForFeedback.TryGetValue(chatId, out var isWaiting) && isWaiting)
            {
                // Сохраняем фидбэк и сбрасываем флаг
                await _dbService.SaveFeedbackAsync(chatId, messageText);
                WaitingForFeedback[chatId] = false;

                await botClient.SendTextMessageAsync(
                    chatId, 
                    "Спасибо за ваш отзыв!",
                    replyMarkup: MainMenuKeyboard.GetMainKeyboard(),
                    cancellationToken: cancellationToken);
            }
            else
            {
                // Включаем ожидание фидбэка
                WaitingForFeedback[chatId] = true;

                await botClient.SendTextMessageAsync(
                    chatId,
                    "Пожалуйста, введите сообщение для обратной связи. Или нажмите 'Вернуться в главное меню', чтобы отменить.",
                    replyMarkup: LearningOptionsKeyboard.OnlyReturnToMainMenu(),
                    cancellationToken: cancellationToken);
            }
        }

        public static void CancelFeedback(long chatId)
        {
            WaitingForFeedback.Remove(chatId);
        }

        public static bool IsWaiting(long chatId)
        {
            return WaitingForFeedback.TryGetValue(chatId, out var waiting) && waiting;
        }
    }
}
