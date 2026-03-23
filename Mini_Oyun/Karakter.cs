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
        public int Savunma { get; set; } // DEX'ten gelen savunma puanı
        public int Tecrube { get; set; }
        public int Seviye { get; set; }
        public List<Oge> Envanter { get; set; }
        public Oge DonanimliSilah { get; set; }

        // --- Temel Statlar ---
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
            Savunma = 0; // Başlangıç savunması
            Tecrube = 0;
            Seviye = 1;
            Envanter = new List<Oge>();
            DonanimliSilah = null;

            // Statları başlangıçta 1 olarak kabul edelim (veya istediğin değer)
            HP_Stat = 1;
            STR_Stat = 1;
            DEX_Stat = 1;
        }

        public void SeviyeAtla()
        {
            Seviye++;
            YetenekPuani += 1; // Her seviyede 1 puan veriyoruz

            // Seviye atlayınca canı hala tazeleyebiliriz (opsiyonel)
            Can = MaksimumCan;

            Console.WriteLine($"\n*** TEBRİKLER! {Seviye}. Seviyeye Ulaştınız! ***");
            Console.WriteLine("1 Yeni Yetenek Puanı Kazandınız!");
        }

        public void TecrubeKazan(int kazanilanTecrube)
        {
            Tecrube += kazanilanTecrube;
            // Dinamik seviye zorluğu
            int sonrakiSeviyeTecrube = Seviye * 100;

            while (Tecrube >= sonrakiSeviyeTecrube)
            {
                Tecrube -= sonrakiSeviyeTecrube;
                SeviyeAtla();
                sonrakiSeviyeTecrube = Seviye * 100;
            }
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