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
                    Console.WriteLine("║                       🏆 ZAFER 🏆                       ║");
                    Console.WriteLine("╚═════════════════════════════════════════════════════════╝");
                    Console.ResetColor();

                    int toplamExp = slotlar.Sum(s => s.VerilenTecrube);
                    int toplamAltin = rnd.Next(45, 101);

                    oyuncu.TecrubeKazan(toplamExp);
                    oyuncu.Altın += toplamAltin;

                    // EXP Bar Hesaplamaları (Mantığın Korundu)
                    int baslangicEXP = oyuncu.MevcutSeviyeBaslangicEXP();
                    int gerekenEXP = oyuncu.SonrakiSeviyeIcinGerekenToplamEXP();
                    int mevcutIlerleme = oyuncu.Tecrube - baslangicEXP;
                    int seviyeIcinGereken = gerekenEXP - baslangicEXP;

                    double yuzde = seviyeIcinGereken > 0 ? (double)mevcutIlerleme / seviyeIcinGereken * 100 : 0;
                    yuzde = Math.Min(100, Math.Max(0, yuzde));

                    Console.WriteLine($"\n⭐ Kazanılan EXP: +{toplamExp}");
                    Console.WriteLine($"💰 Kazanılan Altın: +{toplamAltin}");
                    Console.WriteLine($"📊 Seviye İlerlemesi: [%{(int)yuzde}]");
                    Console.WriteLine(CanBariCiz((int)yuzde, 100)); // EXP Barı için aynı fonksiyonu kullandık

                    // GRUP GANİMETİ MANTIĞI (Aynen Korundu)
                    temizlenenGrupSayisi++;
                    if (temizlenenGrupSayisi % 3 == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\n--- 📦 NADİR GRUP GANİMETLERİ ---");
                        Console.ResetColor();

                        List<Oge> tumPotansiyelLoot = new List<Oge>();
                        foreach (var canavar in slotlar)
                            tumPotansiyelLoot.AddRange(LootManager.LootDusur(canavar));

                        int alinacakMiktar = (rnd.Next(1, 101) <= 2) ? 2 : 1;
                        var secilenGanimetler = tumPotansiyelLoot.OrderByDescending(o => o.Nadirlik).Take(alinacakMiktar).ToList();

                        foreach (var oge in secilenGanimetler)
                        {
                            if (oyuncu.Envanter.Count < 20)
                            {
                                oyuncu.Envanter.Add(oge);
                                Console.ForegroundColor = Oge.NadirlikRengiGetir(oge.Nadirlik);
                                Console.WriteLine($" > {oge.Ad} bulundu! Envantere eklendi.");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine($" > [!] Çanta dolu! {oge.Ad} kayboldu.");
                            }
                            Console.ResetColor();
                        }
                    }

                    if (!oyuncu.Ad.StartsWith("Misafir_")) OyunuKaydet(oyuncu);

                    // SAVAŞ SONRASI SEÇENEKLERİ
                    bool kararVerildi = false;
                    while (!kararVerildi)
                    {
                        Console.WriteLine("\n[1] Keşfe Devam  [2] Şehre Dön  [3] Envanter  [0] Çıkış");
                        Console.Write("Seçiminiz: ");
                        string sSecim = Console.ReadLine();
                        if (sSecim == "1") kararVerildi = true;
                        else if (sSecim == "2") { SehreGit(oyuncu); return; }
                        else if (sSecim == "3") { EnvanterMenusu(oyuncu); }
                        else if (sSecim == "0") return;
                    }
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

        
        public string CanBariCiz(int suAn, int maks)
        {
            int barGenislik = 20;
            int doluKisim = (int)((double)suAn / maks * barGenislik);
            if (doluKisim < 0) doluKisim = 0;

            return "[" + new string('█', doluKisim) + new string('░', barGenislik - doluKisim) + $"] {suAn}/{maks}";
        }



        private void EnvanterMenusu(Karakter karakter)
        {
            while (true)
            {
                Console.Clear();
                // Ekrandaki sıra numarası ile gerçek eşya nesnesini bağlayan sözlük
                Dictionary<int, Oge> secimMap = new Dictionary<int, Oge>();
                int gosterimSirasi = 1;
                int panelGenislik = 58;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("┌" + new string('─', panelGenislik - 10) + " ENVANTER " + new string('─', 2) + "┐");

                // --- SİLAHLAR ---
                YazdirKategori("⚔️  SİLAHLAR", "Silah", ConsoleColor.Yellow, ref gosterimSirasi, secimMap);
                Console.WriteLine("├" + new string('─', panelGenislik) + "┤");

                // --- ZIRHLAR ---
                YazdirKategori("🛡️  ZIRHLAR", "Zirh", ConsoleColor.Green, ref gosterimSirasi, secimMap);
                Console.WriteLine("├" + new string('─', panelGenislik) + "┤");

                // --- İKSİRLER ---
                YazdirKategori("🧪 İKSİRLER", "Tuketilebilir", ConsoleColor.Red, ref gosterimSirasi, secimMap);
                Console.WriteLine("└" + new string('─', panelGenislik) + "┘");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n [0] Geri Dön");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nKullanmak istediğin numara: ");

                if (int.TryParse(Console.ReadLine(), out int secim))
                {
                    if (secim == 0) break;

                    // Sözlükten ekrandaki numaraya karşılık gelen GERÇEK eşyayı bul
                    if (secimMap.ContainsKey(secim))
                    {
                        Oge secilenOge = secimMap[secim];
                        // Artık index ile değil, doğrudan eşya nesnesi ile işlem yapıyoruz
                        karakter.OgeKullanNesneIle(secilenOge);
                    }
                    else
                    {
                        Console.WriteLine("Hatalı seçim!");
                        Thread.Sleep(800);
                    }
                }
            }
        }


        private void YazdirKategori(string baslik, string turFiltresi, ConsoleColor renk, ref int sira, Dictionary<int, Oge> map)
        {
            Console.Write("│ ");
            Console.ForegroundColor = renk;
            
            Console.Write(baslik.PadRight(56));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" │");

            foreach (var oge in oyuncu.Envanter)
            {
                
                if (oge.Tur.ToString().Contains(turFiltresi) || oge.Ad.Contains(turFiltresi))
                {
                    Console.Write("│  "); 
                    Console.ForegroundColor = ConsoleColor.White;

                    
                    string satir = $"[{sira}] {oge.Ad} (+{oge.EtkiDegeri})";

                    
                    Console.Write(satir.PadRight(55));

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" │");

                    map.Add(sira, oge);
                    sira++;
                }
            }
        }

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
                else if (secim == "1" || secim == "2" || secim == "3")
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
                        }
                        else if (secim == "2")
                        {
                            oyuncu.STR_Stat += 1;
                            oyuncu.SaldiriGucu = 25 + (oyuncu.STR_Stat * 2);
                            oyuncu.YetenekPuani -= 1;
                            Console.WriteLine("\n[+] Kasların güçleniyor, vuruşların derinleşiyor! (STR Yükseltildi)");
                        }
                        else if (secim == "3")
                        {
                            oyuncu.DEX_Stat += 1;
                            oyuncu.Savunma = 1 + (oyuncu.DEX_Stat * 1);
                            oyuncu.YetenekPuani -= 1;
                            Console.WriteLine("\n[+] Reflekslerin keskinleşiyor! (DEX Yükseltildi)");
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
            Console.WriteLine($" [SALDIRI GÜCÜ]    : {25 + (oyuncu.STR_Stat * 2)}");
            Console.WriteLine($" [SAVUNMA]         : {1 +  (oyuncu.DEX_Stat * 1)}");
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
            Console.WriteLine($" [STR (Güç)]       : {oyuncu.STR_Stat}  -> (Hasar: {25 + (oyuncu.STR_Stat * 2)})");
            Console.WriteLine($" [DEX (Savunma)]   : {oyuncu.DEX_Stat}  -> (Zırh: {1 + (oyuncu.DEX_Stat * 1)})");


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
            Console.WriteLine($" TOPLAM SALDIRI GÜCÜ : {oyuncu.ToplamSaldiriGucu}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($" TOPLAM SAVUNMA GÜCÜ : {oyuncu.ToplamSavunma}");
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

        public void SehreGit(Karakter karakter) // MARKET SİLAH ZIRH DEMİRCİ YAKINDA EKLENECEK ŞU AN İÇİN HAN VE DİNLENME VAR
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
                Console.WriteLine("  [2] ⚖️   Market (Eşya Al/Sat) - [Yakında]");
                Console.WriteLine("  [3]      Silah Satıcısı - [Yakında]");
                Console.WriteLine("  [4]      Zırh Satıcısı - [Yakında]");
                Console.WriteLine("  [5] 🔨   Demirci (Zırh Geliştir) - [Yakında]");
                Console.WriteLine("  [0] ⬅️    Şehir Kapısından Çık (Ana Menü)");

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

                SavasBaslat(oyuncu, secilenBolge);
            }

        } // Çalışıyor.

       

    }
}
