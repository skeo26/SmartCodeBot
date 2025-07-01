using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.DataAccess;
using SmartCodeBot.Services;

namespace SmartCodeBot.Commands.LaguageMenuCommands
{
    public class LanguageSelectionCommands : ICommandGroup
    {
        public Dictionary<string, IBotCommand> Commands { get; }

        public LanguageSelectionCommands(IUserSessionService userSessionService, IDBService dbService)
        {
            var languageCommand = new LanguageChoiceCommand(userSessionService, dbService);

            Commands = new Dictionary<string, IBotCommand>
            {
                { "C#", languageCommand },
                { "MySQL", languageCommand }
            };
        }
    }
}
