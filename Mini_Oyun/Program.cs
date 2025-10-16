using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mini_Oyun.Oyun_Motoru;

namespace Mini_Oyun
{
    public enum Nadirlik
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic,
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("------- Mini Rpg Oyununa Hoş Geldiniz -------");

            Console.WriteLine("Karakter Adınızı Girin: ");
            string karakterAdi = Console.ReadLine();

            Karakter oyuncu = new Karakter(karakterAdi, 100, 50);

            oyuncu.Envanter.Add(new Oge("Acemi Kılıcı", Nadirlik.Common, OgeTuru.Silah, 5));
            oyuncu.Envanter.Add(new Oge("Küçük Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 20));

            OyunMotoru oyun = new OyunMotoru(oyuncu);

            oyun.OyunuBaslat();

            Console.WriteLine("Oyun kapatıldı. Tekrar görüşmek üzere!");

            Console.ReadKey();
        }
    }
}
