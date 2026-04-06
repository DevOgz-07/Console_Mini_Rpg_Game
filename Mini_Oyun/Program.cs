using System;
using System.Collections.Generic;
using System.IO;        
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
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
            Console.OutputEncoding = System.Text.Encoding.UTF8; // logoların gözükmesini sağlar.

            Oyun_Motoru motor = new Oyun_Motoru();
            Karakter oyuncu = null;

            
            Console.Title = "MİNİ RPG: KARANLIK DÜNYA";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("MİNİ RPG: KARANLIK DÜNYA'YA HOŞ GELDİNİZ!");
            Thread.Sleep(2000);

            DosyaTaramaEfekti();

            Console.ResetColor(); 
           

            while (oyuncu == null)
            {
                Console.Clear();
                
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("===============================================");
                Console.WriteLine("        🛡️  ANA MENÜ - HOŞ GELDİNİZ  🛡️        ");
                Console.WriteLine("===============================================");
                Console.ResetColor();

                Console.WriteLine("\n[1] 📥 Kayıtlı Hesaba Giriş Yap");
                Console.WriteLine("[2] 📝 Yeni Hesap Oluştur");
                Console.WriteLine("[3] 👤 Misafir Modu (Kaydedilmez)");
                Console.WriteLine("[0] ❌ Dünyadan Ayrıl");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n-----------------------------------------------");
                Console.ResetColor();
                Console.Write("  Seçiminiz: ");

                string secim = Console.ReadLine();

                if (secim == "0")
                {
                    Console.Clear();
                    Console.ForegroundColor= ConsoleColor.Yellow;
                    Console.WriteLine($"Dünyadan Ayrılıyor Bir Daha ki Sefere Görüşmek Üzere Maceracı");
                    Console.ResetColor();
                    return;
                }

                if (secim == "1") 
                {
                    Console.Write("Karakter Adınızı Girin: ");
                    string ad = Console.ReadLine();

                    Karakter yuklenen = motor.OyunuYukle(ad);

                    if (yuklenen != null)
                    {
                        Console.Write("Şifrenizi Girin: ");
                        string girilenSifre = motor.SifreOku();

                        if (yuklenen.Sifre == girilenSifre)
                        {
                            oyuncu = yuklenen;
                            Console.WriteLine($"\nHoş geldin {ad}! Giriş başarılı.");
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            Console.WriteLine("\n[HATA]: Şifre yanlış! Tekrar deneyin.");
                            Console.ReadKey();
                            continue; 
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n[HATA]: Bu isimde bir kayıt bulunamadı!");
                        Console.ReadKey();
                        continue; 
                    }
                }
                else if (secim == "2") 
                {
                    Console.Write("Yeni Karakter Adı: ");
                    string ad = Console.ReadLine();

                    
                    Console.Write("Yeni Şifre Belirleyin: ");
                    string girilenSifre = motor.SifreOku();

                    oyuncu = new Karakter(ad);
                    oyuncu.Sifre = girilenSifre;
                    oyuncu.Envanter.Add(new Oge("Acemi Kılıcı", Nadirlik.Common, OgeTuru.Silah, 5));
                    oyuncu.Envanter.Add(new Oge("Küçük Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 20));

                    motor.OyunuKaydet(oyuncu);
                    Console.WriteLine("\nKayıt oluşturuldu ve giriş yapıldı!");
                    Thread.Sleep(2000);
                }
                else if (secim == "3") // MİSAFİR
                {
                    oyuncu = new Karakter("Misafir_" + new Random().Next(100, 999));
                }
                else
                {
                    Console.WriteLine("\nGeçersiz seçim! Lütfen 0-3 arası bir rakam girin.");
                    Console.ReadKey();
                }
            }

            
            if (oyuncu != null)
            {
                motor.SetOyuncu(oyuncu);
                motor.OyunuBaslat();
            }
        }
           static void DosyaTaramaEfekti()
           {
             Console.ForegroundColor = ConsoleColor.Cyan;
              string[] dosyalar = { "Haritalar", "Canavarlar", "Ganimetler", "Karakter_Verileri", "Ses_Efektleri" };
              Random rnd = new Random();

            foreach (var dosya in dosyalar)
            {
                
                Console.Write($"\r  > {dosya} yükleniyor...           ");
                Thread.Sleep(rnd.Next(300, 700));
            }
            Console.WriteLine("\n  [!] Tüm sistemler hazır! Giriş yapılıyor...");
             Thread.Sleep(1000);
           }
    }
}

