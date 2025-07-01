using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCodeBot.Commands.Contracts;
using Telegram.Bot;

namespace SmartCodeBot.Commands.MainMenuCommands
{
    public class SupportCommand : IBotCommand
    {
        public string CommandText => "Поддержка";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Если у вас возникли проблемы, свяжитесь с поддержкой: ",
                cancellationToken: cancellationToken);
        }
    }
}
