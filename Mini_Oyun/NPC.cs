using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Oyun
{
    public class NPC
    {
        public string Ad { get; set; }
        public string Rol { get; set; }
        public string[] Diyaloglar { get; set; }

        public NPC(string ad, string rol, string[] diyaloglar)
        {
            Ad = ad;
            Rol = rol;
            Diyaloglar = diyaloglar;
        }

        public void Konus()
        {
            Random rnd = new Random();
            string secilenDiyalog = Diyaloglar[rnd.Next(Diyaloglar.Length)];

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n[{Ad} - {Rol}]: \"{secilenDiyalog}\"");
            Console.ResetColor();
        }
    }
}
