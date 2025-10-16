using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Oyun
{
    public enum OgeTuru
    {
        Silah,
        Zirh,
        Tuketilebilir, // İksir Tarzı Şeyler.
    }
    internal class Oge // İtem Sınıfı.
    {
        public string Ad {  get; set; }

        public  Nadirlik Nadirlik { get; set; }

        public OgeTuru Tur { get; set; }

        public int EtkiDegeri { get; set; } // Saldırı Gücü, Can Yenileme vb.

        public Oge(string ad, Nadirlik nadirlik, OgeTuru tur, int etkiDegeri = 0)
        {
            Ad = ad;
            Nadirlik = nadirlik;
            Tur = tur;
            EtkiDegeri = etkiDegeri;
        } 

        public void BilgileriGoster()
        {
            string etkiBilgisi = "";
            if (EtkiDegeri > 0)
            {
                if (Tur == OgeTuru.Silah) etkiBilgisi = $", +{EtkiDegeri} Saldırı";
                if (Tur == OgeTuru.Tuketilebilir) etkiBilgisi = $", +{EtkiDegeri} Can";
            }
            Console.WriteLine($"-{Ad} ({Nadirlik}){etkiBilgisi}");
        }


    }
}
