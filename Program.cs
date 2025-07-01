using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartCodeBot.Commands.MainMenuCommands;
using Telegram.Bot;
using SmartCodeBot.Commands.LaguageMenuCommands;
using SmartCodeBot.Commands.LearningOptionsCommands;
using SmartCodeBot.Services;
using SmartCodeBot.Commands.LearningMaterialCommand;
using SmartCodeBot.RegisterCommand;
using SmartCodeBot.Commands.Contracts;
using SmartCodeBot.Commands.OtherCommands;
using SmartCodeBot.Commands.SelectTopicsCommands;
using SmartCodeBot.Providers;
using SmartCodeBot.Commands.TestCommands;
using SmartCodeBot.Commands.NavigationCommands;
using SmartCodeBot.Handlers;
using SmartCodeBot.DataAccess;
using Microsoft.Extensions.Configuration;
using SmartCodeBot.CommandProccesors;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .ConfigureServices((context, services) =>
            {
                var config = context.Configuration;

                // 1. Получаем значения из конфигурации
                var botToken = config["Bot:Token"];
                var connectionString = config["Database:ConnectionString"];
                var apiKey = config["DeepSeek:ApiKey"];

                // 2. Регистрируем зависимости с этими значениями
                services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));
                services.AddSingleton<IDBService>(new DataBaseService(connectionString));
                services.AddSingleton<IAssistantService>(new AssistantService(apiKey));

                // Остальная регистрация команд, провайдеров и хендлеров — без изменений
                services.AddSingleton<IBotCommandHandler, BotCommandHandler>();
                services.AddSingleton<IUpdateHandler, UpdateHandler>();
                services.AddHostedService<TelegramBotService>();

                services.AddSingleton<IUserSessionService, UserSessionService>();
                services.AddSingleton<IUserInitializerService, UserInitializerService>();
                services.AddSingleton<IUserQuestionBuffer, UserQuestionBuffer>();

                services.AddSingleton<ITopicProvider, FileTopicProvider>();
                services.AddSingleton<ITestProvider, TestProvider>();
                services.AddSingleton<IAdditionalProvider, AdditionalProvider>();

                services.AddSingleton<ICommandRegistry, CommandRegistry>();
                services.AddSingleton<ICommandGroup, MainMenuCommands>();
                services.AddSingleton<ICommandGroup, LanguageSelectionCommands>();
                services.AddSingleton<ICommandGroup, LearningOptionsCommands>();

                // Процессоры
                services.AddSingleton<ICommandProcessor, AssistantCommandProcessor>();
                services.AddSingleton<ICommandProcessor, DirectCommandProcessor>();
                services.AddSingleton<ICommandProcessor, FeedbackCommandProcessor>();
                services.AddSingleton<ICommandProcessor, GroupCommandProcessor>();
                services.AddSingleton<ICommandProcessor, InitCommandProcessor>();
                services.AddSingleton<ICommandProcessor, LearningFlowProcessor>();

                // Команды...
                services.AddSingleton<IBotCommand, StartCommand>();
                services.AddSingleton<IBotCommand, BackToMainMenuCommand>();
                services.AddSingleton<IBotCommand, LanguageSelectionCommand>();
                services.AddSingleton<IBotCommand, FeedbackCommand>();
                services.AddSingleton<IBotCommand, SupportCommand>();
                services.AddSingleton<IBotCommand, LanguageChoiceCommand>();
                services.AddSingleton<IBotCommand, BackCommand>();
                services.AddSingleton<IBotCommand, AskAssistantCommand>();
                services.AddSingleton<IBotCommand, StartLearningCommand>();
                services.AddSingleton<IBotCommand, SelectTopicCommand>();
                services.AddSingleton<IBotCommand, TakeTestCommand>();
                services.AddSingleton<IBotCommand, SelectSubtopicCommand>();
                services.AddSingleton<IBotCommand, NextTopicCommand>();
                services.AddSingleton<IBotCommand, PreviousTopicCommand>();
                services.AddSingleton<IBotCommand, SelectFileCommand>();
                services.AddSingleton<IBotCommand, ReadMaterialCommand>();
                services.AddSingleton<IBotCommand, ReturnToTopicSelectionCommand>();
                services.AddSingleton<IBotCommand, AdditionalMaterialCommand>();
                services.AddSingleton<IBotCommand, StartTestCommand>();
            })
            .Build();

        await host.RunAsync();
    }

}