using System;
using System.Collections.Generic;
using System.Linq;

namespace Mini_Oyun
{
    public enum CanavarTuru
    {
        CommonMonster,       
        RareMonster,         
        EpicMonster,         
        LegendaryMonster,    
        MythicMonster,       
        Boss,               
    }
        public class Canavar
        {
            public string Ad { get; set; }
            public int Can { get; set; }
            public int MaksimumCan { get; set; }
            public int SaldiriGucu { get; set; }
            public int VerilenTecrube { get; set; }
            public int Savunma { get; set; } = 0;
            public int MinimumHasari { get; set; }

            public int MaksimumHasari { get; set; }


            public CanavarTuru Turu { get; set; }

            public string LootTableId { get; set; }  //düşürelecek itemlerin havuzunu belirlemek için kullanılacak.

            protected Random random = new Random();

            public Canavar(string ad, int can, int saldiri, int exp, CanavarTuru tur, string lootTableId)
            {
                Ad = ad;
                Can = can;
                MaksimumCan = can;
                SaldiriGucu = saldiri;
                VerilenTecrube = exp;
                Savunma = 5;
                Turu = tur;
                LootTableId = lootTableId;
                MinimumHasari = (int)(saldiri * 0.8);
                MaksimumHasari = (int)(saldiri * 1.2);
        }

            public List<Oge> OgeDusur()
            {
                 return LootManager.LootDusur(this);
            }

            public bool HayattaMi() => Can > 0;
        }
             public class Boss : Canavar
             {
      
        
            public Boss(string ad, int hp, int minHasar, int maxHasar, int exp, string lootTableId)
                : base(ad, hp, (minHasar + maxHasar) / 2, exp, CanavarTuru.Boss, lootTableId)
            {
               

                Savunma = 10; 
                MinimumHasari = minHasar;
                MaksimumHasari = maxHasar;

                
            }
        }
    
}

