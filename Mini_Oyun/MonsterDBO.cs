using System.Collections.Generic;


namespace Mini_Oyun
{
    internal static class CanavarVeritabani
    {

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
            new Canavar("Db_StrenuerP", 40, 10, 20, CanavarTuru.RareMonster, "LT_Db_StrenuerP"),
            new Canavar("Db_VelitorP", 45, 12, 22, CanavarTuru.RareMonster, "LT_Db_VelitorP"),
            new Canavar("Db_KraytixP", 50, 14, 25, CanavarTuru.RareMonster, "LT_Db_KraytixP"),
            new Canavar("Db_ZorvenP", 38, 9, 18, CanavarTuru.RareMonster, "LT_Db_ZorvenP"),
            new Canavar("Db_TalvexP", 42, 11, 21, CanavarTuru.RareMonster, "LT_Db_TalvexP"),
            new Canavar("Db_MirgonP", 39, 10, 19, CanavarTuru.RareMonster, "LT_Db_MirgonP"),
            new Canavar("Db_HexlarP", 48, 13, 24, CanavarTuru.RareMonster, "LT_Db_HexlarP"),
            new Canavar("Db_VornikP", 44, 12, 22, CanavarTuru.RareMonster, "LT_Db_VornikP"),
            new Canavar("Db_JexorP", 37, 9, 17, CanavarTuru.RareMonster, "LT_Db_JexorP"),
            new Canavar("Db_PlentorP", 52, 15, 27, CanavarTuru.RareMonster, "LT_Db_PlentorP"),
            new Canavar("Db_XarminP", 41, 11, 20, CanavarTuru.RareMonster, "LT_Db_XarminP"),
            new Canavar("Db_TorvaxP", 53, 15, 28, CanavarTuru.RareMonster, "LT_Db_TorvaxP"),
            new Canavar("Db_LumerisP", 39, 10, 19, CanavarTuru.RareMonster, "LT_Db_LumerisP"),
            new Canavar("Db_NexilP", 45, 12, 22, CanavarTuru.RareMonster, "LT_Db_NexilP"),
            new Canavar("Db_VorquinP", 47, 13, 24, CanavarTuru.RareMonster, "LT_Db_VorquinP"),
        };
        public static List<Canavar> TumEpicCanavarlar { get; private set; } = new List<Canavar>();
        public static List<Canavar> TumLegendaryCanavarlar { get; private set; } = new List<Canavar>();
        public static List<Canavar> TumMythicCanavarlar { get; private set; } = new List<Canavar>();

        public static List<Boss> TumBossCanavarlar { get; private set; } = new List<Boss>
        {
             new Boss("Dreadlord Xarth", 120, 20, 35, 100,  "LT_Boss_Dreadlord"),
             new Boss("Void Titan Azrul", 150, 25, 40, 100, "LT_Boss_VoidTitan"),
             new Boss("Bloodfang Korgath", 130, 22, 38, 100, "LT_Boss_Bloodfang"),
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
