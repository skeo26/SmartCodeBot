using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SmartCodeBot.Handlers
{
    public interface IBotCommandHandler
    {
        Task HandleCommandAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken);
    }
}
