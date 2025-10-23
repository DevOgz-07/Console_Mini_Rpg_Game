using System;
using System.Collections.Generic;
using System.Linq;

namespace Mini_Oyun
{
    public enum CanavarTuru
    {
        CommonMonster,       //20 adet
        RareMonster,         //15 adet
        EpicMonster,         //10 adet
        LegendaryMonster,    //8 adet
        MythicMonster,       //5 adet
        Boss,                //İsteğe bağlı yükseltilebilir.
    }

    internal class Canavar_ve_Boss
    {
        public class Canavar
        {
            public string Ad { get; set; }
            public int Can { get; set; }
            public int MaksimumCan { get; set; }
            public int SaldiriGucu { get; set; }
            public int VerilenTecrube { get; set; }
            public CanavarTuru Turu { get; set; }
            protected List<Oge> PotansiyelDusurulecekOgeler { get; set; }
            protected Random random = new Random();

            public Canavar(string ad, int can, int saldiriGucu, int verilenTecrube, CanavarTuru turu)
            {
                Ad = ad;
                MaksimumCan = can;
                Can = can;
                SaldiriGucu = saldiriGucu;
                VerilenTecrube = verilenTecrube;
                Turu = turu; 
                PotansiyelDusurulecekOgeler = new List<Oge>();
            }

            public virtual Oge OgeDusur()
            {
                if (PotansiyelDusurulecekOgeler.Count == 0) return null;

                int sans = random.Next(1, 101);

                foreach (var oge in PotansiyelDusurulecekOgeler.OrderByDescending(o => (int)o.Nadirlik))
                {
                    switch (oge.Nadirlik)
                    {
                        case Nadirlik.Mythic:
                            if (sans <= 1) return oge;
                            break;
                        case Nadirlik.Legendary:
                            if (sans <= 3) return oge;
                            break;
                        case Nadirlik.Epic:
                            if (sans <= 7) return oge;
                            break;
                        case Nadirlik.Rare:
                            if (sans <= 15) return oge;
                            break;
                        case Nadirlik.Uncommon:
                            if (sans <= 30) return oge;
                            break;
                        case Nadirlik.Common:
                            if (sans <= 60) return oge;
                            break;
                    }
                }
                return null;
            }

            public bool HayattaMi() => Can > 0;
        }

        public class Boss : Canavar
        {
            public Boss(string ad, int can, int saldiriGucu, int verilenTecrube, CanavarTuru turu)
                : base(ad, can, saldiriGucu, verilenTecrube, turu)
            {
                PotansiyelDusurulecekOgeler.Add(new Oge("Ejderha Pençesi", Nadirlik.Epic, OgeTuru.Silah, 15));
                PotansiyelDusurulecekOgeler.Add(new Oge("Efsanevi Zırh", Nadirlik.Legendary, OgeTuru.Zirh, 20));
                PotansiyelDusurulecekOgeler.Add(new Oge("Kozmik Cevher", Nadirlik.Mythic, OgeTuru.Tuketilebilir, 100));
            }
        }

        

        public class Goblin : Canavar
        {
            public Goblin() : base("Goblin", 20, 5, 10, CanavarTuru.CommonMonster)
            {
                PotansiyelDusurulecekOgeler.Add(new Oge("Paslı Kılıç", Nadirlik.Common, OgeTuru.Silah, 2));
                PotansiyelDusurulecekOgeler.Add(new Oge("Yırtık Pırtık Pelerin", Nadirlik.Common, OgeTuru.Zirh, 1));
                PotansiyelDusurulecekOgeler.Add(new Oge("Küçük Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 20));
            }
        }

        public class Iskelet : Canavar
        {
            public Iskelet() : base("İskelet", 25, 7, 15, CanavarTuru.CommonMonster)
            {
                PotansiyelDusurulecekOgeler.Add(new Oge("Kemik Hançer", Nadirlik.Uncommon, OgeTuru.Silah, 4));
                PotansiyelDusurulecekOgeler.Add(new Oge("İskelet Zırhı", Nadirlik.Uncommon, OgeTuru.Zirh, 3));
                PotansiyelDusurulecekOgeler.Add(new Oge("Orta Can İksiri", Nadirlik.Uncommon, OgeTuru.Tuketilebilir, 35));
            }
        }

        public class Orc : Canavar
        {
            public Orc() : base("Orc", 35, 10, 20, CanavarTuru.CommonMonster)
            {
                PotansiyelDusurulecekOgeler.Add(new Oge("Demir Balta", Nadirlik.Rare, OgeTuru.Silah, 7));
                PotansiyelDusurulecekOgeler.Add(new Oge("Deri Zırh", Nadirlik.Rare, OgeTuru.Zirh, 5));
                PotansiyelDusurulecekOgeler.Add(new Oge("Büyük Can İksiri", Nadirlik.Rare, OgeTuru.Tuketilebilir, 50));
            }
        }
        public class Yarasa : Canavar
        {
            public Yarasa() : base("Orc", 35, 10, 20, CanavarTuru.CommonMonster)
            {
                PotansiyelDusurulecekOgeler.Add(new Oge("Kanat Kılıc", Nadirlik.Common, OgeTuru.Silah, 7));
                PotansiyelDusurulecekOgeler.Add(new Oge("Kanat Zırh", Nadirlik.Rare, OgeTuru.Zirh, 5));
                PotansiyelDusurulecekOgeler.Add(new Oge("Küçük Can İksiri", Nadirlik.Rare, OgeTuru.Tuketilebilir, 50));
            }
        }

        

        public class Ejderha : Boss
        {
            public Ejderha() : base("Ejderha", 400, 50, 200, CanavarTuru.Boss)
            {
                PotansiyelDusurulecekOgeler.Add(new Oge("Ejderha Nefesi Asası", Nadirlik.Legendary, OgeTuru.Silah, 25));
                PotansiyelDusurulecekOgeler.Add(new Oge("Ejderha Pulu Zırhı", Nadirlik.Legendary, OgeTuru.Zirh, 15));
                PotansiyelDusurulecekOgeler.Add(new Oge("Antik Kalp", Nadirlik.Mythic, OgeTuru.Tuketilebilir, 200));
            }
        }

        public class OrcReisi : Boss
        {
            public OrcReisi() : base("Orc Reisi", 250, 30, 150, CanavarTuru.Boss)
            {
                PotansiyelDusurulecekOgeler.Add(new Oge("Orc Reisinin Baltası", Nadirlik.Legendary, OgeTuru.Silah, 25));
                PotansiyelDusurulecekOgeler.Add(new Oge("Orc Reisi Zırhı", Nadirlik.Legendary, OgeTuru.Zirh, 15));
                PotansiyelDusurulecekOgeler.Add(new Oge("Orc Kalbi", Nadirlik.Mythic, OgeTuru.Tuketilebilir, 200));
            }
        }
    }
}
