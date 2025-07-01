using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.Providers
{
    public class FileTopicProvider : ITopicProvider
    {
        private readonly string _basePath = "D:\\Data\\Topics";

        public async Task<List<string>> GetCategoriesAsync(string language)
        {
            string languagePath = Path.Combine(_basePath, language);

            if (!Directory.Exists(languagePath))
                return new List<string> { "Темы не найдены" };

            return Directory.GetDirectories(languagePath)
                .Select(Path.GetFileName)
                .ToList();
        }

        public async Task<string> GetMaterialContentAsync(string language, string category, string fileName)
        {
            string filePath = Path.Combine(_basePath, language, category, $"{fileName}.txt");

            if (!File.Exists(filePath))
                return "Материал не найден.";

            return await File.ReadAllTextAsync(filePath);
        }

        public async Task<List<string>> GetTopicsAsync(string language, string category)
        {
            string categoryPath = Path.Combine(_basePath, language, category);

            if (!Directory.Exists(categoryPath))
                return new List<string> { "Файлы не найдены." };

            return Directory.GetFiles(categoryPath, "*.txt")
                            .Select(Path.GetFileNameWithoutExtension)
                            .ToList();
        }
    }
}
