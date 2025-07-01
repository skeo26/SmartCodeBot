using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.Services
{
    public interface IAssistantService
    {
        Task<string> AskAsync(string language, string question);
    }
}
