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
                "GENEL_COMMON", //Tüm Common Canavarlar Bu itemleri düşürecek
                new List<Oge>
                {
                    new Oge("Küçük Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 25),
                    new Oge("Orta Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 50),
                    new Oge("Büyük Can İkisiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 100),
                    new Oge("Demir Kılıç", Nadirlik.Common, OgeTuru.Silah, 5),
                    new Oge("Derinlik Zırhı", Nadirlik.Common, OgeTuru.Zirh, 3),
                    new Oge("Hilal Kılıcı", Nadirlik.Common, OgeTuru.Silah, 7),
                    new Oge("Zincir Zırh", Nadirlik.Common, OgeTuru.Zirh, 5),
                    new Oge("Azap Kılıcı", Nadirlik.Common, OgeTuru.Silah, 10),
                    new Oge("Kara Zırh", Nadirlik.Common, OgeTuru.Zirh, 7),
                    new Oge("Uzun Kılıç", Nadirlik.Common, OgeTuru.Silah, 8),
                    new Oge("Kısa Kılıç", Nadirlik.Common, OgeTuru.Zirh, 6),


                }
            },
            {
                "GENEL_RARE", //Tüm Rare Canavarlar Bu itemleri düşürecek
                new List<Oge>
                {
                    new Oge("Keskin Çelik Kılıç", Nadirlik.Rare, OgeTuru.Silah, 12),
                    new Oge("Gümüş Kaplama Zırh", Nadirlik.Rare, OgeTuru.Zirh, 8),
                    new Oge("Büyük Şifa İksiri", Nadirlik.Rare, OgeTuru.Tuketilebilir, 40),
                    new Oge("Güçlendirilmiş Kalkan", Nadirlik.Rare, OgeTuru.Zirh, 10)
                }
            },
            {
                "GENEL_BOSS", // Tüm Boss Canavarlar Bu itemleri düşürecek
                new List<Oge>
                {
                    new Oge("Ejderha Kılıcı", Nadirlik.Epic, OgeTuru.Silah, 20),
                    new Oge("Ejderha Zırhı", Nadirlik.Epic, OgeTuru.Zirh, 15),
                    new Oge("Büyük Can İksiri", Nadirlik.Rare, OgeTuru.Tuketilebilir, 50),
                    new Oge("Büyük Şifa İksiri", Nadirlik.Rare, OgeTuru.Tuketilebilir, 40)
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
