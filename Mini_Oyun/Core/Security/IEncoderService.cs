using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Oyun.Core.Security
{
    public interface IEncoderService
    {
        string Encode(string text);
        string Decode(string encodedText);
    }
}
