using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Oyun
{
    public interface IItem
    {
        string Ad { get; }
        Nadirlik Nadirlik { get; }
        OgeTuru Tur { get; }
        int SatisFiyatiniHesapla();
    }
    public enum OgeTuru { Silah, Zirh, Tuketilebilir }
    public class Oge : IItem
    {
        public string Ad { get; set; }
        public Nadirlik Nadirlik { get; set; }
        public OgeTuru Tur { get; set; }
        public int MaksimumStack { get; set; }
        public int Miktar { get; set; }
        public int EtkiDegeri { get; set; }
        public int MaxEtkiDegeri { get; set; }
        public int GerekenSeviye { get; set; }
        public int ArtiSeviyesi { get; set; }

        public string TamAd => ArtiSeviyesi > 0 ? $"{Ad} +{ArtiSeviyesi}" : Ad;

        public Oge(string ad, Nadirlik nadirlik, OgeTuru tur, int etkiDegeri = 0,
                   int maxEtkiDegeri = 0, int gerekenSeviye = 1, int miktar = 1)
        {
            Ad = ad;
            Nadirlik = nadirlik;
            Tur = tur;
            EtkiDegeri = etkiDegeri;
            Miktar = miktar;
            MaksimumStack = (tur == OgeTuru.Tuketilebilir) ? 200 : 1;
            MaxEtkiDegeri = maxEtkiDegeri == 0 ? etkiDegeri : maxEtkiDegeri;
            GerekenSeviye = gerekenSeviye;
            ArtiSeviyesi = 0;
        }
        public int SatisFiyatiniHesapla() => OgePriceCalculator.FiyatHesapla(this);
        public void BilgileriGoster() => OgeUIHelper.BasitBilgiYazdir(this);
        public void DetaySayfasiGoster(Karakter oyuncu) => OgeUIHelper.DetayPenceresiCiz(this, oyuncu);
    }

    
    public static class OgePriceCalculator
    {
        public static int FiyatHesapla(Oge oge)
        {
            int temelFiyat;
            switch (oge.Tur)
            {
                case OgeTuru.Silah:
                    temelFiyat = 50;
                    break;
                case OgeTuru.Zirh:
                    temelFiyat = 40;
                    break;
                case OgeTuru.Tuketilebilir:
                    temelFiyat = 10;
                    break;
                default:
                    temelFiyat = 20;
                    break;
            }

            int nadirlikCarpani;
            switch (oge.Nadirlik)
            {
                case Nadirlik.Common:
                    nadirlikCarpani = 1;
                    break;
                case Nadirlik.Rare:
                    nadirlikCarpani = 2;
                    break;
                case Nadirlik.Epic:
                    nadirlikCarpani = 3;
                    break;
                case Nadirlik.Legendary:
                    nadirlikCarpani = 5;
                    break;
                case Nadirlik.Mythic:
                    nadirlikCarpani = 8;
                    break;
                default:
                    nadirlikCarpani = 1;
                    break;
            }

            double artiBonusu = 1.0 + (oge.ArtiSeviyesi * 0.2);
            int birimFiyat = (int)(temelFiyat * nadirlikCarpani * artiBonusu);
            if (oge.Tur == OgeTuru.Tuketilebilir && oge.Miktar > 1)
            {
                return birimFiyat * oge.Miktar;
            }

            return birimFiyat;
        }

    }
    public static class OgeUIHelper
    {
        public static void BasitBilgiYazdir(Oge oge)
        {
            Console.ForegroundColor = NadirlikRengiGetir(oge.Nadirlik);
            Console.Write($"- {oge.TamAd} [{oge.Nadirlik}]");
            Console.ResetColor();

            if (oge.EtkiDegeri > 0)
            {
                string birim = oge.Tur == OgeTuru.Silah ? "Hasar" : (oge.Tur == OgeTuru.Zirh ? "Savunma" : "Can");
                string deger = (oge.Tur == OgeTuru.Silah && oge.MaxEtkiDegeri > oge.EtkiDegeri)
                    ? $"{oge.EtkiDegeri}-{oge.MaxEtkiDegeri}"
                    : $"{oge.EtkiDegeri}";

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($" (+{deger} {birim})");
                if (oge.GerekenSeviye > 1) Console.Write($" [Lvl: {oge.GerekenSeviye}]");
                Console.WriteLine();
            }
            else Console.WriteLine();
            Console.ResetColor();
        }

        public static void DetayPenceresiCiz(Oge oge, Karakter oyuncu)
        {
            Console.Clear();
            ConsoleColor nadirlikRengi = NadirlikRengiGetir(oge.Nadirlik);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("╔═══════════════════════════════════════════╗");
            Console.Write("║");
            Console.ForegroundColor = nadirlikRengi;
            string miktarEki = oge.Miktar > 1 ? $" (x{oge.Miktar})" : "";
            string isimSatiri = $"{oge.Ad.ToUpper()}{miktarEki}";
            Console.Write(isimSatiri.PadLeft(21 + isimSatiri.Length / 2).PadRight(43));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("║");
            Console.WriteLine("╠══════════════════╤════════════════════════╣");

            YazdirSatir("TÜR", oge.Tur.ToString(), ConsoleColor.White);
            YazdirSatir("NADİRLİK", oge.Nadirlik.ToString(), nadirlikRengi);

            string istatistikBaslik = oge.Tur == OgeTuru.Silah ? "SALDIRI" : (oge.Tur == OgeTuru.Zirh ? "SAVUNMA" : "İYİLEŞTİRME");
            string degerMetni = (oge.Tur == OgeTuru.Silah) ? $"{oge.EtkiDegeri} - {oge.MaxEtkiDegeri}" : $"+{oge.EtkiDegeri} HP";
            YazdirSatir(istatistikBaslik, degerMetni, ConsoleColor.Cyan);

            // Seviye Kontrol Satırı
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("║ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("GEREKEN LVL".PadRight(17));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("│ ");
            Console.ForegroundColor = (oyuncu.Seviye < oge.GerekenSeviye) ? ConsoleColor.Red : ConsoleColor.Green;
            Console.Write($"{oge.GerekenSeviye}".PadRight(23));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("║");

            if (oge.Tur == OgeTuru.Silah || oge.Tur == OgeTuru.Zirh)
                YazdirSatir("YÜKSELTME", "+" + oge.ArtiSeviyesi, ConsoleColor.Yellow);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("╚══════════════════╧════════════════════════╝");
            Console.ResetColor();

            string aksiyon = oge.Tur == OgeTuru.Tuketilebilir ? "[1] İksiri İç" : "[1] Eşyayı Kuşan";
            Console.WriteLine($"\n  {aksiyon}    [0] Geri Dön");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  ───────────────────────────────────────────");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  Seçimin: ");
        }

        private static void YazdirSatir(string baslik, string deger, ConsoleColor renk)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("║ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(baslik.PadRight(17));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("│ ");
            Console.ForegroundColor = renk;
            Console.Write(deger.PadRight(23));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("║");
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