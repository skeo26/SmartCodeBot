using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace SmartCodeBot.KeyboardsMarkup
{
    public class LanguageSelectionKeyboard
    {
        public static ReplyKeyboardMarkup GetLanguageSelectionKeyboard()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "C#", "MySQL" },
                new KeyboardButton[] { "Вернуться на главную" }
            })
            {
                ResizeKeyboard = true
            };
        }
    }
}
