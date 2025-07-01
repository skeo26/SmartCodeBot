using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace SmartCodeBot.KeyboardsMarkup
{
    public class LearningOptionsKeyboard
    {
        public static ReplyKeyboardMarkup GetLearningOptionsKeyboard()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "Начать изучение с нуля"},
                new KeyboardButton[] { "Выбрать тему"},
                new KeyboardButton[] { "Пройти тестирование"},
                new KeyboardButton[] { "SmartCode помощник"},
                new KeyboardButton[] { "Дополнительный материал"},
                new KeyboardButton[] { "Вернуться на главную" },
                new KeyboardButton[] { "Назад" }
            })
            {
                ResizeKeyboard = true
            };
        }

        public static ReplyKeyboardMarkup GetLanguageChoice()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "C#", "MySQL"}
            })
            {
                ResizeKeyboard = true
            };
        }

        public static ReplyKeyboardMarkup GetOthersTopicsAndTestKeyboard(List<string> topics)
        {
            var buttons = topics.Select(topic => new KeyboardButton(topic)).ToList();
            buttons.Add(new KeyboardButton("Пройти тестирование"));

            return new ReplyKeyboardMarkup(buttons.Chunk(2))
            {
                ResizeKeyboard = true
            };
        }

        public static ReplyKeyboardMarkup GetTopicKeyboardForLanguage(List<string> topics)
        {
            var buttons = topics.Select(t => new KeyboardButton(t)).Chunk(2).ToList();

            buttons.Add(new[]
            {
                new KeyboardButton("Вернуться на главную"),
                new KeyboardButton("Назад")
            });

            return new ReplyKeyboardMarkup(buttons)
            {
                ResizeKeyboard = true
            };
        }


        public static ReplyKeyboardMarkup GetReturnKeyboard()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "Вернуться на главную", "Назад" }
            })
            {
                ResizeKeyboard = true
            };
        }

        public static ReplyKeyboardMarkup GetEndOfMaterialKeyboard()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton [] {"Следующая тема", "Предыдущая тема"},
                new KeyboardButton[] { "Вернуться на главную"},
                new KeyboardButton[] { "Начать тест"}
            })
            {
                ResizeKeyboard = true
            };
        }

        public static ReplyKeyboardMarkup OnlyReturnToMainMenu()
        {
            return new ReplyKeyboardMarkup(new[]
               {
                    new KeyboardButton[] { "Вернуться на главную" }
                })
            {
                ResizeKeyboard = true
            };
        }
    }
}
