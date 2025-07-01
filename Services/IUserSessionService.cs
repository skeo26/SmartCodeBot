using SmartCodeBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SmartCodeBot.Services
{
    public interface IUserSessionService
    {
        void SetUserLanguage(long chatId, string language);
        string GetUserLanguage(long chatId);

        void SetUserCategory(long chatId, string category);
        string GetUserCategory(long chatId);

        void SetUserTest(long chatId, string category);
        string GetUserTest(long chatId);

        bool IsUserTakingTest(long chatId);
        void SetUserTakingTest(long chatId, bool isTakingTest);

        string GetUserSubtopic(long chatId);
        void SetUserSubtopic(long chatId, string category);

        void PushMenuState(long chatId, MenuState menuState);
        MenuState PopPreviousMenu(long chatId);

        void SetCurrentTopic(long chatId, string firstTopic);
        string GetCurrentTopic(long chatId);

        bool IsUserInitialized(long chatId);
        void SetUserInitialized(long chatId);

        MenuState GetSession(long chatId);
        void SetSession(long chatId, MenuState session);

    }
}
