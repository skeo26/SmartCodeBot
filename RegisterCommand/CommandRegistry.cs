using SmartCodeBot.Commands.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.RegisterCommand
{
    public class CommandRegistry : ICommandRegistry
    {
        private readonly Dictionary<string, IBotCommand> _commands;

        public CommandRegistry(IEnumerable<ICommandGroup> commandGroups, IEnumerable<IBotCommand> commands)
        {
            _commands = commandGroups
                .SelectMany(group => group.Commands)
                .Concat(commands.ToDictionary(cmd => cmd.CommandText, cmd => cmd))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public IBotCommand? GetCommand(string commandText)
        {
            _commands.TryGetValue(commandText, out var command);
            return command;
        }
    }
}
