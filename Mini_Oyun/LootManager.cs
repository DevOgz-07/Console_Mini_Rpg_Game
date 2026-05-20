using System;
using System.Collections.Generic;
using System.Linq;

namespace Mini_Oyun
{
    internal static class LootManager
    {
        private static readonly Random _random = new Random();

        private static readonly Dictionary<string, List<LootEntry>> LootTables = new Dictionary<string, List<LootEntry>>();

        static LootManager()
        {
            SeedLootTables();
        }

        private static void SeedLootTables()
        {
            // COMMON HAVUZU
            LootTables.Add("GENEL_COMMON", new List<LootEntry>
            {
                new LootEntry(new Oge("Küçük Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 25, 25, 1), 60),
                new LootEntry(new Oge("Demir Kılıç", Nadirlik.Common, OgeTuru.Silah, 5, 9, 1), 30),
                new LootEntry(new Oge("Derinlik Zırhı", Nadirlik.Common, OgeTuru.Zirh, 3, 3, 1), 25),
                new LootEntry(new Oge("Kısa Kılıç", Nadirlik.Common, OgeTuru.Silah, 6, 10, 2), 40),
                new LootEntry(new Oge("Hilal Kılıcı", Nadirlik.Common, OgeTuru.Silah, 7, 12, 3), 20),
                new LootEntry(new Oge("Zincir Zırh", Nadirlik.Common, OgeTuru.Zirh, 5, 5, 4), 15),
                new LootEntry(new Oge("Uzun Kılıç", Nadirlik.Common, OgeTuru.Silah, 8, 14, 5), 10)
            });

            // RARE HAVUZU
            LootTables.Add("GENEL_RARE", new List<LootEntry>
            {
                new LootEntry(new Oge("Keskin Çelik Kılıç", Nadirlik.Rare, OgeTuru.Silah, 12, 18, 8), 25),
                new LootEntry(new Oge("Gümüş Kaplama Zırh", Nadirlik.Rare, OgeTuru.Zirh, 8, 8, 10), 15),
                new LootEntry(new Oge("Güçlendirilmiş Kalkan", Nadirlik.Rare, OgeTuru.Zirh, 10, 10, 7), 20)
            });

            // BOSS HAVUZU
            LootTables.Add("GENEL_BOSS", new List<LootEntry>
            {
                new LootEntry(new Oge("Ejderha Kılıcı", Nadirlik.Epic, OgeTuru.Silah, 20, 32, 15), 40),
                new LootEntry(new Oge("Ejderha Zırhı", Nadirlik.Epic, OgeTuru.Zirh, 15, 15, 15), 35)
            });
        }

        public static List<Oge> LootDusur(Canavar canavar)
        {
            if (canavar == null) return new List<Oge>();

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

            if (LootTables.TryGetValue(poolKey, out var tablo))
            {
                var karisikTablo = tablo.OrderBy(_ => _random.Next()).ToList();

                foreach (var entry in karisikTablo)
                {
                    if (_random.NextDouble() <= entry.GetDropRate())
                    {
                        dusenler.Add(entry.Oge);
                        break;
                    }
                }
            }

            return dusenler;
        }

        private static List<Oge> BossLootDusur(string lootTableKey)
        {
            List<Oge> dusenLootlar = new List<Oge>();

            string activeKey = (!string.IsNullOrEmpty(lootTableKey) && LootTables.ContainsKey(lootTableKey))
                ? lootTableKey
                : "GENEL_RARE";

            var tablo = LootTables[activeKey];

            var kopyaTablo = tablo.OrderByDescending(e => e.DropSans * _random.NextDouble()).ToList();

            int limitselSecimSayisi = Math.Min(2, kopyaTablo.Count);

            for (int i = 0; i < limitselSecimSayisi; i++)
            {
                dusenLootlar.Add(kopyaTablo[i].Oge);
            }

            return dusenLootlar;
        }
    }
}