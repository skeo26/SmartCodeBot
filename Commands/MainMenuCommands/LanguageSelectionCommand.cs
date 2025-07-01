using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.KeyboardsMarkup;
using Telegram.Bot;

namespace SmartCodeBot.Commands.MainMenuCommands
{
    public class LanguageSelectionCommand : IBotCommand
    {
        public string CommandText => "Выбрать язык";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Выберите язык:",
            replyMarkup: LanguageSelectionKeyboard.GetLanguageSelectionKeyboard(),
            cancellationToken: cancellationToken);
        }
    }
}
