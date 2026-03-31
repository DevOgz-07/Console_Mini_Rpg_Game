using System.Collections.Generic;


namespace Mini_Oyun
{
    public class Bolge
    {
        public string Ad { get; set; }
        public int MinSeviye { get; set; }
        public int MaxSeviye { get; set; }

        public int OnerilenSeviye { get; set; }
        public List<Canavar> Canavarlar { get; set; }
        public List<Boss> Bosslar { get; set; }

        public Bolge(string ad, int minLvl, int maxLvl, List<Canavar> canavarlar, List<Boss> bosslar = null)
        {
            Ad = ad;
            MinSeviye = minLvl;
            MaxSeviye = maxLvl;
            Canavarlar = canavarlar;
            Bosslar = bosslar ?? new List<Boss>();
        }

    } // Bölge Ataması

    internal static class CanavarVeritabani
    {
        public static List<Bolge> GumusIsikKoyuBolgeleri = new List<Bolge>
{
    // 1. BÖLGE (Lvl 1-5)
    new Bolge("Huzurlu Çayır", 1, 5, new List<Canavar> {
        new Canavar("Zorven", 22, 5, 100, CanavarTuru.CommonMonster, "LT_Zorven"),
        new Canavar("Zeltar", 23, 6, 11, CanavarTuru.CommonMonster, "LT_Zeltar"),
        new Canavar("Jexor", 23, 6, 12, CanavarTuru.CommonMonster, "LT_Jexor")
    }),

    // 2. BÖLGE (Lvl 3-7)
    new Bolge("Gölgeli Orman", 3, 7, new List<Canavar> {
        new Canavar("Mirgon", 24, 6, 11, CanavarTuru.CommonMonster, "LT_Mirgon"),
        new Canavar("Lumeris", 24, 6, 12, CanavarTuru.CommonMonster, "LT_Lumeris"),
        new Canavar("Strenuer", 25, 6, 12, CanavarTuru.CommonMonster, "LT_Strenuer")
    }),

    // 3. BÖLGE (Lvl 5-10)
    new Bolge("Uğultulu Mağara", 5, 10, new List<Canavar> {
        new Canavar("Fenrax", 25, 7, 12, CanavarTuru.CommonMonster, "LT_Fenrax"),
        new Canavar("Talvex", 26, 7, 13, CanavarTuru.CommonMonster, "LT_Talvex"),
        new Canavar("Xarmin", 26, 7, 13, CanavarTuru.CommonMonster, "LT_Xarmin")
    }),

    // 4. BÖLGE (Lvl 7-12)
    new Bolge("Sisli Bataklık", 7, 12, new List<Canavar> {
        new Canavar("Vornik", 27, 7, 14, CanavarTuru.CommonMonster, "LT_Vornik"),
        new Canavar("Nexil", 27, 8, 14, CanavarTuru.CommonMonster, "LT_Nexil"),
        new Canavar("Trilvox", 27, 8, 15, CanavarTuru.CommonMonster, "LT_Trilvox")
    }),

    // 5. BÖLGE (Lvl 9-15)
    new Bolge("Lanetli Harabeler", 9, 15, new List<Canavar> {
        new Canavar("Velitor", 28, 7, 14, CanavarTuru.CommonMonster, "LT_Velitor"),
        new Canavar("Orvex", 28, 7, 14, CanavarTuru.CommonMonster, "LT_Orvex"),
        new Canavar("Hexlar", 29, 8, 15, CanavarTuru.CommonMonster, "LT_Hexlar")
    }),

    // 6. BÖLGE (Lvl 12-17)
    new Bolge("Kızıl Kanyon", 12, 17, new List<Canavar> {
        new Canavar("Vorquin", 29, 8, 15, CanavarTuru.CommonMonster, "LT_Vorquin"),
        new Canavar("Kraytix", 30, 8, 16, CanavarTuru.CommonMonster, "LT_Kraytix"),
        new Canavar("Kryonid", 30, 9, 17, CanavarTuru.CommonMonster, "LT_Kryonid")
    }),

    // 7. BÖLGE (Lvl 15-20) - En güçlü canavarlar
    new Bolge("Ejderha Tepesi", 15, 20, new List<Canavar> {
        new Canavar("Plentor", 31, 9, 17, CanavarTuru.CommonMonster, "LT_Plentor"),
        new Canavar("Torvax", 32, 9, 18, CanavarTuru.CommonMonster, "LT_Torvax")
    })
};


