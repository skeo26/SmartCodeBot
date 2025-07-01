using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using SmartCodeBot.Providers;
using System.Windows.Input;
using SmartCodeBot.DataAccess;
using SmartCodeBot.Commands.MainMenuCommands;
using SmartCodeBot.CommandProccesors;

namespace SmartCodeBot.Handlers
{
    public class BotCommandHandler : IBotCommandHandler
    {
        private readonly IEnumerable<ICommandProcessor> _processors;

        public BotCommandHandler(IEnumerable<ICommandProcessor> processors)
        {
            _processors = processors;
        }

        public async Task HandleCommandAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            foreach (var processor in _processors)
            {
                if (await processor.TryProcessAsync(botClient, chatId, messageText, cancellationToken))
                    return;
            }

            Console.WriteLine($"Необработанная команда: {messageText}");
        }
    }
}
