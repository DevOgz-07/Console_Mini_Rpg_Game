using System;
using System.Collections.Generic;

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
        public string Ad { get; protected set; }
        public int Can { get; protected set; }
        public int MaksimumCan { get; protected set; }
        public int SaldiriGucu { get; protected set; }
        public int VerilenTecrube { get; protected set; }
        public int Savunma { get; protected set; }
        public int MinimumHasari { get; protected set; }
        public int MaksimumHasari { get; protected set; }
        public CanavarTuru Turu { get; protected set; }
        public string LootTableId { get; protected set; }

        private static readonly Random _sharedRandom = new Random();

        public Canavar(string ad, int can, int saldiri, int exp, CanavarTuru tur, string lootTableId, int savunma = 5)
        {
            if (string.IsNullOrWhiteSpace(ad))
                throw new ArgumentException("Canavar adı boş olamaz.", nameof(ad));

            if (can <= 0)
                throw new ArgumentOutOfRangeException(nameof(can), "Canavar canı 0'dan büyük olmalıdır.");

            Ad = ad;
            Can = can;
            MaksimumCan = can;
            SaldiriGucu = saldiri;
            VerilenTecrube = exp;
            Turu = tur;
            LootTableId = lootTableId;
            Savunma = savunma;

            MinimumHasari = (int)(saldiri * 0.8);
            MaksimumHasari = (int)(saldiri * 1.2);
        }

        public List<Oge> OgeDusur()
        {
            return LootManager.LootDusur(this);
        }

        public bool HayattaMi() => Can > 0;
        public virtual void HasarAl(int miktar)
        {
            int netHasar = Math.Max(0, miktar - Savunma);
            Can = Math.Max(0, Can - netHasar);
        }
    }
    public sealed class Boss : Canavar
    {
        public Boss(string ad, int hp, int minHasar, int maxHasar, int exp, string lootTableId)
            : base(ad, hp, (minHasar + maxHasar) / 2, exp, CanavarTuru.Boss, lootTableId, savunma: 10)
        {
            MinimumHasari = minHasar;
            MaksimumHasari = maxHasar;
        }
    }
}