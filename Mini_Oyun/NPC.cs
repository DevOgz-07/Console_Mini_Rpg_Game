using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Oyun
{
        public interface ISpeaker
        {
            string Ad { get; }
            string Rol { get; }
            void Konus();
        }
        public class NPC : ISpeaker
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
                DialogueManager.RastgeleKonus(this);
            }

            public void HikayeKonus(int adim)
            {
                DialogueManager.SiraliKonus(this, adim);
            }
        }
        public static class DialogueManager
        {
            private static readonly Random _rnd = new Random();

            public static void RastgeleKonus(NPC npc)
            {
                if (npc.Diyaloglar == null || npc.Diyaloglar.Length == 0) return;

                int index = _rnd.Next(npc.Diyaloglar.Length);
                DiyalogCiz(npc.Ad, npc.Rol, npc.Diyaloglar[index]);
            }

            public static void SiraliKonus(NPC npc, int adim)
            {
                if (npc.Diyaloglar == null) return;

                string metin = (adim >= 0 && adim < npc.Diyaloglar.Length)
                               ? npc.Diyaloglar[adim]
                               : npc.Diyaloglar[npc.Diyaloglar.Length - 1]; 

                DiyalogCiz(npc.Ad, npc.Rol, metin);
            }
            private static void DiyalogCiz(string isim, string rol, string mesaj)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"[{isim} - {rol}]");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(": ");
                foreach (char c in mesaj)
                {
                    Console.Write(c);
                    System.Threading.Thread.Sleep(10); 
                }
                Console.WriteLine();
                Console.ResetColor();
            }
        }
    
}
