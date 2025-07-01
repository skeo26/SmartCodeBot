using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SmartCodeBot.Services
{
    public interface IUserInitializerService
    {
        Task InitializeUserSessionAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken);
    }
}
