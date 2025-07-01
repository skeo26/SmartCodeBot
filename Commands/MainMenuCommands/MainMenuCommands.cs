using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.DataAccess;

namespace SmartCodeBot.Commands.MainMenuCommands
{
    public class MainMenuCommands : ICommandGroup
    {
        public Dictionary<string, IBotCommand> Commands { get; }

        public MainMenuCommands(IDBService dbService)
        {
            Commands = new Dictionary<string, IBotCommand>
        {
            { "Выбрать язык", new LanguageSelectionCommand() },
            { "Обратная связь", new FeedbackCommand(dbService) },
            { "Поддержка", new SupportCommand() }
        };
        }
    }

}
