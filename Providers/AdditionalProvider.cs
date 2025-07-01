using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.Providers
{
    public class AdditionalProvider : IAdditionalProvider
    {
        private readonly string _basePath = "D:\\Data\\Topics";

        public async Task<string> GetAdditionalMaterial(string language)
        {
            if (string.IsNullOrEmpty(language))
                return "По данному материалу не найдено дополнительного материала";

            string filePath = Path.Combine(_basePath, language, "Дополнительный материал.txt");

            if (!File.Exists(filePath))
                return "Дополнительного материала не найдено";

            return await File.ReadAllTextAsync(filePath);
        }
    }
}
