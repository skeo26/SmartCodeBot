using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.Models
{
    public class TestQuestion
    {
        public string Question { get; }
        public List<string> Options { get; }
        public string Explanation { get; }
        public int CorrectIndex { get; }
        public string? CodeBlock { get; }

        public TestQuestion(string question, List<string> options, string explanation, int correctIndex, string? codeBlock = null)
        {
            Question = question;
            Options = options;
            Explanation = explanation;
            CorrectIndex = correctIndex;
            CodeBlock = codeBlock;
        }
    }
}
