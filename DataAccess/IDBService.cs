using SmartCodeBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.DataAccess
{
    public interface IDBService
    {
        Task<int> GetOrCreateUserAsync(long chatId, string username);
        Task<UserSessionData> GetUserSessionAsync(long chatId);
        Task SaveUserSessionAsync(long userId, MenuState session);
        Task SaveFeedbackAsync(long chatId, string message);
    } 
}
