using System;

namespace Mini_Oyun.Core.Security
{
    public static class VeriKoruma
    {
        private const string GizliAnahtar = "Gumusisik_Sirlari_2026_Secure!";
        public static ICryptoService Crypto { get; }
        public static IEncoderService Encoder { get; }

        static VeriKoruma()
        {
            Crypto = new Sha256CryptoService(GizliAnahtar);
            Encoder = new Base64EncoderService();
        }
    }
}