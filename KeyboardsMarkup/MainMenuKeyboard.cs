using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace SmartCodeBot.KeyboardsMarkup
{
    public class MainMenuKeyboard
    {
        public static ReplyKeyboardMarkup GetMainKeyboard()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "Выбрать язык", "Обратная связь" },
                new KeyboardButton[] { "Поддержка" }
            })
            {
                ResizeKeyboard = true
            };
        }
    }
}
