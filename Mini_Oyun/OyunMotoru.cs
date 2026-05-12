using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;


namespace Mini_Oyun
{
    internal class Oyun_Motoru
    {
        private Karakter oyuncu;
        private List<Canavar> canavarHavuzu;
        private List<Boss> bossHavuzu;
        public static int temizlenenGrupSayisi = 0;
        // private Random random = new Random(); Eskisi için geçerli idi şimdi lazım değil ama kalabilir.
        // Canavarları Random Getiremek için Kullanıldı Bölge işe girince savaş başlat parametresinde geçersiz kaldı.


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

        #region OYUN KAYDETME VE YÜKLEME SİSTEMİ (Gelişmiş Güvenlik Önlemleriyle)
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

        public static void OyunuKaydet(Karakter k)
        {
            if (k == null) return;

            try
            {
                string dosyaAdi = $"{k.Ad}_kayit.json";

                
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles // KRİTİK SATIR
                };

                string jsonVerisi = JsonSerializer.Serialize(k, options);
                string gizliVeri = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonVerisi));
                string imza = Sifreleme.HashHesapla(gizliVeri + "Gumusisik2026!");

                File.WriteAllText(dosyaAdi, imza + Environment.NewLine + gizliVeri);

                
                 Console.WriteLine($"\n[💾] {k.Ad} başarıyla kaydedildi."); 
            }
            catch (Exception ex)
            {
                // Hatayı görmek için burayı geçici olarak açabilirsin:
                // Console.WriteLine("Kayıt Hatası: " + ex.Message);
            }
        }

        public static class Sifreleme
        {
            public static string HashHesapla(string veri)
            {
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(veri));
                    return BitConverter.ToString(bytes).Replace("-", "").ToLower();
                }
            }
        }

        public Karakter OyunuYukle(string ad)
        {
            string dosyaAdi = $"{ad}_kayit.json";

            if (!File.Exists(dosyaAdi)) return null;

            string[] satirlar = File.ReadAllLines(dosyaAdi);
            if (satirlar.Length < 2) return null;

            string dosyadakiImza = satirlar[0];
            string dosyadakiGizliVeri = satirlar[1];

            // GÜVENLİK KONTROLÜ
            if (Sifreleme.HashHesapla(dosyadakiGizliVeri + "Gumusisik2026!") == dosyadakiImza)
            {

                string orijinalJson = Encoding.UTF8.GetString(Convert.FromBase64String(dosyadakiGizliVeri));
                return JsonSerializer.Deserialize<Karakter>(orijinalJson);
            }
            else
            {
                // HİLE ALGILANDI
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("************************************************");
                Console.WriteLine($"[GÜVENLİK İHLALİ] {ad.ToUpper()} KAYDI TAHRİF EDİLMİŞ!");
                Console.WriteLine("[!] İlerlemeniz silindi. Hesap sıfırlanıyor...");
                Console.WriteLine("************************************************");
                Console.ResetColor();


                Console.Write("\nHesabınız için yeni bir şifre belirleyin: ");
                string yeniSifre = Console.ReadLine();


                Karakter yeniKarakter = new Karakter
                {
                    Ad = ad,
                    Sifre = yeniSifre,
                    Seviye = 1,
                    Altın = 0,
                    Can = 100,
                    BoranTanindi = false,
                    EleraTanindi = false
                };

                OyunuKaydet(yeniKarakter);

                Console.WriteLine("\n[✓] Hesap başarıyla sıfırlandı ve yeni şifreniz kaydedildi.");
                Thread.Sleep(2000);

                return yeniKarakter;
            }
        }
        #endregion

        #region CANAVAR VE BOSS OLUŞTURMA METOTLARI (Veritabanından Dinamik Yükleme)
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
        #endregion

        #region OYUN DÖNGÜSÜ VE SAVAŞ MEKANİĞİ (Gelişmiş Görsel Düzen ve Detaylı Savaş Logları)
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
                        SehreGit(oyuncu);
                        break;

                    case "3":
                        EnvanterMenusu(oyuncu);
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


        private void SavasBaslat(Karakter oyuncu, Bolge secilenBolge)
        {
            Random rnd = new Random();
            bool bolgeKesifDevam = true;

            while (bolgeKesifDevam)
            {
                List<Canavar> slotlar = new List<Canavar>();

                // 1. DÜŞMANLARI OLUŞTURMA (Mantık Korundu)
                for (int i = 0; i < 3; i++)
                {
                    Canavar taslak = secilenBolge.RastgeleCanavarGetir();
                    Canavar yeniCanavar = new Canavar(
                        taslak.Ad,
                        taslak.MaksimumCan,
                        taslak.SaldiriGucu,
                        taslak.VerilenTecrube,
                        taslak.Turu,
                        taslak.LootTableId
                    );
                    yeniCanavar.Savunma = taslak.Savunma;
                    slotlar.Add(yeniCanavar);
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n>>> {secilenBolge.Ad.ToUpper()} KEŞFEDİLİYOR...");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[!] PUSU: 3 adet {slotlar[0].Ad} ortaya çıktı!");
                Console.ResetColor();
                Thread.Sleep(1500);

                // 2. MEVCUT SAVAŞ DÖNGÜSÜ (Görselleştirilmiş)
                while (oyuncu.HayattaMi() && slotlar.Any(s => s.HayattaMi()))
                {
                    Console.Clear();

                    // --- DÜŞMAN PANELİ ---
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("┌────────────────────── DÜŞMANLAR ─────────────────────────┐");
                    for (int i = 0; i < slotlar.Count; i++)
                    {
                        var c = slotlar[i];
                        string durum = c.HayattaMi() ? CanBariCiz(c.Can, c.MaksimumCan) : "---------- ÖLÜ ----------";
                        string satir = $"│ [Slot {i + 1}] {c.Ad,-12} {durum}";
                        Console.WriteLine(satir.PadRight(59) + "│");
                    }
                    Console.WriteLine("└──────────────────────────────────────────────────────────┘");

                    // --- OYUNCU PANELİ ---
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n┌────────────────────── KAHRAMAN ──────────────────────────┐");


                    Console.Write("│ ");


                    Console.ForegroundColor = ConsoleColor.Green;
                    string adVeSeviye = $"{oyuncu.Ad} (Lvl: {oyuncu.Seviye})";
                    string oyuncuCanBar = CanBariCiz(oyuncu.Can, oyuncu.MaksimumCan);
                    string ustIcerik = $"{adVeSeviye,-18} {oyuncuCanBar}";
                    Console.Write(ustIcerik.PadRight(56));


                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" │");


                    Console.Write("│ ");
                    Console.ForegroundColor = ConsoleColor.Cyan;

                    string statlar = $"GÜÇ: {oyuncu.SaldiriGucu,-4} │ SAVUNMA: {oyuncu.Savunma,-4} │ ALTIN: {oyuncu.Altın,-6} 💰";
                    Console.Write(statlar.PadRight(56));

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" │");
                    Console.WriteLine("└──────────────────────────────────────────────────────────┘");
                    Console.ResetColor();

                    // --- AKSİYON MENÜSÜ ---
                    Console.WriteLine("\n [1] ⚔️ Geniş Saldırı  [2] 🧪 İksir  [3] 🏃 Kaç");
                    Console.Write("\nKararın: ");
                    string secim = Console.ReadLine();

                    if (secim == "1")
                    {
                        // 1. DÜŞMANLARIN SALDIRISI (Log Sistemi)
                        Console.WriteLine("\n--- 🛡️ SAVUNMA SIRASI ---");
                        foreach (var canavar in slotlar.Where(s => s.HayattaMi()))
                        {
                            int cHasar = Math.Max(1, rnd.Next(canavar.SaldiriGucu - 2, canavar.SaldiriGucu + 3) - oyuncu.Savunma);
                            oyuncu.Can -= cHasar;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"> {canavar.Ad} vurdu: -{cHasar} HP!");
                            Thread.Sleep(600);
                        }
                        Console.ResetColor();

                        if (!oyuncu.HayattaMi()) break;

                        // 2. OYUNCUNUN SALDIRISI
                        Console.WriteLine("\n--- ⚔️ SALDIRI SIRASI ---");
                        int temelHasar = oyuncu.SaldiriGucu;
                        int sapma = (int)(temelHasar * 0.15);
                        int hamHasar = rnd.Next(temelHasar - sapma, temelHasar + sapma + 1);

                        foreach (var hedef in slotlar.Where(s => s.HayattaMi()))
                        {
                            int netHasar = Math.Max(1, hamHasar - hedef.Savunma);
                            hedef.Can -= netHasar;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"-> {hedef.Ad} düşmanına {netHasar} hasar verdin!");
                        }
                        Console.ResetColor();
                        Thread.Sleep(1500);
                    }
                    else if (secim == "3") return;
                }

                // 3. ZAFER VE GANİMET (Gelişmiş Görünüm)
                if (slotlar.All(s => !s.HayattaMi()))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("╔═════════════════════════════════════════════════════════╗");
                    Console.WriteLine("║                        🏆 ZAFER 🏆                       ║");
                    Console.WriteLine("╚═════════════════════════════════════════════════════════╝");
                    Console.ResetColor();

                    int toplamExp = slotlar.Sum(s => s.VerilenTecrube);
                    int toplamAltin = rnd.Next(45, 101);

                    oyuncu.TecrubeKazan(toplamExp);
                    oyuncu.Altın += toplamAltin;

                    // EXP Bar Hesaplamaları
                    int baslangicEXP = oyuncu.MevcutSeviyeBaslangicEXP();
                    int gerekenEXP = oyuncu.SonrakiSeviyeIcinGerekenToplamEXP();
                    int mevcutIlerleme = oyuncu.Tecrube - baslangicEXP;
                    int seviyeIcinGereken = gerekenEXP - baslangicEXP;

                    double yuzde = seviyeIcinGereken > 0 ? (double)mevcutIlerleme / seviyeIcinGereken * 100 : 0;
                    yuzde = Math.Min(100, Math.Max(0, yuzde));

                    Console.WriteLine($"\n⭐ Kazanılan EXP: +{toplamExp}");
                    Console.WriteLine($"💰 Kazanılan Altın: +{toplamAltin}");
                    Console.WriteLine($"📊 Seviye İlerlemesi: [%{(int)yuzde}]");
                    Console.WriteLine(CanBariCiz((int)yuzde, 100));

                    // GRUP GANİMETİ MANTIĞI
                    temizlenenGrupSayisi++;
                    if (temizlenenGrupSayisi % 3 == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\n✨ TEBRİKLER: 3. Grup Temizlendi! Nadir Ganimetler Toplanıyor...");
                        Console.WriteLine("--- 📦 NADİR GRUP GANİMETLERİ ---");
                        Console.ResetColor();

                        List<Oge> tumPotansiyelLoot = new List<Oge>();
                        foreach (var canavar in slotlar)
                            tumPotansiyelLoot.AddRange(LootManager.LootDusur(canavar));

                        int alinacakMiktar = (rnd.Next(1, 101) <= 2) ? 2 : 1;
                        var secilenGanimetler = tumPotansiyelLoot.OrderByDescending(o => o.Nadirlik).Take(alinacakMiktar).ToList();

                        foreach (var oge in secilenGanimetler)
                        {
                            // STACK KONTROLÜ: Envanterde aynı isimde ve yer olan bir slot var mı?
                            var mevcutStack = oyuncu.Envanter.Find(x => x.Ad == oge.Ad && x.Miktar < x.MaksimumStack);

                            if (mevcutStack != null)
                            {
                                // Zaten varsa sadece miktarını artır (Envanter doluluğuna bakmaya gerek yok)
                                mevcutStack.Miktar++;
                                Console.ForegroundColor = Oge.NadirlikRengiGetir(oge.Nadirlik);
                                Console.WriteLine($" > {oge.Ad} (x{mevcutStack.Miktar}) mevcut yığına eklendi.");
                            }
                            else if (oyuncu.Envanter.Count < 20) // Yeni bir slot gerekiyorsa yer var mı?
                            {
                                oyuncu.Envanter.Add(oge);
                                Console.ForegroundColor = Oge.NadirlikRengiGetir(oge.Nadirlik);
                                Console.WriteLine($" > {oge.Ad} bulundu! Yeni envanter slotuna eklendi.");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine($" > [!] Çanta dolu! {oge.Ad} kayboldu.");
                            }
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        int kalan = 3 - (temizlenenGrupSayisi % 3);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        if (kalan == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("\n🔥 DİKKAT: Bir sonraki grup temizlendiğinde NADİR GANİMET düşecek!");
                        }
                        else
                        {
                            Console.WriteLine($"\nℹ️ Nadir grup ganimeti için temizlenmesi gereken grup: {kalan}");
                        }
                        Console.ResetColor();
                    }
                }

                if (!oyuncu.Ad.StartsWith("Misafir_")) OyunuKaydet(oyuncu);

                // SAVAŞ SONRASI SEÇENEKLERİ
                bool kararVerildi = false;
                while (!kararVerildi)
                {
                    Console.WriteLine("\n[1] Keşfe Devam [2] Şehre Dön [3] Envanter [0] Çıkış");
                    Console.Write("Seçiminiz: ");
                    string sSecim = Console.ReadLine();

                    if (sSecim == "1")
                    {
                        kararVerildi = true;
                    }
                    else if (sSecim == "2")
                    {
                        SehreGit(oyuncu);
                        return;
                    }
                    else if (sSecim == "3")
                    {
                        EnvanterMenusu(oyuncu);
                    }
                    else if (sSecim == "0")
                    {
                        return;
                    }




                    else if (!oyuncu.HayattaMi())
                    {
                        Console.Clear();

                        oyuncu.Can = 0;

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("╔═════════════════════════════════════════════════════════╗");
                        Console.WriteLine("║                    💀 MAĞLUBİYET 💀                     ║");
                        Console.WriteLine("╚═════════════════════════════════════════════════════════╝");

                        Console.WriteLine($"\n [!] {oyuncu.Ad.ToUpper()} karanlığa yenik düştü...");
                        Console.WriteLine(" [!] Bilincin kapanırken Gümüşışık surlarını sayıklıyorsun.");
                        Console.ResetColor();

                        Console.WriteLine("\n-----------------------------------------------------------");
                        Console.WriteLine(" Gözlerini açtığında kendini Gümüşışık'ta bulacaksın...");
                        Console.WriteLine("-----------------------------------------------------------");

                        Console.WriteLine("\n[Devam etmek için bir tuşa bas...]");
                        Console.ReadKey();


                        bolgeKesifDevam = false;
                        SehreGit(oyuncu);
                    }
                }
            }
        }


        
        public string CanBariCiz(int suAn, int maks)
        {
            int barGenislik = 20;
            int doluKisim = (int)((double)suAn / maks * barGenislik);
            if (doluKisim < 0) doluKisim = 0;

            return "[" + new string('█', doluKisim) + new string('░', barGenislik - doluKisim) + $"] {suAn}/{maks}";
        }
        #endregion


        #region ENVANTER MENÜSÜ (Gelişmiş Görsel Düzen ve Seçim Sistemi)    
        private void EnvanterMenusu(Karakter karakter)
        {
            while (true)
            {
                Console.Clear();
                Dictionary<int, Oge> secimMap = new Dictionary<int, Oge>();
                int gosterimSirasi = 1;

                Console.ForegroundColor = ConsoleColor.Cyan;
                // Üst Çerçeve
                Console.WriteLine("┌" + new string('─', 24) + " ENVANTER " + new string('─', 24) + "┐");

                // Kategoriler
                YazdirKategori("⚔️ SİLAHLAR", "Silah", ConsoleColor.Yellow, ref gosterimSirasi, secimMap);
                Console.WriteLine("├" + new string('─', 58) + "┤");

                YazdirKategori("🛡️ ZIRHLAR", "Zirh", ConsoleColor.Green, ref gosterimSirasi, secimMap);
                Console.WriteLine("├" + new string('─', 58) + "┤");

                YazdirKategori("🧪 İKSİRLER", "Tuketilebilir", ConsoleColor.Red, ref gosterimSirasi, secimMap);

                // Alt Çerçeve
                Console.WriteLine("└" + new string('─', 58) + "┘");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n [0] Geri Dön");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nKullanmak istediğin numara: ");

                if (int.TryParse(Console.ReadLine(), out int secim))
                {
                    if (secim == 0) break;

                    if (secimMap.ContainsKey(secim))
                    {
                        Oge secilenOge = secimMap[secim];

                        
                        secilenOge.DetaySayfasiGoster(karakter);

                        string detaySecim = Console.ReadLine();
                        if (detaySecim == "1")
                        {
                            
                            if (karakter.Seviye < secilenOge.GerekenSeviye)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n [!] Seviyen bu kadim eşyayı kullanmaya yetmiyor!");
                                Console.ResetColor();
                                Thread.Sleep(1500);
                            }
                            else
                            {
                                karakter.OgeKullanNesneIle(secilenOge); 
                            }
                        }
                    }
                }
            }
        }


        private void YazdirKategori(string baslik, string turFiltresi, ConsoleColor renk, ref int sira, Dictionary<int, Oge> map)
        {
            // BAŞLIK SATIRI
            Console.Write("│ ");
            Console.ForegroundColor = renk;

            string[] parcalar = baslik.Split(new char[] { ' ' }, 2);
            string emoji = parcalar[0];
            string metin = parcalar.Length > 1 ? parcalar[1] : "";

            Console.Write(emoji + " ");
            Console.Write(metin.PadRight(53));

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" │");

            // İÇERİK SATIRLARI (Eşyalar)
            foreach (var oge in oyuncu.Envanter)
            {
                // Tür kontrolünü daha kesin yapmak için Equals veya direkt Enum kontrolü önerilir
                if (oge.Tur.ToString() == turFiltresi)
                {
                    Console.Write("│  ");

                    // Nadirlik rengini eşyanın adına uygula
                    Console.ForegroundColor = Oge.NadirlikRengiGetir(oge.Nadirlik);

                    // STACK GÖSTERİMİ: Miktar 1'den büyükse (x5) şeklinde ekle
                    string miktarGosterimi = oge.Miktar > 1 ? $" (x{oge.Miktar})" : "";
                    string satir = $"[{sira}] {oge.Ad}{miktarGosterimi} (+{oge.EtkiDegeri})";

                    // İçerik satırını 55 karaktere sabitle
                    Console.Write(satir.PadRight(55));

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" │");

                    
                    if (!map.ContainsKey(sira))
                    {
                        map.Add(sira, oge);
                        sira++;
                    }
                }
            }
        }
        #endregion
        private void StatYukseltmeMenusu()
        {
            bool menudeyim = true;

            while (menudeyim)
            {
                Console.Clear();
                // --- BAŞLIK PANELİ ---
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("┌──────────────────────────────────────────────────────────┐");
                Console.WriteLine("│               📊 STAT YÜKSELTME MENÜSÜ 📊                │");
                Console.WriteLine("└──────────────────────────────────────────────────────────┘");

                // --- BİLGİ SATIRI ---
                Console.Write("│ ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                string puanBilgi = $"Kullanılabilir Puan: {oyuncu.YetenekPuani}";
                Console.Write(puanBilgi.PadRight(56));
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" │");
                Console.WriteLine("├──────────────────────────────────────────────────────────┤");

                // --- STAT LİSTESİ ---
                // HP Satırı
                Console.Write("│ ");
                Console.ForegroundColor = ConsoleColor.White;
                string hpSatir = $"[1] ❤️ HP  (Seviye: {oyuncu.HP_Stat}) -> (+5 Max Can)";
                Console.Write(hpSatir.PadRight(56)); 
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" │");

                // STR Satırı
                Console.Write("│ ");
                Console.ForegroundColor = ConsoleColor.White;
                string strSatir = $"[2] ⚔️ STR (Seviye: {oyuncu.STR_Stat}) -> (+2 Saldırı)";
                Console.Write(strSatir.PadRight(56));
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" │");

                // DEX Satırı
                Console.Write("│ ");
                Console.ForegroundColor = ConsoleColor.White;
                string dexSatir = $"[3] 🛡️ DEX (Seviye: {oyuncu.DEX_Stat}) -> (+1 Savunma)";
                Console.Write(dexSatir.PadRight(57));
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" │");

                Console.WriteLine("├──────────────────────────────────────────────────────────┤");
                Console.Write("│ ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("[0] Geri Dön".PadRight(56));
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" │");
                Console.WriteLine("└──────────────────────────────────────────────────────────┘");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nGeliştirmek istediğin stat (0-3): ");
                Console.ResetColor();

                string secim = Console.ReadLine();

                if (secim == "0")
                {
                    menudeyim = false;
                }
                else if (secim == "1" || secim == "2" || secim == "3") // HER İŞLEMDEN SONRA KAYDEDİLECEK ŞEKİLDE AYARLANDI.
                {
                    if (oyuncu.YetenekPuani > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        if (secim == "1")
                        {
                            oyuncu.HP_Stat += 1;
                            oyuncu.MaksimumCan = 100 + (oyuncu.HP_Stat * 5);
                            oyuncu.Can = oyuncu.MaksimumCan;
                            oyuncu.YetenekPuani -= 1;
                            Console.WriteLine("\n[+] Vücudun daha dayanıklı hale geliyor! (HP Yükseltildi)");
                            Console.WriteLine("Oyun otomatik olarak kaydediliyor...");
                            OyunuKaydet(oyuncu);
                        }
                        else if (secim == "2")
                        {
                            oyuncu.STR_Stat += 1;
                            oyuncu.SaldiriGucu = 25 + (oyuncu.STR_Stat * 2);
                            oyuncu.YetenekPuani -= 1;
                            Console.WriteLine("\n[+] Kasların güçleniyor, vuruşların derinleşiyor! (STR Yükseltildi)");
                            Console.WriteLine("Oyun otomatik olarak kaydediliyor...");
                            OyunuKaydet(oyuncu);
                        }
                        else if (secim == "3")
                        {
                            oyuncu.DEX_Stat += 1;
                            oyuncu.Savunma = 1 + (oyuncu.DEX_Stat * 1);
                            oyuncu.YetenekPuani -= 1;
                            Console.WriteLine("\n[+] Reflekslerin keskinleşiyor! (DEX Yükseltildi)");
                            Console.WriteLine("Oyun otomatik olarak kaydediliyor...");
                            OyunuKaydet(oyuncu);
                        }
                        Console.ResetColor();
                        Thread.Sleep(1200);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[!] Yetersiz yetenek puanı! Maceralara devam ederek TP kazanmalısın.");
                        Console.ResetColor();
                        Thread.Sleep(1500);
                    }
                }
            }
        }

        #region KARAKTER PROFİLİ YAZDIRMA METODU (Gelişmiş Görsel Düzen ve Detaylı Bilgi)
        private void KarakterAyrıntıları()
        {
            Console.Clear();
            int panelGenislik = 58; // İç genişlik
            int gerekenToplamTecrube = oyuncu.SonrakiSeviyeIcinGerekenToplamEXP();
            int kalanTecrube = gerekenToplamTecrube - oyuncu.Tecrube;

            // --- ÜST BAŞLIK ---
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("┌" + new string('─', 20) + " KARAKTER PROFİLİ " + new string('─', 20) + "┐");

            YazdirProfilSatiri("AD", oyuncu.Ad.ToUpper(), ConsoleColor.Yellow);
            Console.WriteLine("├" + new string('─', panelGenislik) + "┤");

            // --- TEMEL BİLGİLER ---
            YazdirProfilSatiri("SEVİYE", oyuncu.Seviye.ToString(), ConsoleColor.White);
            YazdirProfilSatiri("CAN", $"{oyuncu.Can} / {oyuncu.MaksimumCan}", ConsoleColor.Red);
            YazdirProfilSatiri("ALTIN", oyuncu.Altın.ToString(), ConsoleColor.Yellow);
            YazdirProfilSatiri("KRİTİK", "%" + oyuncu.KritikSans, ConsoleColor.Magenta);

            // --- İLERLEME ÇUBUĞU ---
            string expBar = oyuncu.GetEXPBar();
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("│ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"[{"İLERLEME".PadRight(12)}] : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(expBar);
            
            int barKalanBosluk = 39 - expBar.Length;
            if (barKalanBosluk > 0) Console.Write(new string(' ', barKalanBosluk));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" │");

            // --- TP DETAYLARI ---
            YazdirProfilSatiri("SEVİYE TP", $"{oyuncu.Tecrube} / {gerekenToplamTecrube}", ConsoleColor.Yellow);
            YazdirProfilSatiri("KALAN TP", $"{kalanTecrube} TP lazım", ConsoleColor.Magenta);
            YazdirProfilSatiri("TOPLAM TP", oyuncu.ToplamTecrube.ToString(), ConsoleColor.Green);
            

            // --- İSTATİSTİKLER BÖLÜMÜ ---
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("├" + new string('─', 21) + " İSTATİSTİKLER " + new string('─', 22) + "┤");

            YazdirProfilSatiri("HP (Can)", $"{oyuncu.HP_Stat} (+{oyuncu.MaksimumCan - 100} Max Can)", ConsoleColor.Green);
            YazdirProfilSatiri("STR (Güç)", $"{oyuncu.STR_Stat} (Hasar: {25 + (oyuncu.STR_Stat * 2)})", ConsoleColor.Red);
            YazdirProfilSatiri("DEX (Sav)", $"{oyuncu.DEX_Stat} (Zırh: {1 + (oyuncu.DEX_Stat * 1)})", ConsoleColor.Blue);
            YazdirProfilSatiri("YETENEK P.", $"{oyuncu.YetenekPuani} (Harcanabilir)", ConsoleColor.Yellow);

            // --- EKİPMANLAR BÖLÜMÜ ---
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("├" + new string('─', 22) + " EKİPMANLAR " + new string('─', 24) + "┤");

            string silahBilgi = oyuncu.DonanimliSilah != null
                ? $"{oyuncu.DonanimliSilah.Ad} (+{oyuncu.DonanimliSilah.EtkiDegeri} Hasar)" : "Yok";
            string zirhBilgi = oyuncu.MevcutZirh != null
                ? $"{oyuncu.MevcutZirh.Ad} (+{oyuncu.MevcutZirh.EtkiDegeri} Savunma)" : "Yok";

            YazdirProfilSatiri("SİLAH", silahBilgi, ConsoleColor.Yellow);
            YazdirProfilSatiri("ZIRH", zirhBilgi, ConsoleColor.Cyan);

            // --- TOPLAM GÜÇ VE KAPANIŞ ---
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("├" + new string('─', panelGenislik) + "┤");
            YazdirProfilSatiri("TOPLAM HASAR", oyuncu.ToplamSaldiriGucu.ToString(), ConsoleColor.Red);
            YazdirProfilSatiri("TOPLAM ZIRH", oyuncu.ToplamSavunma.ToString(), ConsoleColor.Cyan);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("└" + new string('─', panelGenislik) + "┘");

            Console.ResetColor();
            Console.WriteLine("\nAna menüye dönmek için bir tuşa basın...");
            Console.ReadKey();
        }

        private void YazdirProfilSatiri(string baslik, string deger, ConsoleColor degerRengi)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("│ ");

            Console.ForegroundColor = ConsoleColor.Gray;
            // Başlık alanı (12 karakter) + süsleme = 18 karakter
            Console.Write($"[{baslik.PadRight(12)}] : ");

            Console.ForegroundColor = degerRengi;
            // Toplam 58 - 18 = 40 karakterlik değer alanı (Tam hizalama)
            Console.Write(deger.PadRight(39));

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" │");
        }
        #endregion

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

        public void SehreGit(Karakter karakter) 
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

                Console.WriteLine("\n  [1] 🍻 Şehir Hancısı Boran (Dinlen ve İyileş)");
                Console.WriteLine("  [2] ⚖️ Market Elera (İksir AL/Sat) - [Yakında]");
                Console.WriteLine("  [3] ⚔️ Silah Satıcısı Borgath - [Kılıç Al/Sat]");
                Console.WriteLine("  [4] 🛡️ Zırh Satıcısı - Aethelred - [Zırh Al/Sat]");
                Console.WriteLine("  [5] 🔨 Demirci (Zırh Geliştir) - [Yakında]");
                Console.WriteLine("  [0] ⬅️ Şehir Kapısından Çık (Ana Menü)");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("\n  Nereye gitmek istersin?: ");
                Console.ResetColor();

                string secim = Console.ReadLine();

                switch (secim)
                {
                    case "1":
                        SehirHani(); 
                        break;
                    case "2":
                        MarketSistemi(); 
                        break;
                    case "3":
                        SilahSaticisi();
                        break;
                    case "4":
                        ZırhSaticisi();
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

        #region HAN DİYALOGLARI VE ETKİNLİKLERİ
        public void SehirHani()
        {
            
            string hanciAd = oyuncu.BoranTanindi ? "Boran" : "???";
            string hanciRol = oyuncu.BoranTanindi ? "Hancı" : "İçki Dağıtan Adam";

            NPC hanci = new NPC(hanciAd, hanciRol, new string[] {
               "Gümüşışık eski günlerini arıyor yolcu. Gölgeler her geçen gün daha da uzuyor.",
               "Kuzeydeki mühürlerin zayıfladığını söylüyorlar... Umarım yanılıyorlardır.",
               "Geçen gece ormanda tuhaf ışıklar gördüm. Kadim bir şeyler uyanıyor olabilir."
            });

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--- 🍻 GÜMÜŞIŞIK HANI ---");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Hoş geldin, {oyuncu.Ad}!");
            Console.ResetColor();

            if (!oyuncu.BoranTanindi)
            {
                Console.WriteLine("\nTezgahın arkasında eski, yara izleriyle dolu bir adam duruyor.");
                Console.WriteLine("Seni görünce başıyla hafifçe selam veriyor ama gözlerindeki şüpheyi gizlemiyor.");
            }

            hanci.Konus();

            Console.WriteLine($"\n[Cüzdanın: {oyuncu.Altın} 💰 | Sağlığın: {oyuncu.Can}/{oyuncu.MaksimumCan}]");
            Console.WriteLine("\n  [1] Sıcak bir yemek ve yatak (-25 💰)");

            if (!oyuncu.BoranTanindi)
                Console.WriteLine("  [2] 'Burayı kim işletiyor? Adın ne?'");
            else
                Console.WriteLine("  [2] Hancıya bölgedeki dedikoduları sor");

            Console.WriteLine("  [0] Kapıdan dışarı süzül.");
            Console.Write("\nKararın: ");

            string hancıSecim = Console.ReadLine();

            if (hancıSecim == "1")
            {
                HanciDinlenme(hanci);
            }
            else if (hancıSecim == "2")
            {
                if (!oyuncu.BoranTanindi)
                {
                    BoranTanismaDiyalogu();
                }
                else
                {
                    HanciDiyalog(hanci);
                }
            }
        }

        private void BoranTanismaDiyalogu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Adam elindeki bardağı temizlemeyi bırakıp sana doğru eğiliyor.");
            Console.WriteLine("\n'Adımın bir önemi yoktu, ta ki bu kasaba unutulmaya başlayana kadar.'");
            Console.WriteLine("'Ben Boran. Bu hanın ve bu sırların son bekçisiyim.'");
            Console.WriteLine("'Herkes buraya bir şeylerden kaçmak için gelir, ama kimse nereye gittiğini bilmez.'");
            Console.ResetColor();

            oyuncu.BoranTanindi = true; 
            OyunuKaydet(oyuncu); 

            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[Artık Boran'ı tanıyorsun. Gümüşışık'ta bir dostun var gibi hissediyorsun.]");
            Console.ResetColor();
            Console.WriteLine("\nDevam etmek için bir tuşa bas...");
            Console.ReadKey();
            SehirHani();
        }

        private void HanciDiyalog(NPC npc)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"--- 🍻 {npc.Ad.ToUpper()} İLE SOHBET ---");
            Console.ResetColor();

            Console.WriteLine($"\n[{npc.Ad}]: 'Söyle bakalım evlat, Gümüşışık'ın hangi sırrı uykularını kaçırıyor?'");

            Console.WriteLine("\n  [1] 'Şehir neden bu kadar sessiz?'");
            Console.WriteLine("  [2] 'Kuzeydeki mühürler hakkında ne biliyorsun?'");
            Console.WriteLine("  [3] 'Geçen gece gördüğün o ışıklar da neydi?'");
            Console.WriteLine("  [0] 'Boşver, sadece bakıyordum.'");

            Console.Write("\nSorun: ");
            string sohbetSecim = Console.ReadLine();

            Console.Clear();
            switch (sohbetSecim)
            {
                case "1":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[{npc.Ad}]: 'Sessizlik her zaman huzur demek değildir.\nGümüşışık'ın altındaki kadim zindanlarda bir şeyler nefes alıyor... Kasaba halkı konuşmaya korkuyor.'");
                    Console.ResetColor();
                    break;
                case "2":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[{npc.Ad}]: 'Eskiler, mühürlerin Işığın Elçileri tarafından yerleştirildiğini söyler.\nEğer onlar zayıflarsa, sadece bu kasaba değil, tüm dünya karanlığa gömülür.'");
                    Console.ResetColor();
                    break;
                case "3":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[{npc.Ad}]: 'O ışıklar... Onlar sahipsiz ruhlar değil evlat.\nSisli Dağlar'dan gelen bir çağrı gibiydiler. Sanki birisi -veya bir şey- kayıp olanı geri istiyor.'");
                    Console.ResetColor();
                    break;
                default:
                    SehirHani();
                    return;
            }

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\n[Bu derin bilgiler zihninde yeni kapılar açtı...]");
            Console.ResetColor();
            Console.WriteLine("\nDevam etmek için bir tuşa bas...");
            Console.ReadKey();
            HanciDiyalog(npc); 
        }

        private void HanciDinlenme(NPC hanci)
        {
            if (oyuncu.Altın >= 25)
            {
                oyuncu.Altın -= 25;
                oyuncu.Can = oyuncu.MaksimumCan;

                Console.Clear();
                Console.WriteLine($"\n[{hanci.Ad}]: 'Güzel seçim evlat. Bu çorba seni kendine getirecek.'");
                Console.WriteLine("\nGeceyi hancının anlattığı eski savaş hikayelerini dinleyerek geçiriyorsun...");
                Thread.Sleep(2000);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[!] Sabahın ilk ışıklarıyla uyandın. Yaraların tamamen kapandı!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[{hanci.Ad}]: 'Seni doyurmayı çok isterdim ama ambarım hayır dualarıyla dolmuyor...'");
                Console.ResetColor();
            }
            Console.WriteLine("\nDevam etmek için bir tuşa bas...");
            Console.ReadKey();
        }
        #endregion

        #region MARKET DİYALOGLARI VE ETKİNLİKLERİ
        public void MarketSistemi()
        {
            string gorunenAd = oyuncu.EleraTanindi ? "Elera" : "???";
            string gorunenRol = oyuncu.EleraTanindi ? "Kadim Tüccar" : "Gölgelerdeki Kadın";

            NPC elera = new NPC(gorunenAd, gorunenRol, new string[] {
                "Gözlerin bana birini hatırlatıyor...",
                "Eski dünya hakkında çok az şey kaldı, bu eşyalar onlardan biri."
            });

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"--- ⚖️ {gorunenAd.ToUpper()}'NIN TEZGAHI ---");
            Console.ResetColor();

            if (!oyuncu.EleraTanindi)
            {
                Console.WriteLine("\nTezgahın arkasında, yüzü pelerininin gölgesinde kalmış bir kadın duruyor.");
            }
            else
            {
                Console.WriteLine($"\nElera: 'Yine geldin demek, {oyuncu.Ad}. Tezgahım senin için her zaman açık.'");
            }

            elera.Konus();
            Console.WriteLine($"\n[Cüzdanın: {oyuncu.Altın} 💰]");

            
            Console.WriteLine("\n  [1] ⚔️ Eşya Satın Al");
            Console.WriteLine("  [2] 💰 Elindekileri Sat");

            if (!oyuncu.EleraTanindi)
                Console.WriteLine("  [3] 👤 'Sen de kimsin?'");
            else
                Console.WriteLine("  [3] 📜 Geçmiş Hakkında Konuş");

            Console.WriteLine("  [0] 🚪 Ayrıl");

            Console.Write("\nSeçimin: ");
            string secim = Console.ReadLine();

            switch (secim)
            {
                case "1": EsyaAlimMenusu(elera); break;
                case "2": EsyaSatimMenusu(elera); break;
                case "3":
                    if (!oyuncu.EleraTanindi) EleraTanısmaDiyalogu();
                    else EleraDiyaloglar(elera);
                    break;
                case "0": return;
                default: MarketSistemi(); break;
            }
        }

        private void EleraTanısmaDiyalogu() 
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Adam elindeki bardağı temizlemeyi bırakıp sana doğru eğiliyor.");
            Console.WriteLine("\n'Adımın bir önemi yoktu, ta ki bu kasaba unutulmaya başlayana kadar.'");
            Console.WriteLine("'Ben Boran. Bu hanın ve bu sırların son bekçisiyim.'");
            Console.WriteLine("'Herkes buraya bir şeylerden kaçmak için gelir, ama kimse nereye gittiğini bilmez.'");
            Console.ResetColor();

            oyuncu.EleraTanindi = true;
            OyunuKaydet(oyuncu);

            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[Artık Elera'yı tanıyorsun. Gümüşışık'ta keşfedeceğin daha çok şey var.]");
            Console.ResetColor();
            Console.WriteLine("\nDevam etmek için bir tuşa bas...");
            Console.ReadKey();
            MarketSistemi();
        }

        private void EleraDiyaloglar(NPC elera)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"--- 📜 {elera.Ad.ToUpper()} İLE KADİM SOHBET ---");
            Console.ResetColor();

            Console.WriteLine($"\n[{elera.Ad}]: 'Sormak istediğin şeylerin bedeli altından daha ağırdır çocuk. Ne bilmek istiyorsun?'");

            Console.WriteLine("\n  [1] 'Sisli Dağlar'daki kütüphaneye ne oldu?'");
            Console.WriteLine("  [2] 'Neden hancıyla aranızda bir gerginlik var?'");
            Console.WriteLine("  [3] 'Karanlık gerçekten geri mi dönüyor?'");
            Console.WriteLine("  [0] 'Sadece bakıyordum.'");

            Console.Write("\nSorun: ");
            string sohbetSecim = Console.ReadLine();

            Console.Clear();
            switch (sohbetSecim)
            {
                case "1":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[{elera.Ad}]: 'Kütüphane yanmadı çocuk, o kütüphane infaz edildi. \nİçindeki sırlar birilerinin uykusunu kaçırıyordu. Kurtarabildiğim tek şey bu parşömenler...'");
                    Console.ResetColor();
                    break;
                case "2":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[{elera.Ad}]: 'Boran iyi bir adamdır ama fazla korkak. Gümüşışık'ın altındaki hapishaneyi unutmak istiyor. \nBen ise o hapishanenin anahtarının neden dövüldüğünü hatırlıyorum.'");
                    Console.ResetColor();
                    break;
                case "3":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[{elera.Ad}]: 'Karanlık hiç gitmedi ki... Sadece güneşin doğmasını bekleyen bir gölge gibi köşesinde saklandı. \nVe şimdi güneş batıyor.'");
                    Console.ResetColor();
                    break;
                default: MarketSistemi(); return;
            }

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\n[Bu derin bilgiler zihninde yeni kapılar açtı...]");
            Console.ResetColor();
            Console.WriteLine("\nDevam etmek için bir tuşa bas...");
            Console.ReadKey();
            EleraDiyaloglar(elera);
        }

        private void EsyaAlimMenusu(NPC elera)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"--- 🧪 {elera.Ad.ToUpper()}'NIN SİHRİ ---");
            Console.ResetColor();

            Console.WriteLine($"\n[Cüzdanın: {oyuncu.Altın} 💰]");
            Console.WriteLine("\n  [1] [🧪] Küçük Can İksiri (Birim: 15 💰)");
            Console.WriteLine("  [2] [🧪] Kadim İksir      (Birim: 30 💰)");
            Console.WriteLine("  [3] [🧪] Dev İksiri       (Birim: 55 💰)");
            Console.WriteLine("  [0] [🔙] Vazgeç");

            Console.Write($"\n{elera.Ad}: 'Hangi iksirden ne kadar istersin?': ");
            string alimSecim = Console.ReadLine();

            switch (alimSecim)
            {
                case "1":
                    SatinAl(elera, "Küçük Can İksiri", 15, Nadirlik.Common, 25, "Şifası bol olsun.");
                    break;
                case "2":
                    SatinAl(elera, "Kadim İksir", 30, Nadirlik.Rare, 50, "Kadim güçler seninle.");
                    break;
                case "3":
                    SatinAl(elera, "Dev İksiri", 55, Nadirlik.Epic, 100, "Dikkat et, çok güçlüdür!");
                    break;
                case "0": MarketSistemi(); return;
                default: EsyaAlimMenusu(elera); break;
            }
        }

        private void SatinAl(NPC elera, string esyaAdi, int birimFiyat, Nadirlik nadirlik, int etki, string eleraYorumu)
        {
            // Miktarı oyuncuya soruyoruz
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"\n[{elera.Ad}]: Kaç adet {esyaAdi} almak istersin? (Maksimum 200): ");
            Console.ResetColor();

            // Geçerli bir sayı girilip girilmediğini kontrol ediyoruz
            if (!int.TryParse(Console.ReadLine(), out int alinacakMiktar) || alinacakMiktar <= 0)
            {
                Console.WriteLine("\n[!] Geçersiz bir miktar girdin.");
                Console.ReadKey();
                MarketSistemi();
                return;
            }

            // Girdi 200'den büyükse 200'e sabitliyoruz
            if (alinacakMiktar > 200) alinacakMiktar = 200;

            int toplamFiyat = birimFiyat * alinacakMiktar;

            // 1. Altın Kontrolü (Toplam fiyat üzerinden)
            if (oyuncu.Altın < toplamFiyat)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[{elera.Ad}]: '{alinacakMiktar} adet için {toplamFiyat} altına ihtiyacın var. Sende {oyuncu.Altın} var.'");
                Console.ResetColor();
                Console.ReadKey();
                MarketSistemi();
                return;
            }

            // 2. Envanter ve Stack Kontrolü
            var mevcutStack = oyuncu.Envanter.Find(x => x.Ad == esyaAdi && x.Miktar < x.MaksimumStack);

            if (mevcutStack != null)
            {
                // Mevcut yığının kapasitesini kontrol et (200'ü geçmesin)
                int eklenebilir = mevcutStack.MaksimumStack - mevcutStack.Miktar;
                int gercekEkleme = Math.Min(alinacakMiktar, eklenebilir);

                mevcutStack.Miktar += gercekEkleme;
                oyuncu.Altın -= (birimFiyat * gercekEkleme);
            }
            else if (oyuncu.Envanter.Count < 20)
            {
                // Envanterde yoksa yeni slot aç ve girilen miktarı ata
                oyuncu.Envanter.Add(new Oge(esyaAdi, nadirlik, OgeTuru.Tuketilebilir, etki, miktar: alinacakMiktar));
                oyuncu.Altın -= toplamFiyat;
            }
            else
            {
                Console.WriteLine("\n[!] Çantan tamamen dolu!");
                Console.ReadKey();
                MarketSistemi();
                return;
            }

            // 3. Başarı Ekranı
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("--- 📦 SATIN ALIM BAŞARILI ---");
            Console.ResetColor();
            Console.WriteLine($"\n[{elera.Ad}]: '{eleraYorumu}'");

            int toplamEnvanter = oyuncu.Envanter.Where(x => x.Ad == esyaAdi).Sum(x => x.Miktar);
            Console.WriteLine($"\n [!] {alinacakMiktar} adet {esyaAdi} alındı!");
            Console.WriteLine($" [📦] Çantandaki Toplam: x{toplamEnvanter}");
            Console.WriteLine($" [📉] Kalan Altın: {oyuncu.Altın} 💰");

            OyunuKaydet(oyuncu);
            Console.WriteLine("\nDevam etmek için bir tuşa bas...");
            Console.ReadKey();
            MarketSistemi();
        }

        private int SatisFiyatiHesapla(Oge oge)
        {
            switch (oge.Nadirlik)
            {
                case Nadirlik.Common:
                    return 15;
                case Nadirlik.Uncommon:
                    return 40;
                case Nadirlik.Rare:
                    return 80;
                case Nadirlik.Epic:
                    return 150;
                case Nadirlik.Legendary:
                    return 300;
                case Nadirlik.Mythic:
                    return 600;
                default:
                    return 10;
            }
        }

        private void EsyaSatimMenusu(NPC elera)
        {
            Console.Clear();
            Console.WriteLine($"--- 💰 {elera.Ad.ToUpper()} SATIŞ TEZGAHI ---");
            Console.WriteLine($"[Cüzdanın: {oyuncu.Altın} 💰]\n");

            if (oyuncu.Envanter.Count == 0)
            {
                Console.WriteLine("Çantan bomboş...");
                Console.ReadKey();
                MarketSistemi();
                return;
            }

            for (int i = 0; i < oyuncu.Envanter.Count; i++)
            {
                var oge = oyuncu.Envanter[i];
                int fiyat = SatisFiyatiHesapla(oge);

                
                Console.WriteLine($"  [{i + 1}] {oge.Ad.PadRight(20)} (x{oge.Miktar}) -> {fiyat} 💰");
            }

            Console.Write("\nSatmak istediğin eşyanın numarası: ");
            string secimInput = Console.ReadLine();

            if (int.TryParse(secimInput, out int secim) && secim > 0 && secim <= oyuncu.Envanter.Count)
            {
                var satilanOge = oyuncu.Envanter[secim - 1];
                int kazanc = SatisFiyatiHesapla(satilanOge);

                Console.Clear();

                
                if (satilanOge.Nadirlik >= Nadirlik.Epic)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[{elera.Ad} Gözleri Fal Taşı Gibi Açılarak]: 'Bu... Bu gerçek olamaz! {satilanOge.Ad} efsanelerde anlatılırdı.'");
                    Console.WriteLine("'Bunu dükkanımın en kıymetli köşesine koyacağım. Al şu altınları, fazlasını bile hak ediyorsun.'");
                }
                else if (satilanOge.Nadirlik == Nadirlik.Rare)
                {
                    Console.WriteLine($"[{elera.Ad}]: 'Güzel bir parça. Gümüşışık'ta böyle temiz işçilik az bulunur.'");
                }
                else
                {
                    Console.WriteLine($"[{elera.Ad}]: 'Sıradan bir iş ama işime yarar. Al bakalım.'");
                }

                
                oyuncu.Altın += kazanc;
                oyuncu.Envanter.RemoveAt(secim - 1);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n[💰] {satilanOge.Ad} ({satilanOge.Nadirlik}) satıldı!");
                Console.WriteLine($"[➕] Hesaba Eklenen: {kazanc} Altın");
                Console.ResetColor();
                Console.WriteLine($"[💳] Yeni Bakiyen: {oyuncu.Altın} Altın");

                
                OyunuKaydet(oyuncu);

                Console.WriteLine("\n  [1] Başka bir şey sat");
                Console.WriteLine("  [0] Market ana menüsüne dön");
                Console.Write("\nKararın: ");

                string devamSecim = Console.ReadLine();

                if (devamSecim == "1")
                {
                    EsyaSatimMenusu(elera); 
                }
                else
                {
                    MarketSistemi(); 
                }
            }
            else if (secimInput == "0")
            {
                MarketSistemi(); 
            }
            else
            {
                Console.WriteLine("\n[!] Geçersiz seçim. Menüye dönülüyor...");
                Thread.Sleep(1500);
                MarketSistemi();
            }
        }
        #endregion

        #region SİLAH SATICISI VE ETKİNLİKLERİ 
        public void SilahSaticisi()
        {
            
            string gorunenAd = oyuncu.SaticiTanisildiMi ? "Borgath" : "Asık Suratlı Dev";
            string gorunenRol = oyuncu.SaticiTanisildiMi ? "Usta Silah Dövmecisi" : "Demir Yığını Arkasındaki Adam";

            NPC borgath = new NPC(gorunenAd, gorunenRol, new string[] {
               "Çelik yalan söylemez evlat, sadece insanlar söyler.",
               "Bu kılıçların her birinde bir askerin son nefesi var..."
            });

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"--- ⚒️ {gorunenAd.ToUpper()}'IN CEPHANESİ ---");
            Console.ResetColor();

            if (!oyuncu.SaticiTanisildiMi)
            {
                Console.WriteLine("\nTezgahın arkasında, kolları yanık izleriyle dolu, devasa bir adam zırh parçalarını ayıklıyor.");
            }
            else
            {
                Console.WriteLine($"\nBorgath: 'Yine mi sen? Umarım kılıcını kırmamışsındır, {oyuncu.Ad}. Çeliğe iyi bakmazsan o da seni korumaz.'");
            }

            borgath.Konus();
            Console.WriteLine($"\n[Cüzdanın: {oyuncu.Altın} 💰]");

            Console.WriteLine("\n  [1] ⚔️ Yeni Silah Satın Al");
            Console.WriteLine("  [2] 💰 Eski Ekipmanlarını Sat");

            if (!oyuncu.SaticiTanisildiMi)
                Console.WriteLine("  [3] 👤 'Burada ne yapıyorsun?' (Tanış)");
            else
                Console.WriteLine("  [3] 📜 Cephe Hikayelerini Dinle");

            Console.WriteLine("  [0] 🚪 Ayrıl");

            Console.Write("\nSeçimin: ");
            string secim = Console.ReadLine();

            switch (secim)
            {
                case "1": SilahAlimMenusu(borgath); break;
                case "2": EkipmanSatimMenusu(borgath); break;
                case "3":
                    if (!oyuncu.SaticiTanisildiMi) BorgathTanismaDiyalogu();
                    else BorgathHikayeleri(borgath);
                    break;
                case "0": return;
                default: SilahSaticisi(); break;
            }
        }

        private void BorgathTanismaDiyalogu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Adam elindeki ağır çekici tezgaha bırakıyor, ses dükkanda yankılanıyor.");
            Console.WriteLine("\n'Benim adım Borgath. Bir zamanlar Krallık Muhafızları'nın baş demircisiydim.'");
            Console.WriteLine("'Ama o parlak zırhların içindeki adamların nasıl çürüdüğünü gördüm ve buraya geldim.'");
            Console.WriteLine("'Burada sadece çelikle konuşurum. Eğer dürüst bir savaşçıysan, sana en iyi işçiliğimi sunarım.'");
            Console.ResetColor();

            oyuncu.SaticiTanisildiMi = true;
            OyunuKaydet(oyuncu);

            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n[Artık Borgath'ı tanıyorsun. Sana olan saygısı biraz arttı.]");
            Console.ResetColor();
            Console.WriteLine("\nDevam etmek için bir tuşa bas...");
            Console.ReadKey();
            SilahSaticisi   ();
        }

        private void BorgathHikayeleri(NPC borgath)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"--- 📜 {borgath.Ad.ToUpper()} İLE CEPHE SOHBETİ ---");
            Console.ResetColor();

            Console.WriteLine($"\n[{borgath.Ad}]: 'Savaş hakkında anlatılan masalları boş ver. Gerçekler daha paslıdır. Ne bilmek istiyorsun?'");

            Console.WriteLine("\n  [1] 'Neden ordudan ayrıldın?'");
            Console.WriteLine("  [2] 'Gümüşışık'ın surları neden bu kadar zayıf?'");
            Console.WriteLine("  [3] 'Hiç efsanevi bir silah dövdün mü?'");
            Console.WriteLine("  [4] 'Karanlık güçler çeliğe zarar verebilir mi?'");
            Console.WriteLine("  [0] 'Geri dön.'");

            Console.Write("\nSorun: ");
            string sohbetSecim = Console.ReadLine();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            switch (sohbetSecim)
            {
                case "1":
                    Console.WriteLine($"[{borgath.Ad}]: 'Komutanlar benden daha keskin kılıçlar istiyordu, masumları daha hızlı kesmek için. \nO gün çekicimi onların değil, adaletin tarafına vurmaya karar verdim.'");
                    break;
                case "2":
                    Console.WriteLine($"[{borgath.Ad}]: 'Çünkü surları taş değil, korku tutar evlat. Buradaki insanlar canavarlardan o kadar korkuyor ki, \nsurları onarmaya cesaretleri bile yok. Ama benim demirim o surlardan daha sağlamdır.'");
                    break;
                case "3":
                    Console.WriteLine($"[{borgath.Ad}]: 'Bir keresinde güneş ışığını emen bir kalkan dövmüştüm... Ama o şimdi bir hainin elinde. \nArtık sadece hak edenlere efsanevi şeyler yapıyorum.'");
                    break;
                case "4":
                    Console.WriteLine($"[{borgath.Ad}]: 'Karanlık çeliği eritmez, onu yozlaştırır. Eğer kılıcının soğuduğunu ve ağırlaştığını hissedersen, \nbil ki ruhuna bir şeyler sızıyordur. Öyle anlarda ateşten korkma.'");
                    break;
                default: SilahSaticisi(); return;
            }
            Console.ResetColor();

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\n[Borgath'ın tecrübeleri savaşma azmini artırdı...]");
            Console.ResetColor();
            Console.WriteLine("\nDevam etmek için bir tuşa bas...");
            Console.ReadKey();
            BorgathHikayeleri(borgath);
        }

        private void SilahAlimMenusu(NPC borgath)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"--- ⚔️ {borgath.Ad.ToUpper()}'IN CEPHANESİ ---");
            Console.ResetColor();

            Console.WriteLine($"\n[Cüzdanın: {oyuncu.Altın} 💰]");
            Console.WriteLine("\n  [1] [🗡️] Talim Kılıcı     (Hasar +10)  - 50 💰");
            Console.WriteLine("  [2] [🪓] Savaş Baltası    (Hasar +22)  - 120 💰");
            Console.WriteLine("  [3] [🗡️] Gümüş Engerek    (Hasar +45)  - 300 💰");
            Console.WriteLine("  [0] [🔙] Vazgeç");

            Console.Write($"\n{borgath.Ad}: 'Hangi çelikle kaderini yazacaksın?' : ");
            string alimSecim = Console.ReadLine();

            switch (alimSecim)
            {
                case "1":
                    SatinAlSilah(borgath, "Talim Kılıcı", 50, 7, "Yeni başlayanlar için iyidir, dengesi yerinde.");
                    break;
                case "2":
                    SatinAlSilah(borgath, "Savaş Baltası", 120, 12, "Ağır bir silahtır, vurduğun yerde ot bitmez!");
                    break;
                case "3":
                    SatinAlSilah(borgath, "Gümüş Engerek", 300, 18, "Bunu dövmek için üç gece uyumadım. En iyi işçiliğim budur.");
                    break;
                case "0": SilahSaticisi(); return;
            }
        }

        private void SatinAlSilah(NPC borgath, string silahAdi, int fiyat, int hasar, string borgathYorumu)
        {
            if (oyuncu.Altın >= fiyat)
            {
                oyuncu.Altın -= fiyat;
                oyuncu.Envanter.Add(new Oge(silahAdi, Nadirlik.Rare, OgeTuru.Silah, hasar));

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("--- ✔️ BORGATH ONAYI VERDİ ---");
                Console.ResetColor();

                Console.WriteLine($"\n[{borgath.Ad}]: '{borgathYorumu}'");
                Console.WriteLine("\n-------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($" [!] {silahAdi.ToUpper()} envanterine eklendi.");
                Console.ResetColor();
                Console.WriteLine($" [📉] Kalan Altın: {oyuncu.Altın} 💰");
                Console.WriteLine("-------------------------------------------");

                OyunuKaydet(oyuncu);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[{borgath.Ad}]: 'Bedava çelik ancak mezarda olur çocuk. Altın getir!'");
                Console.ResetColor();
            }

            Console.WriteLine("\nDevam etmek için bir tuşa bas...");
            Console.ReadKey();
            SilahSaticisi();
        }

        private void EkipmanSatimMenusu(NPC borgath) 
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("--- 💰 EŞYA SATIŞ ---");
            Console.ResetColor();

            if (oyuncu.Envanter == null || oyuncu.Envanter.Count == 0)
            {
                Console.WriteLine("\nBorgath: 'Heyben bomboş, beni oyalama.'");
                Console.ReadKey();
                SilahSaticisi();
                return;
            }

            for (int i = 0; i < oyuncu.Envanter.Count; i++)
            {
                int fiyat = SatisFiyatiHesapla(oyuncu.Envanter[i]);
                Console.WriteLine($"  [{i + 1}] {oyuncu.Envanter[i].Ad} -> {fiyat} 💰");
            }

            Console.Write("\nSatılacak eşya (0 İptal): ");
            if (int.TryParse(Console.ReadLine(), out int secim) && secim > 0 && secim <= oyuncu.Envanter.Count)
            {
                var satilan = oyuncu.Envanter[secim - 1];
                int kazanc = SatisFiyatiHesapla(satilan);

                oyuncu.Altın += kazanc;
                oyuncu.Envanter.RemoveAt(secim - 1);

                Console.WriteLine($"\n{satilan.Ad} satıldı. +{kazanc} Altın kazandın.");
                OyunuKaydet(oyuncu);
            }

            Console.WriteLine("\nDevam etmek için bir tuşa bas...");
            Console.ReadKey();
            SilahSaticisi();
        }
        #endregion
        public void ZırhSaticisi()
        {
            string gorünenad = oyuncu.AethelredTanindi ? "Aethelred" : "Hırslı Demirci";
            string gorunenRol = oyuncu.AethelredTanindi ? "Kadim Zırh Ustası" : "Demirci";

            NPC aethelred = new NPC(gorünenad, gorunenRol, new string[] {
               "Çelik sadece bedeni korur, ruhunu koruyacak olan sensin.",
                "Gümüşışık'ın düşüşü zırhlardaki çatlaktan başladı...",
                "Dövülen her çelik bir çığlık atar, duyabiliyor musun?"
            });

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"--- 🛡️ {gorünenad.ToUpper()}'IN OCAĞI ---");
            Console.ResetColor();

            if (!oyuncu.AethelredTanindi)
                Console.WriteLine("\nKıvılcımların arasında devasa bir siluet, elindeki çekici ritmik bir hırsla örse vuruyor.");
            else
                Console.WriteLine($"\nAethelred: 'Ocağımın sıcaklığı bile dışarıdaki o soğuk karanlığı unutturamıyor, {oyuncu.Ad}. Ne arıyorsun?'");

            aethelred.Konus();
            Console.WriteLine($"\n[Cüzdanın: {oyuncu.Altın} 💰]");
            Console.WriteLine("\n  [1] 🛡️ Zırh Tezgâhına Bak (Satın Al)");
            Console.WriteLine("  [2] 💰 Ganimetlerini Erit (Eski Eşyaları Sat)");

            if (!oyuncu.AethelredTanindi)
                Console.WriteLine("  [3] 👤 'Burada ne yapıyorsun?' (Tanış)");
            else
                Console.WriteLine("  [3] 📜 Eski Hikayeler ve Savaş Teorileri");

            Console.WriteLine("  [0] 🚪 Ayrıl");
            Console.Write("\nSeçimin: ");
            string secim = Console.ReadLine();

            switch(secim)
            {
                case "1": ZırhAlimMenusu(aethelred); break;
                case "2": ZırhSatimMenusu(aethelred); break;
                case "3":
                  if (!oyuncu.AethelredTanindi) AethelredTanismaDiyalogu();
                  else AethelredHikayeler(aethelred);
                break;
                case "0": return;
                  default: ZırhSaticisi(); break;
            }
        }

        public void AethelredTanismaDiyalogu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Adam, dövme masasından başını kaldırarak sana bakıyor, gözlerinde bir kıvılcım var.");
            Console.WriteLine("\n'Adım Aethelred. Bu zırh dükkânının sahibiyim ve savaşın acımasız gerçeklerini bilen bir zırh ustasıyım.'");
            Console.WriteLine("'Bu dünyada sadece güçlü kalabilenler hayatta kalır. Ben de bu yüzden en sağlam zırhları yaparım.'");
            Console.WriteLine("'Eğer benimle çalışmak istiyorsan, sadece altın değil, aynı zamanda cesaret ve kararlılık da getirmen gerekir.'");
            Console.ResetColor();
            oyuncu.AethelredTanindi = true;
            OyunuKaydet(oyuncu);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n[Artık Aethelred'i tanıyorsun. Sana olan saygısı biraz arttı.]");
            Console.ResetColor();
            Console.WriteLine("\nDevam etmek için bir tuşa bas...");
            Console.ReadKey();
            ZırhSaticisi();
        }

        public void AethelredHikayeler(NPC aet)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"--- 📜 {aet.Ad.ToUpper()} İLE KADİM SOHBET ---");
            Console.ResetColor();
            Console.WriteLine($"\n[Aethelred]: 'Çelikten fazlasını duymak istiyorsun demek... Sor, ama cevaplar her zaman huzur getirmez.'");

            Console.WriteLine("\n  [1] 'Neden sadece zırh yapıyorsun?' (Zırh Teorisi)");
            Console.WriteLine("  [2] 'Gümüşışık'ın imza mühürleri ne anlama geliyor?' (Gizli Bilgi)");
            Console.WriteLine("  [3] 'Hiç delinemeyen bir zırh var mı?' (Efsane)");
            Console.WriteLine("  [0] 'Geri dön.'");

            Console.Write("\nSorun: ");
            string sohbetSecim = Console.ReadLine();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            switch (sohbetSecim)
            {
                case "1":
                    Console.WriteLine($"[{aet.Ad}]: 'Saldırı geçicidir evlat. Kılıç sallayan her el bir gün yorulur. \nTeorim şu: Savaşları en çok vuran değil, en son ayakta kalan kazanır.'");
                    Console.WriteLine("\n* Teorik Bilgi: Zırh puanın, canından (HP) daha değerlidir çünkü her darbede canının azalmasını engeller.*");
                    break;
                case "2":
                    Console.WriteLine($"[{aet.Ad}]: 'Gümüşışık 2026 mühürü... Onu sadece gerçek ustalar kullanır. \nEfsaneye göre bu mühür, zırhın içine hapsedilmiş bir koruyucu ruhun imzasıdır.'");
                    Console.WriteLine("\n* Teorik Bilgi: Mühürlü zırhlar, kritik darbeleri %10 oranında tamamen savuşturabilir.*");
                    break;
                case "3":
                    Console.WriteLine($"[{aet.Ad}]: 'Vardı. \"Göktaşı Zırhı\". Onu babam dövmüştü. \nFakat o zırhı giyen kişi, bir süre sonra zırhın ağırlığıyla değil, içindeki karanlığın yüküyle taşa dönüştü.'");
                    break;
                default: ZırhSaticisi(); return;
            }

            Console.ResetColor();
            Console.WriteLine("\nDevam etmek için bir tuşa bas...");
            Console.ReadKey();
            AethelredHikayeler(aet);
        }

        public void ZırhAlimMenusu(NPC aet)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("--- 🛡️ AETHELRED'İN TEZGÂHI ---");
            Console.ResetColor();
            Console.WriteLine($"[Cüzdanın: {oyuncu.Altın} 💰]\n");

            Console.WriteLine("  [1] Deri Yelek           (Def: 3) - 25 💰");
            Console.WriteLine("  [2] Zincir Zırh          (Def: 6) - 50 💰");
            Console.WriteLine("  [3] Şövalye Plaka Zırhı  (Def: 9) - 75 💰");
            Console.WriteLine("  [0] Vazgeç");

            Console.Write("\nSeçimin: ");
            string s = Console.ReadLine();

            
            string isim = ""; int fiyat = 0; int def = 0; Nadirlik nadirlik = Nadirlik.Common; string yorum = "";

            switch (s)
            {
                case "1": isim = "Deri Yelek"; fiyat = 25; def = 3; nadirlik = Nadirlik.Common; yorum = "Hafif ama seni bir süre idare eder."; break;
                case "2": isim = "Zincir Zırh"; fiyat = 50; def = 6; nadirlik = Nadirlik.Common; yorum = "Okçulara karşı iyi bir koruma."; break;
                case "3": isim = "Şövalye Plaka Zırhı"; fiyat = 75; def = 9; nadirlik = Nadirlik.Rare; yorum = "Gerçek bir kaleye dönüştün!"; break;
                case "0": ZırhSaticisi(); return;
                default: ZırhAlimMenusu(aet); return;
            }

            
            Console.WriteLine($"\n[Aethelred]: '{isim} mi? {fiyat} altına el sıkışalım mı?'");
            Console.Write("[E] Evet / [Any] Vazgeç: ");

            if (Console.ReadLine().ToUpper() == "E")
            {
                
                SatinAl(aet, isim, fiyat, nadirlik, def, yorum);

                
                if (oyuncu.Altın >= 0) 
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("========================================");
                    Console.WriteLine($"   🛡️ YENİ EKİPMAN: {isim.ToUpper()} ");
                    Console.WriteLine("========================================");
                    Console.ResetColor();

                    Console.WriteLine($"\n [!] Savunma Artışı: +{def}");
                    Console.WriteLine($" [!] Kalan Altın: {oyuncu.Altın} 💰");
                    Console.WriteLine($"\n[Aethelred]: '{yorum}'");

                    Console.WriteLine("\nDevam etmek için bir tuşa bas...");
                    Console.ReadKey();
                    ZırhSaticisi();
                }
            }
            else
            {
                Console.WriteLine("\n[Aethelred]: 'Kararsızlık iyi bir zırhtan daha ağırdır.'");
                Thread.Sleep(1000);
                ZırhAlimMenusu(aet);
            }
        }

        public void ZırhSatimMenusu(NPC aet)
        {
            Console.Clear();
            Console.WriteLine("--- 💰 ESKİ EKİPMANLARINI ERİT ---");
            Console.WriteLine("Aethelred eşyalarını değerinin yarısına satın alır.\n");

            if (oyuncu.Envanter.Count == 0)
            {
                Console.WriteLine("Satacak bir şeyin yok.");
                Console.ReadKey();
                ZırhSaticisi();
                return;
            }

            for (int i = 0; i < oyuncu.Envanter.Count; i++)
            {
                int satisFiyati = 50; 
                Console.WriteLine($" [{i + 1}] {oyuncu.Envanter[i].Ad} - {satisFiyati} 💰");
            }

            Console.WriteLine(" [0] Geri Dön");
            Console.Write("\nSatmak istediğin eşya no: ");
            if (int.TryParse(Console.ReadLine(), out int secim) && secim > 0 && secim <= oyuncu.Envanter.Count)
            {
                var oge = oyuncu.Envanter[secim - 1];
                oyuncu.Altın += 50; // Değeri dinamik yapabilirsin
                oyuncu.Envanter.Remove(oge);
                Console.WriteLine($"\n{oge.Ad} satıldı! +50 Altın kazandın.");
                OyunuKaydet(oyuncu);
            }

            Console.ReadKey();
            ZırhSaticisi();
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

                SavasBaslat(oyuncu, secilenBolge);
            }

        } // Çalışıyor.

       

    }
}
