using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Oyun
{
    internal class Karakter
    {
        public string Ad { get; set; }
        public int Can { get; set; }
        public int MaksimumCan { get; set; }
        public int SaldiriGucu { get; set; }
        public int Savunma { get; set; }
        public int Tecrube { get; set; } // Toplam biriken EXP (Asla sıfırlanmaz)
        public int Seviye { get; set; }
        
        public List<Oge> Envanter { get; set; }
        public Oge DonanimliSilah { get; set; }

        public int YetenekPuani { get; set; } = 0;
        public int HP_Stat { get; set; }
        public int STR_Stat { get; set; }
        public int DEX_Stat { get; set; }

        public Karakter(string ad, int baslangicCan, int baslangicSaldiri)
        {
            Ad = ad;
            MaksimumCan = baslangicCan;
            Can = baslangicCan;
            SaldiriGucu = baslangicSaldiri;
            Savunma = 0;
            Tecrube = 0;
            Seviye = 1;
            Envanter = new List<Oge>();
            DonanimliSilah = null;
            HP_Stat = 1;
            STR_Stat = 1;
            DEX_Stat = 1;
        }


        public int SonrakiSeviyeIcinGerekenToplamEXP()
        {
            
            if (Seviye == 1) return 100;

           
            return 300 * (int)Math.Pow(2, Seviye - 2);
        }

        public int MevcutSeviyeBaslangicEXP()
        {
            
            return 0;
        }

        public void TecrubeKazan(int miktar)
        {
            Tecrube += miktar;
            Console.WriteLine($"\n[+] {miktar} tecrübe puanı kazandınız.");

            
            while (Tecrube >= SonrakiSeviyeIcinGerekenToplamEXP())
            {
                int gereken = SonrakiSeviyeIcinGerekenToplamEXP();
                Tecrube -= gereken; 
                SeviyeAtla();
            }
        }

        public void SeviyeAtla()
        {
            Seviye++;
            YetenekPuani += 1;
            Can = MaksimumCan; 

            Console.WriteLine($"\n*** TEBRİKLER! Seviye Atladınız: {Seviye} ***");
            Console.WriteLine("1 Yeni Yetenek Puanı Kazandınız!");
        }


        public string GetEXPBar()
        {
            int ust = SonrakiSeviyeIcinGerekenToplamEXP();
            int alt = MevcutSeviyeBaslangicEXP(); 

           
            double oran = (double)(Tecrube - alt) / (ust - alt);

            if (oran < 0) oran = 0;
            if (oran > 1) oran = 1;

            int barGenisligi = 20;
            int dolu = (int)(oran * barGenisligi);

            
            string bar = new string('█', dolu) + new string('░', barGenisligi - dolu);
            return $"[{bar}] %{(int)(oran * 100)}";
        }

        public void EnvanteriGoster()
        {
            Console.WriteLine("\n--- Envanter ---");
            if (Envanter.Count == 0)
            {
                Console.WriteLine("Envanteriniz Boş");
                return;
            }
            for (int i = 0; i < Envanter.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                Envanter[i].BilgileriGoster();
            }
            Console.WriteLine("-----------------\n");
        }

        public void OgeKullan(int index)
        {
            if (index < 0 || index >= Envanter.Count)
            {
                Console.WriteLine("Geçersiz öğe numarası.");
                return;
            }

            Oge secilenOge = Envanter[index];

            if (secilenOge.Tur == OgeTuru.Tuketilebilir)
            {
                Can += secilenOge.EtkiDegeri;
                if (Can > MaksimumCan) Can = MaksimumCan;
                Console.WriteLine($"{secilenOge.Ad} kullandınız. Güncel Can: {Can}");
                Envanter.RemoveAt(index);
            }
            else if (secilenOge.Tur == OgeTuru.Silah)
            {
                if (DonanimliSilah != null)
                {
                    Envanter.Add(DonanimliSilah);
                    SaldiriGucu -= DonanimliSilah.EtkiDegeri;
                    Console.WriteLine($"{DonanimliSilah.Ad} çıkarıldı.");
                }
                DonanimliSilah = secilenOge;
                SaldiriGucu += DonanimliSilah.EtkiDegeri;
                Envanter.RemoveAt(index);
                Console.WriteLine($"{DonanimliSilah.Ad} kuşandınız! Toplam Saldırı Gücü: {SaldiriGucu}");
            }
        }

        public bool HayattaMi() => Can > 0;
    }
}