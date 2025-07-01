using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartCodeBot.Services
{
    using System.Text.Json;
    using RestSharp;

    public class AssistantService : IAssistantService
    {
        private readonly RestClient _client;
        private readonly string _apiKey;
        private const string ApiUrl = "https://api.intelligence.io.solutions/api/v1/chat/completions";
        private const string systemPrompt = "Ты помощник по программированию. Отвечай только на вопросы, касающиеся выбранного языка программирования. Отвечай чётко и по делу.";

        public AssistantService(string apiKey)
        {
            _apiKey = apiKey;
            _client = new RestClient(ApiUrl);
        }

        public async Task<string> AskAsync(string language, string question)
        {
            var request = new RestRequest();

            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddHeader("content-type", "application/json");

            var fullPrompt = $"{systemPrompt} Пользователь изучает язык: {language}. Вопрос: {question}. Отвечай только по теме.";

            var body = new
            {
                model = "deepseek-ai/DeepSeek-R1",
                messages = new[]
                {
                new { role = "user", content = fullPrompt }
            }
            };

            request.AddJsonBody(body);

            Console.WriteLine($"[LOG] Запрос отправлен в нейросеть в: {DateTime.Now}");
            var response = await _client.PostAsync(request);
            Console.WriteLine($"[LOG] Ответ получен из нейросети в: {DateTime.Now}");

            if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
                return "Ошибка при получении ответа от помощника.";

            var json = JsonDocument.Parse(response.Content);
            var reply = json.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
            
            reply = reply.Split("</think>")[1].ToString();

            return reply;
        }
    }

}