        public static List<Canavar> TumCommonCanavarlar { get; private set; } = new List<Canavar>
       {
            new Canavar("Strenuer", 25, 6, 12, CanavarTuru.CommonMonster, "LT_Strenuer"),
            new Canavar("Velitor", 28, 7, 14, CanavarTuru.CommonMonster, "LT_Velitor"),
            new Canavar("Kraytix", 30, 8, 16, CanavarTuru.CommonMonster, "LT_Kraytix"),
            new Canavar("Zorven", 22, 5, 10, CanavarTuru.CommonMonster, "LT_Zorven"),
            new Canavar("Talvex", 26, 7, 13, CanavarTuru.CommonMonster, "LT_Talvex"),
            new Canavar("Mirgon", 24, 6, 11, CanavarTuru.CommonMonster, "LT_Mirgon"),
            new Canavar("Hexlar", 29, 8, 15, CanavarTuru.CommonMonster, "LT_Hexlar"),
            new Canavar("Vornik", 27, 7, 14, CanavarTuru.CommonMonster, "LT_Vornik"),
            new Canavar("Jexor", 23, 6, 12, CanavarTuru.CommonMonster, "LT_Jexor"),
            new Canavar("Plentor", 31, 9, 17, CanavarTuru.CommonMonster, "LT_Plentor"),
            new Canavar("Xarmin", 26, 7, 13, CanavarTuru.CommonMonster, "LT_Xarmin"),
            new Canavar("Torvax", 32, 9, 18, CanavarTuru.CommonMonster, "LT_Torvax"),
            new Canavar("Lumeris", 24, 6, 12, CanavarTuru.CommonMonster, "LT_Lumeris"),
            new Canavar("Nexil", 27, 8, 14, CanavarTuru.CommonMonster, "LT_Nexil"),
            new Canavar("Vorquin", 29, 8, 15, CanavarTuru.CommonMonster, "LT_Vorquin"),
            new Canavar("Zeltar", 23, 6, 11, CanavarTuru.CommonMonster, "LT_Zeltar"),
            new Canavar("Orvex", 28, 7, 14, CanavarTuru.CommonMonster, "LT_Orvex"),
            new Canavar("Kryonid", 30, 9, 17, CanavarTuru.CommonMonster, "LT_Kryonid"),
            new Canavar("Fenrax", 25, 7, 12, CanavarTuru.CommonMonster, "LT_Fenrax"),
            new Canavar("Trilvox", 27, 8, 15, CanavarTuru.CommonMonster, "LT_Trilvox"),
       };
        public static List<Canavar> TumRareCanavarlar { get; private set; } = new List<Canavar>
        {
        };
        public static List<Canavar> TumEpicCanavarlar { get; private set; } = new List<Canavar>();
        public static List<Canavar> TumLegendaryCanavarlar { get; private set; } = new List<Canavar>();
        public static List<Canavar> TumMythicCanavarlar { get; private set; } = new List<Canavar>();

        public static List<Boss> TumBossCanavarlar { get; private set; } = new List<Boss>
        {
        };



        public static List<Canavar> TumCanavarlar()
        {
            var tumu = new List<Canavar>();
            tumu.AddRange(TumCommonCanavarlar);
            tumu.AddRange(TumRareCanavarlar);
            tumu.AddRange(TumEpicCanavarlar);
            tumu.AddRange(TumLegendaryCanavarlar);
            tumu.AddRange(TumMythicCanavarlar);
            tumu.AddRange(TumBossCanavarlar);


            return tumu;
        }
    }
}
