using System;
using System.Text;

namespace Mini_Oyun.Core.Security
{
    public sealed class Base64EncoderService : IEncoderService
    {
        public string Encode(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        public string Decode(string encodedText)
        {
            if (string.IsNullOrEmpty(encodedText)) return string.Empty;
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedText));
        }
    }
}