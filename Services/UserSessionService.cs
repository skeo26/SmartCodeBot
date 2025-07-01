using SmartCodeBot.KeyboardsMarkup;
using SmartCodeBot.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace SmartCodeBot.Services
{
    public class UserSessionService : IUserSessionService
    {
        private readonly ConcurrentDictionary<long, string> _userLanguages = new();
        private readonly ConcurrentDictionary<long, string> _userCategories = new();
        private readonly ConcurrentDictionary<long, string> _userTests = new();
        private readonly ConcurrentDictionary<long, bool> _userTakingTest = new();
        private readonly ConcurrentDictionary<long, string> _userTakingSubtopic = new();
        private readonly Dictionary<long, Stack<MenuState>> _userMenuHistory = new();
        private readonly ConcurrentDictionary<long, string> _userCurrentTopic = new();
        private readonly HashSet<long> _initializedUsers = new HashSet<long>();

        public bool IsUserInitialized(long chatId)
        {
            return _initializedUsers.Contains(chatId);
        }

        public void SetUserInitialized(long chatId)
        {
            _initializedUsers.Add(chatId);
        }

        public void SetUserLanguage(long chatId, string language)
        {
            _userLanguages[chatId] = language;
        }

        public string GetUserLanguage(long chatId)
        {
            return _userLanguages.TryGetValue(chatId, out var language) ? language : string.Empty;
        }

        public void SetUserCategory(long chatId, string category)
        {
            _userCategories[chatId] = category;
        }

        public string GetUserCategory(long chatId)
        {
            return _userCategories.TryGetValue(chatId, out var category) ? category : string.Empty;
        }

        public void SetUserTest(long chatId, string test)
        {
            _userTests[chatId] = test;
        }

        public string GetUserTest(long chatId)
        {
            return _userTests.TryGetValue(chatId, out var test) ? test : string.Empty;
        }

        public bool IsUserTakingTest(long chatId)
        {
            return _userTakingTest.TryGetValue(chatId, out var isTakingTest) && isTakingTest;
        }

        public void SetUserTakingTest(long chatId, bool isTakingTest)
        {
            _userTakingTest[chatId] = isTakingTest;
        }

        public string GetUserSubtopic(long chatId)
        {
            return _userTakingSubtopic.TryGetValue(chatId, out var subtopic) ? subtopic : string.Empty;
        }

        public void SetUserSubtopic(long chatId, string subtopic)
        {
            _userTakingSubtopic[chatId] = subtopic;
        }

        public void PushMenuState(long chatId, MenuState menuState)
        {
            if (!_userMenuHistory.ContainsKey(chatId))
            {
                _userMenuHistory[chatId] = new Stack<MenuState>();
            }

            menuState.Language = GetUserLanguage(chatId);
            menuState.Category = GetUserCategory(chatId);
            menuState.Subtopic = GetUserSubtopic(chatId);
            menuState.Test = GetUserTest(chatId);
            menuState.IsTakingTest = IsUserTakingTest(chatId);
            menuState.CurrentTopic = GetCurrentTopic(chatId);

            _userMenuHistory[chatId].Push(menuState);
        }

        public MenuState PopPreviousMenu(long chatId)
        {
            if (_userMenuHistory.ContainsKey(chatId) && _userMenuHistory[chatId].Count > 1)
            {
                _userMenuHistory[chatId].Pop();
                return _userMenuHistory[chatId].Peek();
            }
            return null;
        }

        public void SetCurrentTopic(long chatId, string firstTopic)
        {
            _userCurrentTopic[chatId] = firstTopic;
        }

        public string GetCurrentTopic(long chatId)
        {
            return _userCurrentTopic.TryGetValue(chatId, out var  topic) ? topic : string.Empty;
        }

        public MenuState GetSession(long chatId)
        {
            return new MenuState
            {
                Language = GetUserLanguage(chatId),
                Category = GetUserCategory(chatId),
                Subtopic = GetUserSubtopic(chatId),
                Test = GetUserTest(chatId),
                IsTakingTest = IsUserTakingTest(chatId),
                CurrentTopic = GetCurrentTopic(chatId)
            };
        }

        public void SetSession(long chatId, MenuState session)
        {
            if (session == null) return;

            SetUserLanguage(chatId, session.Language ?? string.Empty);
            SetUserCategory(chatId, session.Category ?? string.Empty);
            SetUserSubtopic(chatId, session.Subtopic ?? string.Empty);
            SetUserTest(chatId, session.Test ?? string.Empty);
            SetUserTakingTest(chatId, session.IsTakingTest);
            SetCurrentTopic(chatId, session.CurrentTopic ?? string.Empty);
        }

    }
}
