using System.Collections.Generic;
using static Mini_Oyun.Canavar_ve_Boss;

namespace Mini_Oyun
{
    internal static class CanavarVeritabani
    {
        
        public static List<Canavar> TumCommonCanavarlar { get; private set; } = new List<Canavar>
        {
            new Goblin(),
            new Iskelet(),
            new Orc(),
            new Yarasa(),
           
        };

        
        public static List<Boss> TumBosslar { get; private set; } = new List<Boss>
        {
            new Ejderha(),
            new OrcReisi()
        };

        
        public static List<Canavar> TumRareCanavarlar { get; private set; } = new List<Canavar>();
        public static List<Canavar> TumEpicCanavarlar { get; private set; } = new List<Canavar>();
        public static List<Canavar> TumLegendaryCanavarlar { get; private set; } = new List<Canavar>();
        public static List<Canavar> TumMythicCanavarlar { get; private set; } = new List<Canavar>();

      
        public static List<Canavar> TumCanavarlar()
        {
            var tumu = new List<Canavar>();
            tumu.AddRange(TumCommonCanavarlar);
            tumu.AddRange(TumRareCanavarlar);
            tumu.AddRange(TumEpicCanavarlar);
            tumu.AddRange(TumLegendaryCanavarlar);
            tumu.AddRange(TumMythicCanavarlar);
            tumu.AddRange(TumBosslar);
            return tumu;
        }
    }
}
