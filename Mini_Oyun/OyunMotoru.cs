using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Mini_Oyun.Canavar_ve_Boss;

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
                canavarHavuzu.Add(new Goblin());
                canavarHavuzu.Add(new Iskelet());
                canavarHavuzu.Add(new Orc());
            }

            private void BosslariOlustur()
            {
                bossHavuzu.Add(new Ejderha());
                bossHavuzu.Add(new OrcReisi());
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
                        case "3":
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
                    Console.WriteLine($"Oyuncu Can: {oyuncu.Can}/{oyuncu.MaksimumCan}, Saldırı Gücü: {oyuncu.SaldiriGucu + (oyuncu.DonanimliSilah?.EtkiDegeri ?? 0)} | {hedef.Ad} Can: {hedef.Can}/{hedef.MaksimumCan}");
                    Console.WriteLine("1. Saldır");
                    Console.WriteLine("2. İksir kullan");
                    Console.WriteLine("3. Kaç");

                    string savasSecim = Console.ReadLine();

                    switch (savasSecim)
                    {
                        case "1":
                            
                            int toplamOyuncuSaldirisi = oyuncu.SaldiriGucu + (oyuncu.DonanimliSilah?.EtkiDegeri ?? 0);
                            int oyuncuHasar = random.Next(toplamOyuncuSaldirisi / 2, toplamOyuncuSaldirisi + 1);
                            hedef.Can -= oyuncuHasar;
                            Console.WriteLine($"{oyuncu.Ad}, {hedef.Ad}'a {oyuncuHasar} hasar verdi!");
                            Thread.Sleep(500);
                            break;
                        case "2":
                            
                            var iksirler = oyuncu.Envanter.Where(o => o.Tur == OgeTuru.Tuketilebilir).ToList();
                            if (iksirler.Any())
                            {
                                Console.WriteLine("\n--- Kullanılabilir İksirler ---");
                                for (int i = 0; i < iksirler.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {iksirler[i].Ad} (+{iksirler[i].EtkiDegeri} Can)");
                                }
                                Console.WriteLine("0. Geri dön");
                                Console.Write("Kullanmak istediğiniz iksirin numarasını girin: ");
                                if (int.TryParse(Console.ReadLine(), out int iksirSecimIndex) && iksirSecimIndex > 0 && iksirSecimIndex <= iksirler.Count)
                                {
                                    
                                    Oge kullanilacakIksir = iksirler[iksirSecimIndex - 1];
                                    int gercekIndex = oyuncu.Envanter.IndexOf(kullanilacakIksir);
                                    oyuncu.OgeKullan(gercekIndex);
                                }
                                else if (iksirSecimIndex == 0)
                                {
                                    Console.WriteLine("İksir kullanmaktan vazgeçildi.");
                                }
                                else
                                {
                                    Console.WriteLine("Geçersiz seçim.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Hiç iksiriniz yok.");
                            }
                            Thread.Sleep(500);
                            break;
                        case "3":
                            
                            if (random.Next(0, 100) < 40) 
                            {
                                Console.WriteLine("Savaştan başarıyla kaçtınız!");
                                return; 
                            }
                            else
                            {
                                Console.WriteLine("Kaçamadınız! Savaş devam ediyor.");
                            }
                            Thread.Sleep(500);
                            break;
                        default:
                            Console.WriteLine("Geçersiz seçim. Hamlenizi kaybettiniz.");
                            break;
                    }

                    if (!hedef.HayattaMi())
                    {
                        Console.WriteLine($"{hedef.Ad} yenildi!");
                        oyuncu.TecrubeKazan(hedef.VerilenTecrube);
                        Oge dusenOge = hedef.OgeDusur();
                        if (dusenOge != null)
                        {
                            Console.WriteLine($"{hedef.Ad} öldü ve {dusenOge.Ad} düşürdü!");
                            oyuncu.Envanter.Add(dusenOge);
                        }
                        break; 
                    }

                    
                    int canavarHasar = random.Next(hedef.SaldiriGucu / 2, hedef.SaldiriGucu + 1);
                    oyuncu.Can -= canavarHasar;
                    Console.WriteLine($"{hedef.Ad}, {oyuncu.Ad}'a {canavarHasar} hasar verdi!");
                    Thread.Sleep(500);

                    if (!oyuncu.HayattaMi())
                    {
                        Console.WriteLine("Karakteriniz savaşta yenildi!");
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
        }
    }
}
