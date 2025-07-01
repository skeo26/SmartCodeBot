using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace SmartCodeBot.Models
{
    public class MenuState
    {
        public string Text { get; set; }
        public ReplyKeyboardMarkup Keyboard { get; set; }

        public string Language { get; set; }
        public string Category { get; set; }
        public string Subtopic { get; set; }
        public string Test { get; set; }
        public bool IsTakingTest { get; set; }
        public string CurrentTopic { get; set; }
    }
}
