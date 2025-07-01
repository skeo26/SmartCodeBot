using SmartCodeBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.Providers
{
    public interface ITestProvider
    {
        Task<List<string>> GetAvailableTestsAsync(string language, string category);
        Task<List<TestQuestion>> GetTestQuestionsAsync(string language, string category, string topicName);
    }
}
