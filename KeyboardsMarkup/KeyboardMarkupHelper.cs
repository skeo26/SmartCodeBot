using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace SmartCodeBot.KeyboardsMarkup
{
    public class KeyboardMarkupHelper
    {
        public static string GetWelcomeText()
        {
            return "👋 Привет! Рад тебя видеть! \n\n"
                    + "Я бот для изучения языков программирования. 📚\n"
                    + "На данный момент доступны:\n"
                    + "✅ C#\n"
                    + "✅ SQL\n\n"
                    + "В будущем будут добавлены новые функции и языки! 🚀";
        }       
    }
}
