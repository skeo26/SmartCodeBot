using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.Services
{
    public interface IUserQuestionBuffer
    {
        void MarkAwaitingQuestion(long chatId);
        bool HasPendingQuestion(long chatId);
        void ClearPending(long chatId);
    }
}
