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
        public int Tecrube { get; set; }
        public int ToplamTecrube { get; private set; } = 0;
        public int Seviye { get; set; }
        public int YetenekPuani { get; set; } = 0;

        // --- İSTATİSTİKLER ---
        public int HP_Stat { get; set; }
        public int STR_Stat { get; set; }
        public int DEX_Stat { get; set; }
        public int KritikSans => 15;

        // --- DİNAMİK HESAPLANAN ÖZELLİKLER ---
        // Bu kısımlar statlar arttığında otomatik güncellenir.
        public int MaksimumCan { get; set; } = 350;
        public int SaldiriGucu { get; set; } = 25;
        public int Savunma { get; set; } = 1;

        public List<Oge> Envanter { get; set; }
        public Oge DonanimliSilah { get; set; }

        
        public Karakter(string ad)
        {
            Ad = ad;
            Seviye = 1;
            Tecrube = 0;
            HP_Stat = 1;
            STR_Stat = 1;
            DEX_Stat = 1;

            // Başlangıç canı, statlardan hesaplanan MaksimumCan'a eşitlenir.
            Can = MaksimumCan;
            Envanter = new List<Oge>();
            DonanimliSilah = null;
        }

        #region karakter seviye ve tecrube sistemi
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
            ToplamTecrube += miktar; 

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
            int hedef = SonrakiSeviyeIcinGerekenToplamEXP();
            double oran = (double)Tecrube / hedef; 

            if (oran > 1) oran = 1;

            int barGenisligi = 20;
            int dolu = (int)(oran * barGenisligi);

            string bar = new string('█', dolu) + new string('░', barGenisligi - dolu);
            return $"{bar} %{(int)(oran * 100)}";
        }
        #endregion


        #region karakter envanter ve öğe kullanımı
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
        #endregion

        public bool HayattaMi() => Can > 0;
    }
}