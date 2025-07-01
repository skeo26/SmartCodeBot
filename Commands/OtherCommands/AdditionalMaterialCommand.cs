using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.KeyboardsMarkup;
using SmartCodeBot.Models;
using SmartCodeBot.Providers;
using SmartCodeBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace SmartCodeBot.Commands.OtherCommands
{
    public class AdditionalMaterialCommand : IBotCommand
    {
        private readonly IUserSessionService _userSessionService;
        private readonly IAdditionalProvider _additionalProvider;

        public AdditionalMaterialCommand(IUserSessionService userSessionService, IAdditionalProvider additionalProvider)
        {
            _userSessionService = userSessionService;
            _additionalProvider = additionalProvider;
        }

        public string CommandText => "Дополнительный материал";

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
        {
            var language = _userSessionService.GetUserLanguage(chatId);

            if (string.IsNullOrEmpty(language))
            {
                await botClient.SendTextMessageAsync(chatId, "Сначала выберите язык",
                    replyMarkup: LanguageSelectionKeyboard.GetLanguageSelectionKeyboard(),
                    cancellationToken: cancellationToken);
                return;
            }

            var content = await _additionalProvider.GetAdditionalMaterial(language);
            if (content == "Дополнительного материала не найдено")
            {
                await botClient.SendTextMessageAsync(
                      chatId: chatId,
                      text: "Дополнительного материала по данному языку не найдено",
                      replyMarkup: LearningOptionsKeyboard.GetLearningOptionsKeyboard(),
                      cancellationToken: cancellationToken);
                return;
            }
            else
            { 
                 var keyboard = LearningOptionsKeyboard.GetReturnKeyboard();

                _userSessionService.PushMenuState(chatId, new MenuState
                {
                    Text = CommandText,
                    Keyboard = LearningOptionsKeyboard.GetReturnKeyboard()
                });

                await botClient.SendTextMessageAsync(
                   chatId: chatId,
                   text: $"{content}",
                   replyMarkup: keyboard,
                   parseMode: ParseMode.Html,
                   cancellationToken: cancellationToken);
            }
        }
    }
}
