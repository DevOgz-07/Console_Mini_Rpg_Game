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
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Oyun_Motoru motor = new Oyun_Motoru();
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            //bool guncellemeMevcut = await GuncellemeSistemi.YeniGuncellemeVarMi();

            Console.Title = "MİNİ RPG: KARANLIK DÜNYA";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("MİNİ RPG: KARANLIK DÜNYA'YA HOŞ GELDİNİZ!");
            Thread.Sleep(2000);
            DosyaTaramaEfekti();
            Console.ResetColor();


            while (true)
            {
                Karakter oyuncu = null;

                // KARAKTER SEÇİM/GİRİŞ DÖNGÜSÜ
                while (oyuncu == null)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    //Console.WriteLine($"[ 📦 Oyun Sürümü: {GuncellemeSistemi.MevcutVersiyon} ]");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("===============================================");
                    Console.WriteLine("        🛡️  ANA MENÜ - HOŞ GELDİNİZ  🛡️        ");
                    Console.WriteLine("===============================================");
                    Console.ResetColor();

                    Console.WriteLine("\n[1] 📥 Kayıtlı Hesaba Giriş Yap");
                    Console.WriteLine("[2] 📝 Yeni Hesap Oluştur");
                    Console.WriteLine("[3] 👤 Misafir Modu (Kaydedilmez)");
                    //if (guncellemeMevcut)
                    //{
                    //    Console.ForegroundColor = ConsoleColor.Green;
                    //    Console.WriteLine("[4] 🚀 YENİ GÜNCELLEME MEVCUT! (Hemen Yükle)");
                    //    Console.ResetColor();
                    //}
                    Console.WriteLine("[0] ❌ Uygulamadan Tamamen Çık");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n-----------------------------------------------");
                    Console.ResetColor();
                    Console.Write("  Seçiminiz: ");

                    string secim = Console.ReadLine();

                    if (secim == "0")
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Dünyadan Ayrılıyor... Bir dahaki sefere görüşmek üzere maceracı!");
                        Console.ResetColor();
                        Thread.Sleep(1500);
                        return; // Tüm programı kapatır.
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
                                Thread.Sleep(1500);
                            }
                            else
                            {
                                Console.WriteLine("\n[HATA]: Şifre yanlış!");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n[HATA]: Kayıt bulunamadı!");
                            Console.ReadKey();
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

                        HikayeliKarakterOlusturma(oyuncu);

                        oyuncu.Envanter.Add(new Oge("Acemi Kılıcı", Nadirlik.Common, OgeTuru.Silah, 5));
                        oyuncu.Envanter.Add(new Oge("Küçük Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 20));

                        OyunuKaydet(oyuncu);
                        Console.WriteLine("\nKayıt oluşturuldu!");
                        Thread.Sleep(1500);
                    }
                    else if (secim == "3")
                    {
                        Random rnd = new Random();
                        Karakter misafir = new Karakter("Misafir_" + rnd.Next(100, 999));
                        HikayeliKarakterOlusturma(misafir);
                        oyuncu = misafir;
                        Console.WriteLine("\n[!] Misafir girişi yapıldı.");
                        Thread.Sleep(1500);
                    }
                    //else if (secim == "4" && guncellemeMevcut)
                    //{
                    //    Console.Clear();
                    //    Console.ForegroundColor = ConsoleColor.Green;
                    //    Console.WriteLine("\n[!] Güncelleme paketi GitHub üzerinden çekiliyor...");

                    //    await GuncellemeSistemi.GuncellemeBaslat();

                    //    return;
                    //}
                    else
                    {
                        Console.WriteLine("\nGeçersiz seçim!");
                        Console.ReadKey();
                    }
                }


                if (oyuncu != null)
                {
                    motor.SetOyuncu(oyuncu);
                    motor.OyunuBaslat();

                }
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
        } // Başlangıç için tarama efekti

        public static void HikayeliKarakterOlusturma(Karakter oyuncu)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;

            // --- AŞAMA 1: PROLOG (GÖKTEN DÜŞÜŞ) ---
            string[] prolog = {
              "Yıldızların arasından sessizce süzülen gemin, ansızın karanlık bir çekimle sarsıldı...",
              "Karanlık Dünya'da atmosfer seni bir canavar gibi yutarken sistemler birer birer çöktü.",
              "Dumanlar tüten enkazdan dışarı adım attığında, gökyüzünün olmadığını fark ettin.",
              "Sadece zifiri bir karanlık ve bu karanlığın ortasında parlayan devasa, çarpık bir silüet..."
    };

            foreach (string satir in prolog)
            {
                foreach (char c in satir) { Console.Write(c); Thread.Sleep(30); }
                Console.WriteLine();
                Thread.Sleep(1500);
            }

            // --- AŞAMA 2: GEÇMİŞİN SEÇİMİ ---
            bool secimYapildi = false;
            while (!secimYapildi)
            {
                Console.WriteLine("\nZihnindeki pus dağılırken, bu dünyaya düşmeden önce kim olduğunu hatırlıyorsun...");
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" --- GEÇMİŞİNİN GÖLGESİNE KARAR VER --- ");
                Console.ResetColor();
                Console.WriteLine("\n");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[1] Soylu Bir Tüccar Varisi  (+15 HP, +2 STR, +2 DEX, +100 Altın)");
                Console.WriteLine("[2] Sokakların Dövüşçüsü      (+5 HP, +5 STR, +3 DEX)");
                Console.WriteLine("[3] Orman Avcısı              (+5 HP, +3 STR, +7 DEX)");
                Console.WriteLine("[4] Eski Kule Muhafızı        (+10 HP, +1 STR, +10 DEX)");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\nGeçmişini Seç (1-4): ");
                string secim = Console.ReadLine()?.Trim();
                Console.ResetColor();

                switch (secim)
                {
                    case "1":
                        oyuncu.HP_Stat += 15; oyuncu.STR_Stat += 2; oyuncu.DEX_Stat += 2; oyuncu.Altın += 100;
                        secimYapildi = true; break;
                    case "2":
                        oyuncu.HP_Stat += 5; oyuncu.STR_Stat += 5; oyuncu.DEX_Stat += 3;
                        secimYapildi = true; break;
                    case "3":
                        oyuncu.HP_Stat += 5; oyuncu.STR_Stat += 3; oyuncu.DEX_Stat += 7;
                        secimYapildi = true; break;
                    case "4":
                        oyuncu.HP_Stat += 10; oyuncu.STR_Stat += 1; oyuncu.DEX_Stat += 10;
                        secimYapildi = true; break;
                    default:
                        Console.WriteLine("\n[!] Geçersiz seçim!");
                        Thread.Sleep(1000);
                        Console.Clear();
                        break;
                }

                if (secimYapildi)
                {
                    
                    oyuncu.MaksimumCan = 100 + (oyuncu.HP_Stat * 5);
                    oyuncu.SaldiriGucu = 25 + (oyuncu.STR_Stat * 2);
                    oyuncu.Savunma = 1 + (oyuncu.DEX_Stat * 1);
                    oyuncu.Can = oyuncu.MaksimumCan;
                }
            }

            // --- AŞAMA 3: UMUTSUZ SAVAŞ (SÜTUN MUHAFIZI) ---
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[!] BİR GÖLGE DEHŞETİ BELİRDİ: KARANLIK SÜTUNUN MUHAFIZI!");
            Console.ResetColor();
            Thread.Sleep(5000);
            Console.WriteLine($"\n{oyuncu.Ad} kılıcını çekti ama elleri titriyor...");
            Thread.Sleep(5000);
            Console.WriteLine($"{oyuncu.Ad} saldırıyor! Hasar: {oyuncu.SaldiriGucu}");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("MUHAFIZ SÜTUN: 'Bu dünyaya ait olmayan ışık... Burada sadece sönmek için varsın!'");
            Console.ResetColor();
            Thread.Sleep(5000);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\nMUHAFIZ KARANLIK BİR DARBE İNDİRİYOR!");
            Console.WriteLine("ALINAN HASAR: 99999!");
            Console.ResetColor();
            Thread.Sleep(5000);

            // --- AŞAMA 4: UYANIŞ ---
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            string[] uyanisMetni = {
               "Gözlerini araladığında, yüzüne vuran keskin ve soğuk rüzgârı hissediyorsun...",
               "Gümüşışık Şehri'nin kadim surları, ufukta görkemli birer gölge gibi yükseliyor.",
               "Saldırmayı denedin ama kılıcın o varlığın gövdesinde bir tüy gibi sekti.",
               "Gemin parçalandı, teknolojin yok oldu... Sadece özündeki yeteneklerin kaldı.",
               "Etrafında, henüz anlamlandıramadığın kadim bir çarkın döndüğünü seziyorsun...",
              $"Kaderin mühürlendi, {oyuncu.Ad}. Gümüşışık kapıları açılıyor..."
    };

            foreach (string satir in uyanisMetni)
            {
                foreach (char c in satir) { Console.Write(c); Thread.Sleep(30); }
                Console.WriteLine();
                Thread.Sleep(1500);
            }
            Console.ResetColor();
        }
    }
}


