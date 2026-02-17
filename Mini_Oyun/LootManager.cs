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

        #region CommonMonsters
        private static Dictionary<string, List<Oge>> CommonlootTables =
            new Dictionary<string, List<Oge>>
        {
        {
            "LT_Strenuer",
            new List<Oge>
            {
                new Oge("Hasarlı Lazer Parçası", Nadirlik.Common, OgeTuru.Silah, 3),
                new Oge("Metal Plaka", Nadirlik.Common, OgeTuru.Zirh, 2),
                new Oge("Küçük Enerji İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 15)
            }
        },

        {
            "LT_Velitor",
            new List<Oge>
            {
                new Oge("Velitor Dişi", Nadirlik.Common, OgeTuru.Silah, 4),
                new Oge("Çatlak Zırh", Nadirlik.Common, OgeTuru.Zirh, 2),
                new Oge("Enerji Hücresi", Nadirlik.Common, OgeTuru.Tuketilebilir, 18)
            }
        },

        {
            "LT_Kraytix",
            new List<Oge>
            {
                new Oge("Kraytix Pençesi", Nadirlik.Common, OgeTuru.Silah, 5),
                new Oge("Zırh Parçası", Nadirlik.Common, OgeTuru.Zirh, 3),
                new Oge("Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 20)
            }
        },

        {
            "LT_Zorven",
            new List<Oge>
            {
                new Oge("Zorven Kılıcı", Nadirlik.Common, OgeTuru.Silah, 4),
                new Oge("Zorven Zırhı", Nadirlik.Common, OgeTuru.Zirh, 3),
                new Oge("Can Yenileme İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 20)
            }
        },
        {
            "LT_Talvex",
            new List<Oge>
            {
                new Oge("Talvex Kalkanı", Nadirlik.Common, OgeTuru.Zirh, 4),
                new Oge("Talvex Kılıcı", Nadirlik.Common, OgeTuru.Silah, 5),
                new Oge("Enerji Yenileme İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 18)
            }
        },
        {
            "LT_Mirgon",
            new List<Oge>
            {
                new Oge("Mirgon Kalkanı", Nadirlik.Common, OgeTuru.Zirh, 3),
                new Oge("Mirgon Kılıcı", Nadirlik.Common, OgeTuru.Silah, 4),
                new Oge("Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 20)
            }

        },
        {
            "LT_Hexlar",
            new List<Oge>
            {
                new Oge("Hexlar Zırhı" , Nadirlik.Common, OgeTuru.Zirh, 3),
                new Oge("Hex Kılıcı" , Nadirlik.Common, OgeTuru .Silah, 4),
                new Oge("Can İksiri" , Nadirlik.Common, OgeTuru.Tuketilebilir , 20)


            }
        },
        {
            "LT_Vornik",
            new List<Oge>
            {
                new Oge("Vornik Baltası", Nadirlik.Common, OgeTuru.Silah, 5),
                new Oge("Vornik Deri Zırh", Nadirlik.Common, OgeTuru.Zirh, 3),
                new Oge("Dayanıklılık İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 18)
            }
        },

{
    "LT_Jexor",
    new List<Oge>
    {
        new Oge("Jexor Mızrağı", Nadirlik.Common, OgeTuru.Silah, 4),
        new Oge("Jexor Göğüslük", Nadirlik.Common, OgeTuru.Zirh, 2),
        new Oge("Küçük Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 15)
    }
},

{
    "LT_Plentor",
    new List<Oge>
    {
        new Oge("Plentor Kılıcı", Nadirlik.Common, OgeTuru.Silah, 6),
        new Oge("Plentor Zırh Parçası", Nadirlik.Common, OgeTuru.Zirh, 3),
        new Oge("Enerji İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 20)
    }
},

{
    "LT_Xarmin",
    new List<Oge>
    {
        new Oge("Xarmin Hançeri", Nadirlik.Common, OgeTuru.Silah, 4),
        new Oge("Xarmin Omuzluk", Nadirlik.Common, OgeTuru.Zirh, 3),
        new Oge("Can Yenileme İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 18)
    }
},

{
    "LT_Torvax",
    new List<Oge>
    {
        new Oge("Torvax Baltası", Nadirlik.Common, OgeTuru.Silah, 6),
        new Oge("Torvax Zırhı", Nadirlik.Common, OgeTuru.Zirh, 4),
        new Oge("Büyük Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 25)
    }
},

{
    "LT_Lumeris",
    new List<Oge>
    {
        new Oge("Lumeris Asası", Nadirlik.Common, OgeTuru.Silah, 5),
        new Oge("Lumeris Cübbesi", Nadirlik.Common, OgeTuru.Zirh, 3),
        new Oge("Mana İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 20)
    }
},

{
    "LT_Nexil",
    new List<Oge>
    {
        new Oge("Nexil Kılıcı", Nadirlik.Common, OgeTuru.Silah, 5),
        new Oge("Nexil Zincir Zırh", Nadirlik.Common, OgeTuru.Zirh, 3),
        new Oge("Dayanıklılık İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 18)
    }
},

{
    "LT_Vorquin",
    new List<Oge>
    {
        new Oge("Vorquin Pençesi", Nadirlik.Common, OgeTuru.Silah, 5),
        new Oge("Vorquin Zırh Parçası", Nadirlik.Common, OgeTuru.Zirh, 3),
        new Oge("Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 20)
    }
},

{
    "LT_Zeltar",
    new List<Oge>
    {
        new Oge("Zeltar Kılıcı", Nadirlik.Common, OgeTuru.Silah, 4),
        new Oge("Zeltar Deri Zırh", Nadirlik.Common, OgeTuru.Zirh, 2),
        new Oge("Küçük Enerji İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 15)
    }
},

{
    "LT_Orvex",
    new List<Oge>
    {
        new Oge("Orvex Baltası", Nadirlik.Common, OgeTuru.Silah, 5),
        new Oge("Orvex Zırhı", Nadirlik.Common, OgeTuru.Zirh, 3),
        new Oge("Can Yenileme İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 18)
    }
},

{
    "LT_Kryonid",
    new List<Oge>
    {
        new Oge("Kryonid Kılıcı", Nadirlik.Common, OgeTuru.Silah, 6),
        new Oge("Kryonid Göğüslük", Nadirlik.Common, OgeTuru.Zirh, 4),
        new Oge("Büyük Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 25)
    }
},

{
    "LT_Fenrax",
    new List<Oge>
    {
        new Oge("Fenrax Hançeri", Nadirlik.Common, OgeTuru.Silah, 5),
        new Oge("Fenrax Zırh Parçası", Nadirlik.Common, OgeTuru.Zirh, 3),
        new Oge("Enerji İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 20)
    }
},

{
    "LT_Trilvox",
    new List<Oge>
    {
        new Oge("Trilvox Kılıcı", Nadirlik.Common, OgeTuru.Silah, 5),
        new Oge("Trilvox Zırhı", Nadirlik.Common, OgeTuru.Zirh, 3),
        new Oge("Dayanıklılık İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 20)
    }
},









        };
        #endregion
        #region RareMonsters
        private static Dictionary<string, List<Oge>> RarelootTables =
            new Dictionary<string, List<Oge>>
            {
                {
                  "LT_Db_StrenuerP",
                  new List<Oge>
                  {
                      new Oge("Strenuer Parçalanmış Kılıcı", Nadirlik.Rare, OgeTuru.Silah, 10),
                      new Oge("Strenuer Zırh Parçası", Nadirlik.Rare, OgeTuru.Zirh, 5),
                      new Oge("Büyük Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 30)
                  }
                }
            };
        #endregion
        #region BossMonsters
        private static Dictionary<string, List<Oge>> bossLootTables =
            new Dictionary<string, List<Oge>>
        {
        {
            "LT_Boss_Dreadlord",
            new List<Oge>
            {
                new Oge("Dreadlord Büyük Kılıç", Nadirlik.Rare, OgeTuru.Silah, 15),
                new Oge("Dreadlord Zırhı", Nadirlik.Rare, OgeTuru.Zirh, 12),
                new Oge("Büyük Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 35)
            }
        },
        {
            "LT_Boss_VoidTitan",
            new List<Oge>
            {
                new Oge("Void Titan Baltası", Nadirlik.Rare, OgeTuru.Silah, 18),
                new Oge("Void Zırh Kaplaması", Nadirlik.Rare, OgeTuru.Zirh, 14),
                new Oge("Mega Enerji İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 40)
            }
        },
        {
            "LT_Boss_Bloodfang",
            new List<Oge>
            {
                new Oge("Bloodfang Pençesi", Nadirlik.Rare, OgeTuru.Silah, 17),
                new Oge("Kanlı Zırh Parçası", Nadirlik.Rare, OgeTuru.Zirh, 13),
                new Oge("Efsanevi Can İksiri", Nadirlik.Common, OgeTuru.Tuketilebilir, 45)
            }
        }
        };
        #endregion

        public static List<Oge> LootDusur(Canavar canavar)
        {
            switch (canavar.Turu)
            {
                case CanavarTuru.CommonMonster:
                    return CommonLootDusur(canavar.LootTableId);
                case CanavarTuru.RareMonster:
                    return RareLootDusur(canavar.LootTableId);
                case CanavarTuru.Boss:
                    return BossLootDusur(canavar.LootTableId);
                default:
                    return new List<Oge>();
            }
        }


        private static List<Oge> CommonLootDusur(string lootTableKey)
        {
            List<Oge> dusenLootlar = new List<Oge>();

            if (!CommonlootTables.ContainsKey(lootTableKey))
                return dusenLootlar;

            var tablo = CommonlootTables[lootTableKey];

            dusenLootlar.Add(tablo[random.Next(tablo.Count)]);

            return dusenLootlar;
        }

        private static List<Oge> RareLootDusur(string lootTableKey)
        {
            List<Oge> dusenLootlar = new List<Oge>();

            if (!RarelootTables.ContainsKey(lootTableKey))
                return dusenLootlar;

            var tablo = RarelootTables[lootTableKey];

            dusenLootlar.Add(tablo[random.Next(tablo.Count)]);

            return dusenLootlar;
        }

        public static List<Oge> BossLootDusur(string lootTableKey)
        {
            List<Oge> dusenLootlar = new List<Oge>();

            if (!bossLootTables.ContainsKey(lootTableKey))
                return dusenLootlar;

            var tablo = bossLootTables[lootTableKey];

            // 2 garanti loot (birbirinden farklı)
            var kopyaTablo = new List<Oge>(tablo);

            for (int i = 0; i < 2 && kopyaTablo.Count > 0; i++)
            {
                int index = random.Next(kopyaTablo.Count);
                dusenLootlar.Add(kopyaTablo[index]);
                kopyaTablo.RemoveAt(index);
            }

            // %30 ihtimalle ekstra bir loot
            if (random.Next(100) < 30 && kopyaTablo.Count > 0)
            {
                dusenLootlar.Add(kopyaTablo[random.Next(kopyaTablo.Count)]);
            }

            return dusenLootlar;
        }

    }
}
