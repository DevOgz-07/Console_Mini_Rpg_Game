using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json; 
using System.IO;        


namespace Mini_Oyun
{
    internal class Oyun_Motoru
    {
        private Karakter oyuncu;
        private List<Canavar> canavarHavuzu;
        private List<Boss> bossHavuzu;
        private Random random = new Random();

        public void SetOyuncu(Karakter k) { this.oyuncu = k; }

        
        public Oyun_Motoru()
        {
            canavarHavuzu = new List<Canavar>();
            bossHavuzu = new List<Boss>();
            CanavarlariOlustur();
            BosslariOlustur();
        }

        public Oyun_Motoru(Karakter karakter) : this() 
        {
            oyuncu = karakter;
        }

            public string SifreOku()
        {
            string sifre = "";
            ConsoleKeyInfo tus;
            do
            {
                tus = Console.ReadKey(true);
                if (tus.Key != ConsoleKey.Backspace && tus.Key != ConsoleKey.Enter)
                {
                    sifre += tus.KeyChar;
                    Console.Write("*");
                }
                else if (tus.Key == ConsoleKey.Backspace && sifre.Length > 0)
                {
                    sifre = sifre.Substring(0, (sifre.Length - 1));
                    Console.Write("\b \b");
                }
            } while (tus.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return sifre;
        }

            public void OyunuKaydet(Karakter k)
            {
            if (k == null) return;
            string dosyaAdi = $"{k.Ad}_kayit.json";
            var secenekler = new JsonSerializerOptions { WriteIndented = true };
            string jsonVerisi = JsonSerializer.Serialize(k, secenekler);
            File.WriteAllText(dosyaAdi, jsonVerisi);
            
            }

            public Karakter OyunuYukle(string ad)
        {
            string dosyaAdi = $"{ad}_kayit.json";
            if (File.Exists(dosyaAdi))
            {
                string jsonVerisi = File.ReadAllText(dosyaAdi);
                return JsonSerializer.Deserialize<Karakter>(jsonVerisi);
            }
            return null;
        }

            private void CanavarlariOlustur()
            {
                canavarHavuzu.AddRange(CanavarVeritabani.TumCommonCanavarlar);
                canavarHavuzu.AddRange(CanavarVeritabani.TumRareCanavarlar);
                canavarHavuzu.AddRange(CanavarVeritabani.TumEpicCanavarlar);
                canavarHavuzu.AddRange(CanavarVeritabani.TumLegendaryCanavarlar);
                canavarHavuzu.AddRange(CanavarVeritabani.TumMythicCanavarlar);
                

            }

            private void BosslariOlustur()
            {
                canavarHavuzu.AddRange(CanavarVeritabani.TumBossCanavarlar);
            }

            public void OyunuBaslat()
            {
           
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n  >>> {oyuncu.Ad.ToUpper()} İÇİN KADER ANI BAŞLIYOR... <<<");
                Console.ResetColor();
                Thread.Sleep(2000);

               while (true) 
               {
                 Console.Clear();

                 // --- ÜST PANEL (DURUM ÇUBUĞU) ---
                 Console.ForegroundColor = ConsoleColor.DarkCyan;
                 Console.WriteLine("===============================================================");
                 Console.ResetColor();

                 Console.Write($"  👤 {oyuncu.Ad.PadRight(12)} ");

                 
                 if (oyuncu.Can < (oyuncu.MaksimumCan * 0.3)) Console.ForegroundColor = ConsoleColor.Red;
                 else Console.ForegroundColor = ConsoleColor.Green;
                 Console.Write($"❤️ HP: {oyuncu.Can}/{oyuncu.MaksimumCan}   ");

                 Console.ForegroundColor = ConsoleColor.Yellow;
                 Console.Write($"💰 Altın: {oyuncu.Altın}   ");

                 Console.ForegroundColor = ConsoleColor.Magenta;
                 Console.WriteLine($"⭐ Lvl: {oyuncu.Seviye}");

                 Console.ForegroundColor = ConsoleColor.DarkCyan;
                 Console.WriteLine("===============================================================");
                 Console.ResetColor();

                 // --- ANA MENÜ SEÇENEKLERİ ---
                 Console.WriteLine("\n[1] ⚔️  DÜNYA HARİTASI VE BÖLGELER");
                 Console.WriteLine("[2] 🏰  GÜMÜŞIŞIK ŞEHRİ (Dinlen & Market)");
                 Console.WriteLine("[3] 🎒  ENVANTERİ KONTROL ET");
                 Console.WriteLine("[4] 📋  KARAKTER AYRINTILARI");
                 Console.WriteLine("[5] 📈  STAT YÜKSELTME");
                 Console.WriteLine("[0] 💾  KAYDET VE DÜNYADAN AYRIL");

                 Console.ForegroundColor = ConsoleColor.DarkGray;
                 Console.Write("\n  Şu anki kararın nedir?: ");
                 Console.ResetColor();

                 string secim = Console.ReadLine();

                  switch (secim)
                  {
                    case "1":
                        BolgeSecimiYap();
                        break;

                    case "2":
                        SehreGit(); 
                        break;

                    case "3":
                        EnvanterMenusu();
                        break;

                    case "4":
                        KarakterAyrıntıları();
                        break;

                    case "5":
                        StatYukseltmeMenusu();
                        break;

                    case "0":
                        Console.WriteLine("\n  İlerleme kontrol ediliyor...");
                        
                        if (!oyuncu.Ad.StartsWith("Misafir_"))
                        {
                            OyunuKaydet(oyuncu);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("  [!] Ruhun ve eşyaların mühürlendi. Güvendesin.");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("  [!] Misafir olduğun için hatıraların rüzgara karışacak.");
                        }
                        Console.ResetColor();
                        Thread.Sleep(1500);
                        return; 

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("  [?] Bilinmeyen bir komut girdin, kahraman...");
                        Console.ResetColor();
                        Thread.Sleep(800);
                        break;
                  }
               }
            }

       
            private void SavasBaslat(Karakter oyuncu, Canavar hedef)
        {
            
            Random rnd = new Random();

            
            hedef.Can = hedef.MaksimumCan;

            Console.WriteLine($"\n--- {hedef.Ad} Belirdi! ---");
            Console.WriteLine($"Can: {hedef.Can}, Saldırı Gücü: {hedef.SaldiriGucu}");
            Thread.Sleep(1000);

            while (oyuncu.HayattaMi() && hedef.HayattaMi())
            {
                Console.WriteLine("\n--- Savaş Devam Ediyor ---");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"Oyuncu Can: {oyuncu.Can}/{oyuncu.MaksimumCan} ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("| ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{hedef.Ad} Can: {hedef.Can}/{hedef.MaksimumCan}");
                Console.ResetColor();

                Console.WriteLine("1. Saldır");
                Console.WriteLine("2. İksir kullan");
                Console.WriteLine("3. Kaç");

                string secim = Console.ReadLine();

                if (secim == "1")
                {
                    // OYUNCU SALDIRISI
                    int temelHasar = oyuncu.SaldiriGucu;
                    int sapma = (int)(temelHasar * 0.15);
                    int hamHasar = rnd.Next(temelHasar - sapma, temelHasar + sapma + 1);

                    bool kritikVurduMu = false;
                    if (rnd.Next(1, 101) <= oyuncu.KritikSans)
                    {
                        hamHasar *= 2;
                        kritikVurduMu = true;
                    }

                    int netHasar = Math.Max(1, hamHasar - hedef.Savunma);
                    hedef.Can -= netHasar;

                    if (kritikVurduMu)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(" [!!! MÜKEMMEL VURUŞ !!!] ");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"\n{oyuncu.Ad} tam kalbinden vurdu: {netHasar} HASAR!");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"\n{oyuncu.Ad}, {hedef.Ad}'a {netHasar} hasar verdi.");
                        Thread.Sleep(1000);
                    }
                    Console.ResetColor();

                    // CANAVAR ÖLDÜ MÜ? (ZAFER KISMI)
                    if (!hedef.HayattaMi())
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("===========================================");
                        Console.WriteLine("            SAVAŞ SONUCU (ZAFER!)          ");
                        Console.WriteLine("===========================================");
                        Console.ResetColor();

                        
                        int kazanilanExp = hedef.VerilenTecrube;
                        oyuncu.TecrubeKazan(kazanilanExp);
                        int kazanilanAltin = rnd.Next(15, 51);
                        oyuncu.Altın += kazanilanAltin;

                        Console.WriteLine($"Kazanılan EXP: +{kazanilanExp} [Bar: {oyuncu.GetEXPBar()}]");
                        Console.WriteLine($"Kazanılan ALTIN: 💰+{kazanilanAltin}");



                        List<Oge> dusenEsyalar = LootManager.LootDusur(hedef);

                        if (dusenEsyalar != null && dusenEsyalar.Count > 0)
                        {
                            foreach (var oge in dusenEsyalar)
                            {
                                
                                Console.ForegroundColor = Oge.NadirlikRengiGetir(oge.Nadirlik);
                                Console.WriteLine($"Düşen Eşya: [{oge.Nadirlik}] {oge.Ad} (Etki: {oge.EtkiDegeri})");
                                Console.ResetColor();

                                
                                oyuncu.Envanter.Add(oge);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Bu canavardan herhangi bir eşya düşmedi.");
                        }

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[!] Misafir Modunda oynuyorsanız, ilerlemeniz kaydedilmeyecektir.");
                        Console.ResetColor();

                        if (!oyuncu.Ad.StartsWith("Misafir_")) OyunuKaydet(oyuncu);

                        Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                        Console.ReadKey();
                        break;
                    }
                }
                else if (secim == "2")
                {
                    
                    var iksir = oyuncu.Envanter.FirstOrDefault(x => x.Ad.Contains("İksir"));
                    if (iksir != null)
                    {
                        oyuncu.Can = Math.Min(oyuncu.MaksimumCan, oyuncu.Can + iksir.EtkiDegeri);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n[İKSİR] {iksir.Ad} kullanıldı!");
                        Console.ResetColor();
                        oyuncu.Envanter.Remove(iksir);
                    }
                    else { Console.WriteLine("İksir yok!"); continue; }
                }
                else if (secim == "3")
                {
                    if (rnd.Next(100) < 40) { Console.WriteLine("Kaçtınız!"); return; }
                    else Console.WriteLine("Kaçamadınız!");
                }

                
                if (hedef.HayattaMi() && oyuncu.HayattaMi())
                {
                    int canavarHamHasar = rnd.Next(hedef.SaldiriGucu / 2, hedef.SaldiriGucu + 1);
                    int toplamSavunma = oyuncu.Savunma + (oyuncu.MevcutZirh?.EtkiDegeri ?? 0);
                    int gercekHasar = Math.Max(0, canavarHamHasar - toplamSavunma);

                    oyuncu.Can -= gercekHasar;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n{hedef.Ad} size {gercekHasar} hasar verdi!");
                    Console.ResetColor();
                }

                
                if (oyuncu.Can <= 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("===========================================");
                    Console.WriteLine("          SAVAŞ SONUCU (BOZGUN)            ");
                    Console.WriteLine("===========================================");
                    Console.ResetColor();

                    
                    int hedefCan = oyuncu.MaksimumCan / 2;
                    oyuncu.Can = 0;
                    while (oyuncu.Can < hedefCan)
                    {
                        oyuncu.Can += Math.Max(15, oyuncu.MaksimumCan / 75);
                        if (oyuncu.Can > hedefCan) oyuncu.Can = hedefCan;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("█ ");
                        Thread.Sleep(300);
                    }
                    Console.ResetColor();
                    Console.WriteLine($"\n\n[!] Gözlerinizi açtınız. Can: {oyuncu.Can}/{oyuncu.MaksimumCan}");
                    Thread.Sleep(2000);
                    break;
                }
            }
            } 

            private void EnvanterMenusu()
            {
                while (true)
                {
                    oyuncu.EnvanteriGoster();
                    Console.WriteLine("Hangi öğeyi kullanmak istersiniz? (Sıra numarasını girin, 0 ile geri dönün)");
                    Console.Write("Seçiminiz: ");
                    if (int.TryParse(Console.ReadLine(), out int secim))
                    {
                        if (secim == 0)
                        {
                            break; 
                        }
                        else
                        {
                            oyuncu.OgeKullan(secim - 1); 
                        }
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz giriş.");
                    }
                    Thread.Sleep(1000);
                }
            }

            private void StatYukseltmeMenusu()
        {
            bool menudeyim = true;

            while (menudeyim)
            {
                Console.Clear();
                Console.WriteLine($"\n--- STAT YÜKSELTME SİSTEMİ ---");
                Console.WriteLine($"Kullanılabilir Puan: {oyuncu.YetenekPuani}");
                Console.WriteLine("------------------------------");
                Console.WriteLine($"1. HP  (Stat: {oyuncu.HP_Stat})  -> (+5 Max Can)");
                Console.WriteLine($"2. STR (Stat: {oyuncu.STR_Stat}) -> (+2 Saldırı)");
                Console.WriteLine($"3. DEX (Stat: {oyuncu.DEX_Stat}) -> (+1 Savunma)");
                Console.WriteLine("\n[Çıkmak için 0'a basın]"); 

                Console.Write("\nSeçiminiz: ");
                string secim = Console.ReadLine();

                if (secim == "0")
                {
                    menudeyim = false;
                }
                else if (secim == "1" || secim == "2" || secim == "3")
                {
                    
                    if (oyuncu.YetenekPuani > 0)
                    {
                        if (secim == "1")
                        {
                            oyuncu.HP_Stat += 1;
                            oyuncu.MaksimumCan += 5;
                            oyuncu.Can = oyuncu.MaksimumCan; 
                            oyuncu.YetenekPuani -= 1;
                            Console.WriteLine("\n[+] HP başarıyla yükseltildi!");
                        }
                        else if (secim == "2")
                        {
                            oyuncu.STR_Stat += 1;
                            oyuncu.SaldiriGucu += 2;
                            oyuncu.YetenekPuani -= 1;
                            Console.WriteLine("\n[+] STR başarıyla yükseltildi!");
                        }
                        else if (secim == "3")
                        {
                            oyuncu.DEX_Stat += 1;
                            oyuncu.Savunma += 1;
                            oyuncu.YetenekPuani -= 1;
                            Console.WriteLine("\n[+] DEX başarıyla yükseltildi!");
                        }
                    }
                    else
                    {
                        
                        Console.WriteLine("\n[!] Yetersiz yetenek puanı! Stat yükseltemezsiniz.");
                    }

                    Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                    Console.ReadKey();
                }
                else
                {
                    
                    Console.WriteLine("\n[!] Yanlış işlem yaptınız! Lütfen geçerli bir seçim yapın.");
                    Console.WriteLine("Menüye yönlendiriliyorsunuz...");
                    Thread.Sleep(1500); // 1.5 saniye bekleyip menüye döner
                }
            }
        }  
           
            private void KarakterAyrıntıları()
            {
                Console.Clear(); 



                int gerekenToplamTecrube = oyuncu.SonrakiSeviyeIcinGerekenToplamEXP();
                int kalanTecrube = gerekenToplamTecrube - oyuncu.Tecrube;

                Console.WriteLine("========================================");
                Console.WriteLine($"        KARAKTER PROFİLİ: {oyuncu.Ad.ToUpper()} ");
                Console.WriteLine("========================================");
                Console.WriteLine($" [SEVİYE]          : {oyuncu.Seviye}");
                Console.WriteLine($" [CAN]             : {oyuncu.Can} / {oyuncu.MaksimumCan}");
                Console.WriteLine($" [SALDIRI GÜCÜ]    : {oyuncu.SaldiriGucu}");
                Console.WriteLine($" [SAVUNMA]         : {oyuncu.Savunma}");
                Console.WriteLine($" [ALTIN]           : {oyuncu.Altın}");
                Console.WriteLine($" [KRİTİK ŞANS]     : %{oyuncu.KritikSans}");
                Console.WriteLine($" [İLERLEME]        : {oyuncu.GetEXPBar()}"); 
                Console.WriteLine($" [SEVİYE TP]       : {oyuncu.Tecrube} / {gerekenToplamTecrube}");
                Console.WriteLine($" [TOPLAM TP]       : {oyuncu.ToplamTecrube}"); 
                Console.WriteLine($" [KALAN TP]        : Bir Sonraki Seviye İçin {kalanTecrube} TP lazım.");


                Console.WriteLine("----------------------------------------");
                Console.WriteLine("         TEMEL İSTATİSTİKLER  ");
                Console.WriteLine("----------------------------------------");
                Console.WriteLine($" [HP  (Can)]       : {oyuncu.HP_Stat}  -> (Max Can: {oyuncu.MaksimumCan})");
                Console.WriteLine($" [STR (Güç)]       : {oyuncu.STR_Stat}  -> (Hasar: {oyuncu.SaldiriGucu})");
                Console.WriteLine($" [DEX (Savunma)]   : {oyuncu.DEX_Stat}  -> (Zırh: {oyuncu.Savunma})");


                Console.WriteLine("----------------------------------------");
                Console.WriteLine($" [YETENEK PUANI]   : {oyuncu.YetenekPuani} (Harcanabilir)");
                Console.WriteLine("========================================");


                // Silah Satırı
                string silahBilgi = oyuncu.DonanimliSilah != null
                ? $"{oyuncu.DonanimliSilah.Ad} (+{oyuncu.DonanimliSilah.EtkiDegeri} Saldırı)"
                : "Yok";

                // Zırh Satırı
                string zirhBilgi = oyuncu.MevcutZirh != null
                ? $"{oyuncu.MevcutZirh.Ad} (+{oyuncu.MevcutZirh.EtkiDegeri} Savunma)"
                : "Yok";

                Console.WriteLine($" [KUŞANILMIŞ SİLAH]: {silahBilgi}");
                Console.WriteLine($" [KUŞANILMIŞ ZIRH] : {zirhBilgi}");
                Console.WriteLine("========================================");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" TOPLAM SALDIRI GÜCÜ : {oyuncu.SaldiriGucu}");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($" TOPLAM SAVUNMA GÜCÜ : {oyuncu.Savunma}");
                Console.ResetColor();

                Console.WriteLine("\nAna menüye dönmek için bir tuşa basın...");
                Console.ReadKey();
            }

            public Oge RastgeleOgeUret()
            {
               Random rnd = new Random();
               int sans = rnd.Next(1, 101); 

            
               Nadirlik secilenNadirlik;

                if (sans <= 1) secilenNadirlik = Nadirlik.Mythic;    
                else if (sans <= 5) secilenNadirlik = Nadirlik.Legendary; 
                else if (sans <= 15) secilenNadirlik = Nadirlik.Epic;      
                else if (sans <= 35) secilenNadirlik = Nadirlik.Rare;      
                else if (sans <= 65) secilenNadirlik = Nadirlik.Uncommon;  
                else secilenNadirlik = Nadirlik.Common;    

            
               OgeTuru secilenTur = (OgeTuru)rnd.Next(0, 3); 

            
               string esyaAdı = "";
               int etki = 0;

            
               int nadirlikCarpani = (int)secilenNadirlik + 1;

               switch (secilenTur)
               {
                  case OgeTuru.Silah:
                    esyaAdı = $"{secilenNadirlik} Kılıç";
                    etki = rnd.Next(5, 11) * nadirlikCarpani; 
                    break;
                  case OgeTuru.Zirh:
                    esyaAdı = $"{secilenNadirlik} Zırh";
                    etki = rnd.Next(2, 6) * nadirlikCarpani;
                    break;
                  case OgeTuru.Tuketilebilir:
                    esyaAdı = $"{secilenNadirlik} İksir";
                    etki = rnd.Next(15, 26) * nadirlikCarpani;
                    break;
               }

            
               return new Oge(esyaAdı, secilenNadirlik, secilenTur, etki);
            }

            public void SehreGit()
            {
               bool sehirdeyim = true;

               while (sehirdeyim)
               {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===============================================");
                Console.WriteLine("        🏰 GÜMÜŞIŞIK ŞEHRİ MERKEZİ        ");
                Console.WriteLine("===============================================");
                Console.ResetColor();

                // Şehir içi durum çubuğu
                Console.WriteLine($"  👤 {oyuncu.Ad} | ❤️ Can: {oyuncu.Can}/{oyuncu.MaksimumCan} | 💰 Altın: {oyuncu.Altın}");
                Console.WriteLine("-----------------------------------------------");

                Console.WriteLine("\n  [1] 🍻 Şehir Hanı (Dinlen ve İyileş)");
                Console.WriteLine("  [2] ⚖️  Market (Eşya Al/Sat) - [Yakında]");
                Console.WriteLine("  [3] 🔨 Demirci (Zırh Geliştir) - [Yakında]");
                Console.WriteLine("  [0] ⬅️  Şehir Kapısından Çık (Ana Menü)");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("\n  Nereye gitmek istersin?: ");
                Console.ResetColor();

                string secim = Console.ReadLine();

                switch (secim)
                {
                    case "1":
                        SehirHani(); // Han metoduna git
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("\n🛒 MARKET SOKAĞI");
                        Console.WriteLine("----------------");
                        Console.WriteLine("Tüccar kervanı henüz şehre ulaşmadı. (Yapım Aşamasında)");
                        Console.WriteLine("\nDönmek için bir tuşa bas...");
                        Console.ReadKey();
                        break;
                    case "0":
                        sehirdeyim = false; 
                        break;
                    default:
                        Console.WriteLine("\n  Muhafızlar size garip bakıyor, geçersiz seçim.");
                        Thread.Sleep(1000);
                        break;
                }
               }
            }

            public void SehirHani()
            {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--- 🍻 GÜMÜŞIŞIK HANI ---");
            Console.ResetColor();

            if (oyuncu.Can >= oyuncu.MaksimumCan)
            {
                Console.WriteLine("\nHancı: 'Zaten turp gibisin evlat! Git de biraz canavar avla.'");
                Console.WriteLine("\nAna menüye dönmek için bir tuşa bas...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nHancı: 'Oldukça bitkin görünüyorsun. 25 Altına sıcak bir yemek ve temiz bir yatak ister misin?'");
            Console.WriteLine($"[Cüzdanın: {oyuncu.Altın} 💰]");
            Console.WriteLine("\n  [1] Evet, İyileş (-25 💰)");
            Console.WriteLine("  [0] Hayır, kalsın.");
            Console.Write("\nKararın: ");

            string hancıSecim = Console.ReadLine();

            if (hancıSecim == "1")
            {
                if (oyuncu.Altın >= 25)
                {
                    oyuncu.Altın -= 25;
                    oyuncu.Can = oyuncu.MaksimumCan; 
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n[!] Güzelce dinlendin ve tüm yaraların iyileşti!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nHancı: Yeterli Paran Yok");
                    Console.ResetColor();
                }
                Thread.Sleep(1500);
            }
            }

            public void BolgeSecimiYap()
        {
            Console.Clear();
            
            List<Bolge> gosterilenBolgeler = new List<Bolge>();

            var tumBolgeler = CanavarVeritabani.GumusIsikKoyuBolgeleri;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== GİDİLEBİLİR BÖLGELER ===");
            Console.ResetColor();


            for (int i = 0; i < tumBolgeler.Count; i++)
            {
                if (oyuncu.Seviye >= tumBolgeler[i].MinSeviye)
                {
                    gosterilenBolgeler.Add(tumBolgeler[i]);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[{gosterilenBolgeler.Count}] {tumBolgeler[i].Ad} (Min Lvl: {tumBolgeler[i].MinSeviye})");
                    Console.ResetColor();

                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[0] Geri Dön");
            Console.ResetColor();
            Console.Write("\nSeçimin: ");
            string secim = Console.ReadLine();

            if (secim == "0") return;

            
            if (int.TryParse(secim, out int secilenNo) && secilenNo > 0 && secilenNo <= gosterilenBolgeler.Count)
            {
                Bolge secilenBolge = gosterilenBolgeler[secilenNo - 1]; 

                Random rng = new Random();
                Canavar karsilasilan = secilenBolge.Canavarlar[rng.Next(secilenBolge.Canavarlar.Count)];

                SavasBaslat(oyuncu, karsilasilan);
            }
            else
            {
                Console.WriteLine("Geçersiz seçim veya yetersiz seviye!");
                Thread.Sleep(1000);
            }
        }

    }
}
