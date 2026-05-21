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

            // =================================================================
            // 🔥 AŞAMA 0:İŞGAL EDİLEN KUTSAL TOPRAKLARIN TRAİLERI
            // =================================================================
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            EkranaYazdir("--- KUTSAL IŞIĞIN TUTULMASI 1.KISIM ---", 50, 2000);
            Console.ResetColor();

            string[] trailerMetni = {
                "Bu dünya henüz zifiri karanlığa gömülmeden önce, gök kubbede üç kutsal mimar parıldardı...",
                "Vaelor, Zul'Khaar ve Nyxaris... Dünyayı şefkatle saran üç tanrısal Ay.",
                "Onlar hayatı, ruhu ve zamanı var eden kadim yaratıcılardı.",
                "Fakat... Derin boşluktan gelen Ana Karanlık, bu kutsal diyara göz dikti.",
                "Ayları gökyüzüne zincirleyen, ışıklarını emen o dehşet verici hamlesini yaptı:",
                "Yeryüzünün kalbine saplanan 'Karanlığın 4 Büyük Sütun Gölgesi'...",
                "Bu devasa çiviler dünyayı ele geçirdi, etrafı canavarlarla doldurdu ve kutsal olan her şeyi yuttu.",
                "Şimdilik bu sütunların ne olduğunu bilmiyorsun, isimleri fısıldanamaz birer [BİLİNMEYEN GÖLGE SÜTUNU]...",
                "Ve o sütunlardan yükselen karanlık çekim, senin yıldızlar arası gemini bir tüy gibi yeryüzüne fırlattı!",
                "Tanrılar esir, dünya işgal altında... Ve sen, bu karanlığı yırtacak son ışık kırıntısısın."
            };

            foreach (string line in trailerMetni)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                EkranaYazdir(line, 25, 1800);
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            EkranaYazdir("\n...Kutsal topraklar üzerinde uyanış başlıyor...", 60, 2500);
            Console.ResetColor();
            Console.Clear();

            // =================================================================
            // 🚀 AŞAMA 1: PROLOG (GÖKTEN DÜŞÜŞ)
            // =================================================================
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            EkranaYazdir("--- GÖKTEN DÜŞÜŞ 2.KISIM ---", 50, 2000);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;

            string[] prolog = {
                "Yıldızların arasından sessizce süzülen gemin, ansızın karanlık bir çekimle sarsıldı...",
                "İşgal altındaki atmosfer seni bir canavar gibi yutarken sistemler birer birer çöktü.",
                "Dumanlar tüten enkazdan dışarı adım attığında, gökyüzünün olmadığını fark ettin.",
                "Sadece zifiri bir karanlık ve bu karanlığın ortasında, can çekişen 3 Tanrı Ay'ın soluk silüeti..."
            };

            foreach (string satir in prolog)
            {
                EkranaYazdir(satir, 30, 1500);
            }

            // =================================================================
            // 🛡️ AŞAMA 2: GEÇMİŞİN SEÇİMİ
            // =================================================================
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
                Console.WriteLine("[1] Soylu Bir Tüccar Varisi   (+15 HP, +2 STR, +2 DEX, +100 Altın)");
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
            Console.Clear();
            // =================================================================
            // ⚔️ AŞAMA 3: UMUTSUZ SAVAŞ (BİLİNMEYEN SÜTUN MUHAFIZI)
            // =================================================================
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            EkranaYazdir("--- UMUTSUZ SAVAŞ 3.KISIM ---", 50, 2000);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[!] BİR İŞGALCİ DEHŞET BELİRDİ: [BİLİNMEYEN GÖLGE SÜTUNU MUHAFIZI]!");
            Console.ResetColor();
            Thread.Sleep(3000);

            Console.WriteLine($"\n{oyuncu.Ad} kılıcını çekti ama bu mutlak karanlık karşısında elleri titriyor...");
            Thread.Sleep(2000);
            Console.WriteLine($"{oyuncu.Ad} saldırıyor! Hasar: {oyuncu.SaldiriGucu}");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\nMUHAFIZ: 'Ayların yarattığı zavallı mahluk... Gökyüzünüz düştü. Bu sütunun gölgesi her şeyi yutacak!'");
            Console.ResetColor();
            Thread.Sleep(4000);

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\nMUHAFIZ ANİ BİR KARANLIK DARBE İNDİRİYOR!");
            Console.WriteLine("ALINAN HASAR: 99999! (RUHUNUN EZİLİŞİ)");
            Console.ResetColor();
            Thread.Sleep(4000);
            Console.Clear();

           // =================================================================
            // 👁️ AŞAMA 4: GİZEMLİ BULUŞMA (AFTARLIFE / BİLİNÇALTI DİYALOĞU)
            // =================================================================
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            EkranaYazdir("--- GİZEMLİ BULUŞMA 4.KISIM  ---", 50, 2000);
            Console.ResetColor();
            Thread.Sleep(2000);

           
            Console.ForegroundColor = ConsoleColor.White;

            string[] gizemliKizDiyalog = {
                "Ruhunun derinliklerinde, varoluşun ve mutlak hiçliğin o bıçak sırtı sınırındasın...",
                "Ana Karanlık etini ve anılarını bir asit gibi eritirken, zihninin çöplüğünde saf, bozulmamış gümüş bir ışık parıldıyor.",
                "Zifiri karanlığın, o yoğun ve kıpırdayan mimarisinin içinden soluk bir silüet süzülüyor.",
                "Gözleri, kıyameti yaşamış ve sönmekte olan kadim yıldızların koruna benziyor... Sana doğru eğiliyor.",
                "Ölüm kadar soğuk, fakat bir ana kadar şefkatli ellerini yanaklarına koyuyor; sesi kulaklarına değil, doğrudan varlığının özüne yankılanıyor:",

                "\n[Gizemli Kız]: 'Demek sonsuz boşluğun bağrından düşen, kaderin o son talihsiz kırıntısı sensin...'",

                "[Gizemli Kız]: 'Korkma... Etin dövüldü, iraden kırıldı ama özün hala burada. Kirletilemeyen o kutsal çekirdek... Üç Ay'ın kadim çarkı henüz tamamen durmadı, yabancı.'",

                "[Gizemli Kız]: 'Bu lanetli diyar; gök kubbeden vahşetle sökülen, gümüş gözleri kör edilen o üç yüce mimarın, Vaelor, Zul'Khaar ve Nyxaris'in sessiz çığlıklarıyla can çekişiyor.'",

                "[Gizemli Kız]: 'Aşağı fırlatılan o Dört Devasa Sütun yeryüzünün damarlarını zehirlemiş, etrafı etten canavarlarla mühürlemiş olabilir... Evet, Ana Karanlık tanrılarımızı göklere zincirledi...'",

                "[Gizemli Kız]: 'Fakat zihnini aç ve beni iyi dinle: En koyu, en mutlak gölge bile, hemen arkasında bükülmeden duran o saklı, kadim ışık yüzünden form kazanır. Işık olmasa, karanlık kendi ismini bile fısıldayamazdı.'",

                "[Gizemli Kız]: 'Senin bu topraklara düşüşün bir kaza değil, kozmik bir kusurdur. Kaderin; bu karanlığı bir sünger gibi emmek, bu sahte gerçekliği içeriden yırtmak ve kalbimizde mühürlenmiş o saf kutsal ışığı yeniden hür bırakmak.'",

                "[Gizemli Kız]: 'Şimdi uyan... Gümüşışık seni çağırıyor. Gölgelerin arkasında, gözlerim üzerinde olacak...'"
            };

            foreach (string satir in gizemliKizDiyalog)
            {
                
                int hiz = satir.StartsWith("[Gizemli Kız]") ? 45 : 25;
                EkranaYazdir(satir, hiz, 2000);
            }

            Console.ForegroundColor = ConsoleColor.White;
            EkranaYazdir("\nKızın silüeti gümüş bir toza dönüşerek dağılırken, kalbinin yeniden çarptığını hissediyorsun...", 30, 3000);
            Console.ResetColor();
            Console.Clear();

            // =================================================================
            // 🌟 AŞAMA 5: UYANIŞ
            // =================================================================
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            EkranaYazdir("--- UYANIŞ 5.KISIM ---", 50, 2000);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;

            string[] uyanisMetni = {
                "Gözlerini araladığında, yüzüne vuran o keskin ve kirli rüzgârı hissediyorsun...",
                "Gümüşışık Şehri'nin kadim surları, işgale direnen son kale gibi ufukta yükseliyor.",
                "O kadim Sütun Muhafızına indirdiğin darbe, dünyayı esir alan karanlığın yanında bir hiçti.",
                "Gemin yok oldu, teknolojin bitti... Ama ruhunda, esir düşmüş Üç Ay'ın kutsal kıvılcımı hala canlı.",
                "Yukarıda solan Vaelor, Zul'Khaar ve Nyxaris kurtarılmayı bekliyor...",
                $"Seçilmiş kurban ya da kurtarıcı... Kaderin mühürlendi, {oyuncu.Ad}. Gümüşışık kapıları açılıyor..."
            };

            foreach (string satir in uyanisMetni)
            {
                EkranaYazdir(satir, 30, 1500);
            }
            Console.ResetColor();
        }

        private static void EkranaYazdir(string metin, int harfHizi, int satirBeklemeSuresi)
        {
            foreach (char c in metin)
            {
                Console.Write(c);
                Thread.Sleep(harfHizi);
            }
            Console.WriteLine();
            Thread.Sleep(satirBeklemeSuresi);
        }
    }
}



