using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.Commands.Contracts
{
    public interface ICommandGroup
    {
        Dictionary<string, IBotCommand> Commands { get; }
    }
}
