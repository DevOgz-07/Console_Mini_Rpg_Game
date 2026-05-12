using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Oyun
{
    internal static class LootManager
    {
        private static Random random = new Random();


        private static Dictionary<string, List<Oge>> LootTables = new Dictionary<string, List<Oge>>
{
    {
        "GENEL_COMMON",
        new List<Oge>
        {
            // Format: Ad, Nadirlik, Tür, EtkiDegeri(Min), MaxEtkiDegeri(Max), GerekenSeviye
            new Oge("Küçük Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 25, 25, 1),
            new Oge("Demir Kılıç", Nadirlik.Common, OgeTuru.Silah, 5, 9, 1),
            new Oge("Derinlik Zırhı", Nadirlik.Common, OgeTuru.Zirh, 3, 3, 1),
            new Oge("Hilal Kılıcı", Nadirlik.Common, OgeTuru.Silah, 7, 12, 3),
            new Oge("Zincir Zırh", Nadirlik.Common, OgeTuru.Zirh, 5, 5, 4),
            new Oge("Uzun Kılıç", Nadirlik.Common, OgeTuru.Silah, 8, 14, 5),
            new Oge("Kısa Kılıç", Nadirlik.Common, OgeTuru.Silah, 6, 10, 2) 
        }
    },
    {
        "GENEL_RARE",
        new List<Oge>
        {
            new Oge("Keskin Çelik Kılıç", Nadirlik.Rare, OgeTuru.Silah, 12, 18, 8),
            new Oge("Gümüş Kaplama Zırh", Nadirlik.Rare, OgeTuru.Zirh, 8, 8, 10),
            new Oge("Güçlendirilmiş Kalkan", Nadirlik.Rare, OgeTuru.Zirh, 10, 10, 7)
        }
    },
    {
        "GENEL_BOSS",
        new List<Oge>
        {
            new Oge("Ejderha Kılıcı", Nadirlik.Epic, OgeTuru.Silah, 20, 32, 15),
            new Oge("Ejderha Zırhı", Nadirlik.Epic, OgeTuru.Zirh, 15, 15, 15)
        }
    },
};



        public static List<Oge> LootDusur(Canavar canavar)
        {
            switch (canavar.Turu)
            {
                case CanavarTuru.CommonMonster:
                    return GenelLootGetir("GENEL_COMMON");

                case CanavarTuru.RareMonster:
                    return GenelLootGetir("GENEL_RARE");

                case CanavarTuru.Boss:
                    
                    return BossLootDusur(canavar.LootTableId);

                default:
                    return new List<Oge>();
            }
        }

        
        private static List<Oge> GenelLootGetir(string poolKey)
        {
            List<Oge> dusenler = new List<Oge>();

            if (LootTables.ContainsKey(poolKey))
            {
                var tablo = LootTables[poolKey];
                
                dusenler.Add(tablo[random.Next(tablo.Count)]);
            }

            return dusenler;
        }

        private static List<Oge> BossLootDusur(string lootTableKey)
        {
            List<Oge> dusenLootlar = new List<Oge>();

           
            if (!LootTables.ContainsKey(lootTableKey))
            {
                return GenelLootGetir("GENEL_RARE");
            }

            var tablo = LootTables[lootTableKey];
            var kopyaTablo = new List<Oge>(tablo);

            
            for (int i = 0; i < 2 && kopyaTablo.Count > 0; i++)
            {
                int index = random.Next(kopyaTablo.Count);
                dusenLootlar.Add(kopyaTablo[index]);
                kopyaTablo.RemoveAt(index);
            }

            return dusenLootlar;
        }
    }
}
