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
        public class Canavar
        {
            public string Ad { get; set; }
            public int Can { get; set; }
            public int MaksimumCan { get; set; }
            public int SaldiriGucu { get; set; }
            public int VerilenTecrube { get; set; }
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
                Turu = tur;
                LootTableId = lootTableId;
            }

            public List<Oge> OgeDusur()
            {
                 return LootManager.LootDusur(this);
            }

            public bool HayattaMi() => Can > 0;
        }
             public class Boss : Canavar
             {
             public Boss(string ad, int hp, int minHasar, int maxHasar,int exp, string lootTableId)
                : base(ad, hp, minHasar, maxHasar, CanavarTuru.Boss, lootTableId)
        {
                 Ad = ad;
                 Can = hp;
                 MaksimumCan = hp;
                 VerilenTecrube = exp;
                 MinimumHasari = minHasar;
                 MaksimumHasari = maxHasar;
                 LootTableId = lootTableId;



             }
        }


}

