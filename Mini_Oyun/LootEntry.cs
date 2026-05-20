using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Oyun
{
    public sealed class LootEntry
    {
       
        public Oge Oge { get; }
        public int DropSans { get; } 

        public LootEntry(Oge oge, int dropSans)
        {
            
            Oge = oge ?? throw new ArgumentNullException(nameof(oge), "Loot girdisi için geçerli bir 'Oge' nesnesi sağlanmalıdır.");

            if (dropSans < 0 || dropSans > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(dropSans), "Düşme şansı (DropSans) 0 ile 100 arasında bir yüzde değeri olmalıdır.");
            }

            DropSans = dropSans;
        }
        public double GetDropRate() => DropSans / 100.0;
    }
}
