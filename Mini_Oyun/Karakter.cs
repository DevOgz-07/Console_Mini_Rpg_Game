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
        public int Can {  get; set; }
        public int MaksimumCan { get; set; }
        public int SaldiriGucu { get; set; }
        public int Tecrube { get; set; }
        public int Seviye { get; set; }
        public List<Oge> Envanter { get; set; }
        public Oge DonanimliSilah { get; set; }

        public Karakter(string ad, int can, int saldiriGucu)
        {
            Ad = ad;
            MaksimumCan = can;
            Can = can;
            SaldiriGucu = saldiriGucu;
            Tecrube = 0;
            Seviye = 1;
            Envanter = new List<Oge>();
            DonanimliSilah = null;

        }

        public void SeviyeAtla()
        {
            Seviye++;
            MaksimumCan += 20; // Karakter bir seviye aldıkça karakterin seviye bazlı yükseleceği can miktarı.
            Can = MaksimumCan; // Canı Tamamlar.
            SaldiriGucu += 5; // Seviye arttıkça artan saldırı değeri.
            Console.WriteLine($"\n***Tebrikler! {Seviye}. seviyeye Ulaştınız! ***");
            Console.WriteLine($"Yeni Can: {Can}, Yeni Saldırı Gücü: {SaldiriGucu + (DonanimliSilah?.EtkiDegeri ?? 0)}");
        }

        public void TecrubeKazan(int kazanilanTecrube)
        {
            Tecrube += kazanilanTecrube;
            int sonrakiSeviyeTecrube = Seviye * 50; // Sonraki seviye için gereken tecrübe

            while (Tecrube >= sonrakiSeviyeTecrube)
            {
                Tecrube -= sonrakiSeviyeTecrube;
                SeviyeAtla();
                sonrakiSeviyeTecrube = Seviye * 150; // Yeni seviye için yeni tecrübe gereksinimi
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
                Console.WriteLine($"{secilenOge.Ad} kullandınız. Canınız {secilenOge.EtkiDegeri} arttı. Güncel Can: {Can}");
                Envanter.RemoveAt(index); 
            }
            else if (secilenOge.Tur == OgeTuru.Silah)
            {
                if (DonanimliSilah != null)
                {
                    Envanter.Add(DonanimliSilah); 
                    SaldiriGucu -= DonanimliSilah.EtkiDegeri; // Eski silahın etkisini kaldırır.
                    Console.WriteLine($"{DonanimliSilah.Ad} çıkarıldı.");
                }
                DonanimliSilah = secilenOge;
                SaldiriGucu += DonanimliSilah.EtkiDegeri;
                Envanter.RemoveAt(index); // Silahı envanterden çıkartır.
                Console.WriteLine($"{DonanimliSilah.Ad} kuşandınız! Saldırı gücünüz {DonanimliSilah.EtkiDegeri} arttı. Güncel Saldırı Gücü: {SaldiriGucu}");
            }
            else if (secilenOge.Tur == OgeTuru.Zirh)
            {
                Console.WriteLine("Zırh kuşatma özelliği henüz eklenmedi.");
                // Zırhın Savunma Etkisi.
            }
        }

        public bool HayattaMi()
        {
            return Can > 0;
        }
    }
}
