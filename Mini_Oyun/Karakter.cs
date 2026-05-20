using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mini_Oyun
{
    public class Karakter
    {
        // Temel Kimlik
        public string Ad { get; set; }
        public string Sifre { get; set; }

        // Durum Değerleri
        public int Can { get; set; }
        public int MaksimumCan { get; set; } = 100;
        public int Altın { get; set; } = 50;

        // Seviye ve Tecrübe
        public int Seviye { get; set; } = 1;
        public int Tecrube { get; set; } = 0;
        public int ToplamTecrube { get; set; } = 0;
        public int YetenekPuani { get; set; } = 0;

        // İstatistikler 
        public int HP_Stat { get; set; } = 1;
        public int STR_Stat { get; set; } = 1;
        public int DEX_Stat { get; set; } = 1;
        public int KritikSans { get; set; } = 15;
        public int SaldiriGucu { get; set; }
        public int Savunma { get; set; }

        // Ekipmanlar
        public List<Oge> Envanter { get; set; } = new List<Oge>();
        public Oge DonanimliSilah { get; set; }
        public Oge MevcutZirh { get; set; }

        // NPC İlişkileri 
        public bool BoranTanindi { get; set; }
        public bool EleraTanindi { get; set; }
        public bool SaticiTanisildiMi { get; set; }
        public int SaticiKonusmaSayisi { get; set; }
        public bool AethelredTanindi { get; set; }

        // Toplam Güç Hesaplamaları 
        public int ToplamSaldiriGucu => (25 + (STR_Stat * 2)) + (DonanimliSilah?.EtkiDegeri ?? 0);
        public int ToplamSavunma => (1 + (DEX_Stat * 1)) + (MevcutZirh?.EtkiDegeri ?? 0);

        public  Karakter() { }

        public Karakter(string ad)
        {
            Ad = ad;
            Seviye = 1;
            Tecrube = 0;
            Altın = 50;
            KritikSans = 15;
            HP_Stat = 1;
            STR_Stat = 1;
            DEX_Stat = 1;
            Envanter = new List<Oge>();
            BaslangicEkipmanlariniVer();
            Can = MaksimumCan;
            DonanimliSilah = null;
            MevcutZirh = null;
        }
        private void BaslangicEkipmanlariniVer()
        {
            Envanter.Add(new Oge("Demir Kılıç", Nadirlik.Common, OgeTuru.Silah, 5));
            Envanter.Add(new Oge("Küçük Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 25, miktar: 2));
        }
        public bool HayattaMi() => Can > 0;
        public static class LevelManager
        {
            public static int GerekenEXP(int seviye)
            {
                return seviye == 1 ? 100 : 300 * (int)Math.Pow(2, seviye - 2);
            }
            public static int SonrakiSeviyeIcinGerekenToplamEXP(Karakter karakter)
            {
                return GerekenEXP(karakter.Seviye);
            }
            public static string GetEXPBar(Karakter karakter) 
            {
                int barGenisligi = 20;
                int gerekenEXP = GerekenEXP(karakter.Seviye);

                float oran = (float)karakter.Tecrube / gerekenEXP;
                int doluKisim = (int)(oran * barGenisligi);
                int bosKisim = barGenisligi - doluKisim;

                return "[" + new string('■', doluKisim) + new string('-', bosKisim) + "] " +
                       "%" + (int)(oran * 100);
            }

            public static void TecrubeKazan(Karakter karakter, int miktar)
            {
                karakter.Tecrube += miktar;
                karakter.ToplamTecrube += miktar;
                Console.WriteLine($"\n[+] {miktar} tecrübe kazandınız.");

                while (karakter.Tecrube >= GerekenEXP(karakter.Seviye))
                {
                    karakter.Tecrube -= GerekenEXP(karakter.Seviye);
                    SeviyeAtla(karakter);
                }
            }

            private static void SeviyeAtla(Karakter karakter)
            {
                karakter.Seviye++;
                karakter.YetenekPuani++;
                karakter.Can = karakter.MaksimumCan;
                Console.WriteLine($"\n*** TEBRİKLER! Seviye: {karakter.Seviye} ***");
            }
        }
        public static class InventoryManager
        {
            public static void OgeKullan(Karakter karakter, Oge secilenOge)
            {
                if (secilenOge == null) return;

                switch (secilenOge.Tur)
                {
                    case OgeTuru.Tuketilebilir:
                        IksirKullan(karakter, secilenOge);
                        break;
                    case OgeTuru.Silah:
                        SilahKusan(karakter, secilenOge);
                        break;
                    case OgeTuru.Zirh:
                        ZirhKusan(karakter, secilenOge);
                        break;
                }
            }

            private static void IksirKullan(Karakter k, Oge iksir)
            {
                if (k.Can >= k.MaksimumCan)
                {
                    Console.WriteLine("\n[!] Canınız zaten dolu!");
                    return;
                }
                k.Can = Math.Min(k.Can + iksir.EtkiDegeri, k.MaksimumCan);
                iksir.Miktar--;
                if (iksir.Miktar <= 0) k.Envanter.Remove(iksir);
                Console.WriteLine($"[🧪] {iksir.Ad} kullanıldı. Güncel Can: {k.Can}");
            }

            private static void SilahKusan(Karakter k, Oge yeniSilah)
            {
                if (k.DonanimliSilah != null) k.Envanter.Add(k.DonanimliSilah);
                k.DonanimliSilah = yeniSilah;
                k.Envanter.Remove(yeniSilah);
                Console.WriteLine($"[⚔️] {yeniSilah.Ad} kuşandınız!");
            }

            private static void ZirhKusan(Karakter k, Oge yeniZirh)
            {
                if (k.MevcutZirh != null) k.Envanter.Add(k.MevcutZirh);
                k.MevcutZirh = yeniZirh;
                k.Envanter.Remove(yeniZirh);
                Console.WriteLine($"[🛡️] {yeniZirh.Ad} kuşandınız!");
            }
        }
    }
}