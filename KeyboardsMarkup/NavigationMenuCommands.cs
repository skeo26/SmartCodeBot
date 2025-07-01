using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace SmartCodeBot.KeyboardsMarkup
{
    public class NavigationMenuCommands
    {
        public static ReplyKeyboardMarkup NavigationMenu()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "Следующая тема", "Предыдущая тема" },
                new KeyboardButton[] { "Вернуться на главную" },
            })
            {
                ResizeKeyboard = true
            };
        }


    }
}
