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
        Tuketilebilir, 
    }
    public class Oge
    {
        public string Ad { get; set; }
        public Nadirlik Nadirlik { get; set; }
        public OgeTuru Tur { get; set; }
        public int MaksimumStack { get; set; } = 200;
        public int Miktar { get; set; }



        public int EtkiDegeri { get; set; }

        // NEW
        public int MaxEtkiDegeri { get; set; } 
        public int GerekenSeviye { get; set; }  
        public int ArtiSeviyesi { get; set; }   

        public string TamAd => ArtiSeviyesi > 0 ? $"{Ad} +{ArtiSeviyesi}" : Ad;


        public Oge(string ad, Nadirlik nadirlik, OgeTuru tur, int etkiDegeri = 0, int maxEtkiDegeri = 0, int gerekenSeviye = 1,
        int miktar = 1)
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




        public void BilgileriGoster()
        {
            Console.ForegroundColor = NadirlikRengiGetir(this.Nadirlik);
            Console.Write($"- {TamAd} [{Nadirlik}]");
            Console.ResetColor();

            if (EtkiDegeri > 0)
            {
                string birim = Tur == OgeTuru.Silah ? "Hasar" : (Tur == OgeTuru.Zirh ? "Savunma" : "Can");
                string deger = (Tur == OgeTuru.Silah && MaxEtkiDegeri > EtkiDegeri)
                               ? $"{EtkiDegeri}-{MaxEtkiDegeri}"
                               : $"{EtkiDegeri}";

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($" (+{deger} {birim})");

                if (GerekenSeviye > 1) Console.Write($" [Lvl: {GerekenSeviye}]");
                Console.WriteLine();
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
        public void DetaySayfasiGoster(Karakter oyuncu)
        {
            Console.Clear();
            ConsoleColor nadirlikRengi = NadirlikRengiGetir(this.Nadirlik);

            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("╔═══════════════════════════════════════════╗");

            
            Console.Write("║");
            Console.ForegroundColor = nadirlikRengi;
            string miktarEki = Miktar > 1 ? $" (x{Miktar})" : "";
            string isimSatiri = $"{Ad.ToUpper()}{miktarEki}";
            Console.Write(isimSatiri.PadLeft(21 + isimSatiri.Length / 2).PadRight(43));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("║");

            
            Console.WriteLine("╠══════════════════╤════════════════════════╣");

            
            YazdirDetaySatiri("TÜR", Tur.ToString(), ConsoleColor.White);
            YazdirDetaySatiri("NADİRLİK", Nadirlik.ToString(), nadirlikRengi);

            
            string istatistikBaslik = Tur == OgeTuru.Silah ? "SALDIRI" : (Tur == OgeTuru.Zirh ? "SAVUNMA" : "İYİLEŞTİRME");
            string degerMetni = (Tur == OgeTuru.Silah) ? $"{EtkiDegeri} - {MaxEtkiDegeri}" : $"+{EtkiDegeri} HP";
            YazdirDetaySatiri(istatistikBaslik, degerMetni, ConsoleColor.Cyan);

            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("║ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("GEREKEN LVL".PadRight(17));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("│ ");

            if (oyuncu.Seviye < GerekenSeviye) Console.ForegroundColor = ConsoleColor.Red;
            else Console.ForegroundColor = ConsoleColor.Green;

            Console.Write($"{GerekenSeviye}".PadRight(23));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("║");

            
            if (Tur == OgeTuru.Silah || Tur == OgeTuru.Zirh)
            {
                YazdirDetaySatiri("YÜKSELTME", "+" + ArtiSeviyesi, ConsoleColor.Yellow);
            }

            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("╚══════════════════╧════════════════════════╝");

            
            Console.ResetColor();
            string aksiyonMetni = Tur == OgeTuru.Tuketilebilir ? "[1] İksiri İç" : "[1] Eşyayı Kuşan";

            Console.WriteLine($"\n  {aksiyonMetni}    [0] Geri Dön");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  ───────────────────────────────────────────");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  Seçimin: ");
        }

        private void YazdirDetaySatiri(string baslik, string deger, ConsoleColor degerRengi)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("║ "); 

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(baslik.PadRight(17)); 

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("│ "); 

            Console.ForegroundColor = degerRengi;
            Console.Write(deger.PadRight(23)); 

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("║"); 
        }
        public int SatisFiyatiniHesapla()
        {
            int temelFiyat = 0;

            // 1. ADIM: Türlerine göre temel taban fiyat belirleyelim
            switch (Tur)
            {
                case OgeTuru.Silah:
                    temelFiyat = 50;
                    break;
                case OgeTuru.Zirh:
                    temelFiyat = 40;
                    break;
                case OgeTuru.Tuketilebilir: // İksirler vb.
                    temelFiyat = 10;
                    break;
                default:
                    temelFiyat = 20;
                    break;
            }

            int nadirlikCarpani;
            switch (this.Nadirlik)
            {
                case Nadirlik.Common:
                    nadirlikCarpani = 1;
                    break;
                case Nadirlik.Rare:
                    nadirlikCarpani = 3;
                    break;
                case Nadirlik.Epic:
                    nadirlikCarpani = 8;
                    break;
                case Nadirlik.Legendary:
                    nadirlikCarpani = 20;
                    break;
                default:
                    nadirlikCarpani = 1;
                    break;
            }

            double artiBonusu = 1.0 + (ArtiSeviyesi * 0.2);

            int sonuc = (int)(temelFiyat * nadirlikCarpani * artiBonusu);

            if (Tur == OgeTuru.Tuketilebilir && Miktar > 1)
            {
                return sonuc * Miktar;
            }

            return sonuc;
        }

    }
}
