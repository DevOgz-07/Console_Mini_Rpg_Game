using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Oyun
{
    public class LootEntry
    {
        public Oge Oge { get; set; }
        public int DropSans { get; set; } // yüzde

        public LootEntry(Oge oge, int dropSans)
        {
            Oge = oge;
            DropSans = dropSans;
        }
    }
}
