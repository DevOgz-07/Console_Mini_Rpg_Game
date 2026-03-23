using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Mini_Oyun
{
    internal class Oyun_Motoru
    {
        public class OyunMotoru
        {
            private Karakter oyuncu;
            private List<Canavar> canavarHavuzu; 
            private List<Boss> bossHavuzu; 
            private Random random = new Random();

            public OyunMotoru(Karakter karakter)
            {
                oyuncu = karakter;
                canavarHavuzu = new List<Canavar>();
                bossHavuzu = new List<Boss>();
                CanavarlariOlustur();
                BosslariOlustur();
            }

            private void CanavarlariOlustur()
            {
                canavarHavuzu.AddRange(CanavarVeritabani.TumCommonCanavarlar);
                canavarHavuzu.AddRange(CanavarVeritabani.TumRareCanavarlar);

            }

           

            private void BosslariOlustur()
            {
                canavarHavuzu.AddRange(CanavarVeritabani.TumBossCanavarlar);
            }

            public void OyunuBaslat()
            {
                Console.WriteLine($"\n--- {oyuncu.Ad} maceraya başlıyor! ---");
                Console.WriteLine($"Başlangıç Can: {oyuncu.Can}, Saldırı Gücü: {oyuncu.SaldiriGucu}, Seviye: {oyuncu.Seviye}");

                while (oyuncu.HayattaMi())
                {
                    Console.WriteLine("\nNe yapmak istersiniz?");
                    Console.WriteLine("1. Canavarlarla savaş");
                    Console.WriteLine("2. Envanteri kontrol et ve öğe kullan");
                    Console.WriteLine("3. Stat yükseltme menüsüne git");
                    Console.WriteLine("3. Oyundan çık");

                    string secim = Console.ReadLine();

                    switch (secim)
                    {
                        case "1":
                            SavasBaslat();
                            break;
                        case "2":
                            EnvanterMenusu();
                            break;
                        case "3": // Update
                            StatYukseltmeMenusu();
                            break;
                        case "4":
                            Console.WriteLine("Oyundan çıkılıyor...");
                            return;
                        default:
                            Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
                            break;
                    }
                }

                Console.WriteLine("\n--- Oyun Bitti! ---");
                Console.WriteLine("Karakteriniz öldü. Daha şanslı bir dahaki sefere!");
            }

            private void SavasBaslat()
            {
                Canavar hedef;

                if (random.Next(0, 100) < 15 && bossHavuzu.Any())
                {
                    hedef = bossHavuzu[random.Next(bossHavuzu.Count)];
                    Console.WriteLine($"\nDevasa bir düşman belirdi: {hedef.Ad}!");
                }
                else
                {
                    hedef = canavarHavuzu[random.Next(canavarHavuzu.Count)];
                    Console.WriteLine($"\nBir {hedef.Ad} ile karşılaştınız!");
                }

                hedef.Can = hedef.MaksimumCan;

                Console.WriteLine($"Can: {hedef.Can}, Saldırı Gücü: {hedef.SaldiriGucu}");
                Thread.Sleep(1000);

                while (oyuncu.HayattaMi() && hedef.HayattaMi())
                {
                    Console.WriteLine("\n--- Savaş Devam Ediyor ---");
                    Console.WriteLine(
                        $"Oyuncu Can: {oyuncu.Can}/{oyuncu.MaksimumCan} | " +
                        $"{hedef.Ad} Can: {hedef.Can}/{hedef.MaksimumCan}");

                    Console.WriteLine("1. Saldır");
                    Console.WriteLine("2. İksir kullan");
                    Console.WriteLine("3. Kaç");

                    string secim = Console.ReadLine();

                    // =====================
                    // OYUNCU HAMLESİ
                    // =====================

                    if (secim == "1")
                    {
                        int toplamSaldiri =
                            oyuncu.SaldiriGucu +
                            (oyuncu.DonanimliSilah?.EtkiDegeri ?? 0);

                        int oyuncuHasar =
                            random.Next(toplamSaldiri / 2, toplamSaldiri + 1);

                        hedef.Can -= oyuncuHasar;

                        if (hedef.Can < 0)
                            hedef.Can = 0;

                        Console.WriteLine(
                            $"{oyuncu.Ad}, {hedef.Ad}'a {oyuncuHasar} hasar verdi!");

                        Thread.Sleep(500);

                        // ===== CANAVAR ÖLDÜ MÜ?
                        if (!hedef.HayattaMi())
                        {
                            Console.WriteLine($"\n{hedef.Ad} öldü!");

                            var dusenOgeler = hedef.OgeDusur();

                            if (dusenOgeler != null && dusenOgeler.Count > 0)
                            {
                                Console.WriteLine("Şunları düşürdü:");

                                foreach (var oge in dusenOgeler)
                                {
                                    Console.WriteLine($"- {oge.Ad}");
                                    oyuncu.Envanter.Add(oge);
                                }
                            }

                            Console.WriteLine($"{hedef.VerilenTecrube} tecrübe puanı kazandınız!");

                            oyuncu.TecrubeKazan(hedef.VerilenTecrube);

                            break; 
                        }
                    }
                    else if (secim == "2")
                    {
                        Console.WriteLine("İksir sistemi burada çalışır...");
                        continue; // Tur bitmez
                    }
                    else if (secim == "3")
                    {
                        if (random.Next(100) < 40)
                        {
                            Console.WriteLine("Savaştan kaçtınız!");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Kaçamadınız!");
                            Thread.Sleep(500);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz seçim.");
                        continue;
                    }

                    // =====================
                    // CANAVAR KARŞI SALDIRI
                    // =====================

                    if (hedef.HayattaMi())
                    {
                        int hamHasar;

                        // 1. Hasar Belirleme (Boss ise kendi aralığını, normal ise SaldiriGucu'nu kullanır)
                        if (hedef is Boss boss)
                        {
                            hamHasar = random.Next(boss.MinimumHasari, boss.MaksimumHasari + 1);
                        }
                        else
                        {
                            hamHasar = random.Next(hedef.SaldiriGucu / 2, hedef.SaldiriGucu + 1);
                        }

                        
                        int gercekHasar = Math.Max(0, hamHasar - oyuncu.Savunma);

                        oyuncu.Can -= gercekHasar;

                        if (gercekHasar > 0)
                        {
                            Console.WriteLine($"{hedef.Ad}, size {gercekHasar} hasar verdi! (Savunma ile {hamHasar - gercekHasar} hasar engellendi)");
                        }
                        else
                        {
                            Console.WriteLine($"{hedef.Ad} saldırdı ama savunmanız sayesinde hiç hasar almadınız!");
                        }

                        Thread.Sleep(500);

                        if (!oyuncu.HayattaMi())
                        {
                            Console.WriteLine("Karakteriniz savaşta yenildi!");
                            break;
                        }
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
                while (oyuncu.YetenekPuani > 0)
                {
                    Console.WriteLine($"\n--- STAT YÜKSELTME ---");
                    Console.WriteLine($"Kullanılabilir Puan: {oyuncu.YetenekPuani}");
                    Console.WriteLine($"Mevcut Statlar -> HP: {oyuncu.HP_Stat} | STR: {oyuncu.STR_Stat} | DEX: {oyuncu.DEX_Stat}");
                    Console.WriteLine("Hangi statı yükseltmek istersiniz?");
                    Console.WriteLine("1. HP (+5 Maksimum Can)");
                    Console.WriteLine("2. STR (+2 Saldırı Gücü)");
                    Console.WriteLine("3. DEX (+1 Savunma)");
                    Console.WriteLine("0. Geri Dön");

                    string secim = Console.ReadLine();

                    if (secim == "1")
                    {
                        oyuncu.HP_Stat += 1;
                        oyuncu.MaksimumCan += 5;
                        oyuncu.Can = oyuncu.MaksimumCan; // HP artınca canı da güncelle
                        oyuncu.YetenekPuani -= 1;
                        Console.WriteLine("HP yükseltildi!");
                    }
                    else if (secim == "2")
                    {
                        oyuncu.STR_Stat += 1;
                        oyuncu.SaldiriGucu += 2;
                        oyuncu.YetenekPuani -= 1;
                        Console.WriteLine("STR yükseltildi!");
                    }
                    else if (secim == "3")
                    {
                        oyuncu.DEX_Stat += 1;
                        oyuncu.Savunma += 1;
                        oyuncu.YetenekPuani -= 1;
                        Console.WriteLine("DEX yükseltildi!");
                    }
                    else if (secim == "0")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz seçim!");
                    }
                }
            }
        }
    }
}
