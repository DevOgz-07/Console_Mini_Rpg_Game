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
    public class Oge // İtem Sınıfı.
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
            Console.ForegroundColor = NadirlikRengiGetir(this.Nadirlik); 
            Console.Write($"- {Ad} [{Nadirlik}]");
            Console.ResetColor();

            if (EtkiDegeri > 0)
            {
                string birim = Tur == OgeTuru.Silah ? "Saldırı" : (Tur == OgeTuru.Zirh ? "Savunma" : "Can");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($" (+{EtkiDegeri} {birim})");
            }
            else Console.WriteLine();
            Console.ResetColor();
        }


        public static ConsoleColor NadirlikRengiGetir(Nadirlik seviye)
        {
            switch (seviye)
            {
                case Nadirlik.Common:
                    return ConsoleColor.White;
                case Nadirlik.Uncommon:
                    return ConsoleColor.Green;
                case Nadirlik.Rare:
                    return ConsoleColor.Blue;
                case Nadirlik.Epic:
                    return ConsoleColor.Magenta;
                case Nadirlik.Legendary:
                    return ConsoleColor.Yellow;
                case Nadirlik.Mythic:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.Gray;
            }
        }


    }
}
