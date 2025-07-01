using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.Models
{
    public class UserSessionData
    {
        public string Language { get; set; }
        public string Category { get; set; }
        public string Topic { get; set; }
        public bool IsTakingTest { get; set; }
        public string Test { get; set; }
    }

}
