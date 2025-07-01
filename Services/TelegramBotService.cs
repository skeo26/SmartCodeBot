using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.Handlers;

namespace SmartCodeBot.Services
{
    public class TelegramBotService : BackgroundService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUpdateHandler _updateHandler;

        public TelegramBotService(ITelegramBotClient botClient, IUpdateHandler updateHandler)
        {
            _botClient = botClient;
            _updateHandler = updateHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _botClient.StartReceiving(
                _updateHandler.HandleUpdateAsync,
                _updateHandler.HandleErrorAsync,
                cancellationToken: stoppingToken);

            Console.WriteLine("Бот запущен...");
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
