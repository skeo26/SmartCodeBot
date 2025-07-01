using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.Providers
{
    public interface ITopicProvider
    {
        Task<List<string>> GetCategoriesAsync(string language);
        Task<List<string>> GetTopicsAsync(string language, string category);
        Task<string> GetMaterialContentAsync(string language, string category, string fileName);
    }
}
