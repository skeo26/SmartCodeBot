using SmartCodeBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.Providers
{
    public class TestProvider : ITestProvider
    {
        private readonly string _basePath = "D:\\Data\\Tests"; // Папка с тестами

        public async Task<List<string>> GetAvailableTestsAsync(string language, string category)
        {
            string categoryPath = Path.Combine(_basePath, language, category);

            if (!Directory.Exists(categoryPath))
                return new List<string> { "Тесты не найдены" };

            var files = Directory.GetFiles(categoryPath, "*.txt")
                                 .Select(Path.GetFileNameWithoutExtension)
                                 .ToList();

            return files.Any() ? files : new List<string> { "Тесты не найдены" };
        }

        public async Task<List<TestQuestion>> GetTestQuestionsAsync(string language, string category, string topicName)
        {
            string filePath = Path.Combine(_basePath, language, category, topicName + ".txt");

            if (!File.Exists(filePath))
                return new List<TestQuestion> { new TestQuestion("❌ Тест не найден", new List<string>(), string.Empty, -1) };

            var lines = await File.ReadAllLinesAsync(filePath);

            if (lines.Length < 3)
                return new List<TestQuestion> { new TestQuestion("⚠️ Ошибка в формате теста", new List<string>(), string.Empty, -1) };

            var questions = new List<TestQuestion>();
            var currentQuestionLines = new List<string>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (currentQuestionLines.Count > 0)
                    {
                        var question = ParseQuestion(currentQuestionLines);
                        questions.Add(question);
                        currentQuestionLines.Clear();
                    }
                }
                else
                {
                    currentQuestionLines.Add(line);
                }
            }

            // Добавляем последний вопрос, если он есть
            if (currentQuestionLines.Count > 0)
            {
                var question = ParseQuestion(currentQuestionLines);
                questions.Add(question);
            }

            return questions;
        }

        private TestQuestion ParseQuestion(List<string> lines)
        {
            if (lines.Count < 4)
                return new TestQuestion("⚠️ Ошибка в формате вопроса.", new List<string>(), string.Empty, -1);

            string question = string.Empty;
            List<string> options = new List<string>();
            string explanation = string.Empty;
            int correctAnswerIndex = -1;
            string? codeBlock = null;

            bool inCodeSection = false;
            List<string> codeLines = new List<string>();

            foreach (var line in lines)
            {
                if (line.StartsWith("Question:"))
                {
                    question = line.Substring(9).Trim();
                }
                else if (line.StartsWith("Code:"))
                {
                    inCodeSection = true;
                    codeLines.Clear();
                }
                else if (line.StartsWith("Options:"))
                {
                    if (inCodeSection)
                    {
                        codeBlock = string.Join("\n", codeLines);
                        inCodeSection = false;
                    }

                    options = lines.SkipWhile(l => l != "Options:").Skip(1)
                                   .TakeWhile(l => !l.StartsWith("Explanation:") && !l.StartsWith("CorrectAnswerIndex:"))
                                   .Select(l => l.TrimStart('-').Trim())
                                   .ToList();
                }
                else if (line.StartsWith("Explanation:"))
                {
                    explanation = line.Substring(12).Trim();
                }
                else if (line.StartsWith("CorrectAnswerIndex:"))
                {
                    if (!int.TryParse(line.Substring(19).Trim(), out correctAnswerIndex))
                    {
                        return new TestQuestion("⚠️ Некорректный индекс правильного ответа.", new List<string>(), string.Empty, -1);
                    }
                }
                else if (inCodeSection)
                {
                    codeLines.Add(line);
                }
            }

            if (inCodeSection)
            {
                codeBlock = string.Join("\n", codeLines);
            }

            if (options.Count < 2)
            {
                return new TestQuestion("⚠️ Недостаточно вариантов для вопроса.", new List<string>(), string.Empty, -1);
            }

            return new TestQuestion(question, options, explanation, correctAnswerIndex, codeBlock);
        }

    }
}
