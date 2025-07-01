using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.KeyboardsMarkup;
using SmartCodeBot.Services;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SmartCodeBot.Commands.OtherCommands
{
    public class BackCommand : IBotCommand
    {
        private readonly IUserSessionService _userSessionService;

        public BackCommand(IUserSessionService userSessionService)
        {
            _userSessionService = userSessionService;
        }

        public string CommandText => "Назад";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string _, CancellationToken cancellationToken)
        {
            var previousMenu = _userSessionService.PopPreviousMenu(chatId);

            if (previousMenu == null)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Вы уже в главном меню.",
                    replyMarkup: MainMenuKeyboard.GetMainKeyboard(),
                    cancellationToken: cancellationToken);
                return;
            }

            _userSessionService.SetUserCategory(chatId, previousMenu.Category);
            _userSessionService.SetUserSubtopic(chatId, previousMenu.Subtopic);
            _userSessionService.SetUserTest(chatId, previousMenu.Test);
            _userSessionService.SetUserTakingTest(chatId, previousMenu.IsTakingTest);
            _userSessionService.SetCurrentTopic(chatId, previousMenu.CurrentTopic);

            var messageText = string.IsNullOrWhiteSpace(previousMenu.Text)
                ? "Вы вернулись в предыдущее меню."
                : previousMenu.Text;

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: messageText,
                replyMarkup: previousMenu.Keyboard,
                cancellationToken: cancellationToken);
        }
    }
}
