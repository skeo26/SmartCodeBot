using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.Services
{
    public interface ITopicProgressService
    {
        int GetCurrentTopic(long chatId);
        void SetCurrentTopic(long chatId, int topicIndex);
    }
}
