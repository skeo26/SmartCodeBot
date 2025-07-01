using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.Providers
{
    public interface IAdditionalProvider
    {
        Task<string> GetAdditionalMaterial(string language);
    }
}
