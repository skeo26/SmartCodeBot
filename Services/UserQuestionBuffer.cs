using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.Services
{
    public class UserQuestionBuffer : IUserQuestionBuffer
    {
        private readonly HashSet<long> _pendingUsers = new();

        public void MarkAwaitingQuestion(long chatId) => _pendingUsers.Add(chatId);

        public bool HasPendingQuestion(long chatId) => _pendingUsers.Contains(chatId);

        public void ClearPending(long chatId) => _pendingUsers.Remove(chatId);
    }
}
