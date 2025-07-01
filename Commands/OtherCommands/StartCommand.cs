using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.KeyboardsMarkup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SmartCodeBot.Commands.OtherCommands
{
    public class StartCommand : IBotCommand
    {
        public string CommandText => "/start";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            string welcomeMessage = KeyboardMarkupHelper.GetWelcomeText();

            await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: welcomeMessage,
               replyMarkup: MainMenuKeyboard.GetMainKeyboard(),
               cancellationToken: cancellationToken);
        }
    }
}
