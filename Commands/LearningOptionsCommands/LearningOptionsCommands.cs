using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.Providers;
using SmartCodeBot.Services;

namespace SmartCodeBot.Commands.LearningOptionsCommands
{
    public class LearningOptionsCommands : ICommandGroup
    {
        public Dictionary<string, IBotCommand> Commands { get; }
        private readonly IUserSessionService _userSessionService;
        private readonly ITopicProvider _topicProvider;

        public LearningOptionsCommands(IUserSessionService userSessionService, ITopicProvider topicProvider)
        {
            _userSessionService = userSessionService;
            _topicProvider = topicProvider;

            Commands = new Dictionary<string, IBotCommand>
            {
                { "Начать изучение с нуля", new StartLearningCommand(_userSessionService, _topicProvider) },
                { "Выбрать тему", new SelectTopicCommand(_topicProvider, _userSessionService) },
                { "Пройти тестирование", new TakeTestCommand(_userSessionService, _topicProvider) }
            };
        }
    }
}
