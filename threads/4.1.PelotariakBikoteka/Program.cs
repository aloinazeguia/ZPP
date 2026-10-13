using System;
using System.Threading;

class Program
{
    private static readonly object TxandaLock = new object();
    
    // Txanda daukan jokalariaren IDa (0, 1, 2 edo 3)
    private static int PelotaDuenJokalariaId = 0; 
    private static readonly Random random = new Random();

    static void Main(string[] args)
    {
        // 1. Taldea: 0 (Aurrelaria), 1 (Atzelaria)
        // 2. Taldea: 2 (Aurrelaria), 3 (Atzelaria)
        Thread j1 = new Thread(() => JokalariaLana(0, 0, "1. Taldea - Aurrelaria"));
        Thread j2 = new Thread(() => JokalariaLana(1, 0, "1. Taldea - Atzelaria"));
        Thread j3 = new Thread(() => JokalariaLana(2, 1, "2. Taldea - Aurrelaria"));
        Thread j4 = new Thread(() => JokalariaLana(3, 1, "2. Taldea - Atzelaria"));

        j1.Start();
        j2.Start();
        j3.Start();
        j4.Start();

        j1.Join();
        j2.Join();
        j3.Join();
        j4.Join();
    }

    static void JokalariaLana(int jokalariaId, int taldeaId, string izena)
    {
        while (true)
        {
            lock (TxandaLock)
            {
                // PulseAll erabiltzean, hari GUZTIAK esnatuko dira (Wait egoeratik irtenez).
                // Baina 'while' baldintzak ziurtatzen du bere txanda ez den haria
                // berriro Monitor.Wait() egoeran sartuko dela.
                while (PelotaDuenJokalariaId != jokalariaId)
                {
                    Monitor.Wait(TxandaLock);
                }

                Console.WriteLine($"{izena}(k) pilota jo du!");
                Thread.Sleep(250);

                // Aurkako taldeko jokalari bat random bidez aukeratu (aurrelaria edo atzelaria)
                int besteTaldea = (taldeaId == 0) ? 1 : 0;
                int jokaliarenPosizioa = random.Next(0, 2); // 0 (Aurrelaria) edo 1 (Atzelaria)

                PelotaDuenJokalariaId = (besteTaldea == 0) ? jokaliarenPosizioa : 2 + jokaliarenPosizioa;

                // Hari GUZTIEI abisatu egoera aldatu dela
                Monitor.PulseAll(TxandaLock);
            }
        }
    }
}