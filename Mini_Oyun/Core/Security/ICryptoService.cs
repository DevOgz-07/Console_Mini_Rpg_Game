using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Oyun.Core.Security
{ 
    public interface ICryptoService
    {
        string ComputeHash(string text);
        bool VerifyHash(string text, string hash);
    }

    
}
