class Programa
{
    // 1. Sinkronizaziorako blokeo-objektu bakarra
    private static readonly object etxeaLock = new object();

    // 2. Aldagai partekatuak (Etxearen egoera)
    private static int aulkiLibreak = 4;
    private static int zerbitzatzekoZainDaudenIpotxak = 0;

    static void Main(string[] args)
    {
        Console.WriteLine("--- Edurnezuri eta Zazpi Ipotxak ---\n");

        // 3. Edurnezuriren haria sortu eta abiarazi
        Thread edurnezuriThread = new Thread(EdurnezuriLana);
        edurnezuriThread.Start();

        // 4. Zazpi ipotxen hariak sortu eta abiarazi
        for (int i = 1; i <= 7; i++)
        {
            int ipotxId = i; 
            Thread ipotxThread = new Thread(() => IpotxLana(ipotxId));
            ipotxThread.Start();
        }
    }

    // IPOTXEN LOGIKA
    static void IpotxLana(int id)
    {
        Random random = new Random();

        while (true)
        {
            // Meategian lanean (Simulazioa: 1-3 segundo)
            Console.WriteLine($"[{id}. Ipotxa] Meategian lanean ari da...");
            Thread.Sleep(random.Next(1000, 3000));

            Console.WriteLine($"[{id}. Ipotxa] Etxera iritsi da, gose da.");

            // KRITIKOA: Etxean sartu eta aldagaiak aldatu
            lock (etxeaLock)
            {
                // Aulki librerik ez badago,itxaron aulki librea egon arte
                while (aulkiLibreak == 0)
                {
                    Console.WriteLine($"[{id}. Ipotxa] Ez dago aulkirik libre. Kanpoan zain...");
                    Monitor.Wait(etxeaLock); 
                }

                // Aulkia hartu
                aulkiLibreak--;
                zerbitzatzekoZainDaudenIpotxak++;
                Console.WriteLine($"[{id}. Ipotxa] 'Edurnezuri, eseri naiz, zerbitzatu nazazu mesedez!'\nAulki libreak:{aulkiLibreak}");

                // Edurnezuriri esnatu (buelta bat ematen badago, abisatzeko)
                Monitor.PulseAll(etxeaLock);

                // Orain Edurnezurik janaria zerbitzatu arte ITXARON behar du
                // (Gure logikan, Edurnezurik zerbitzatzen duenean 'zerbitzatzekoZainDaudenIpotxak' jaitsiko du)
                int BazkialtzekoZain = zerbitzatzekoZainDaudenIpotxak;
                while (zerbitzatzekoZainDaudenIpotxak >= BazkialtzekoZain)
                {
                    Monitor.Wait(etxeaLock);
                }

                // Console.WriteLine($"[{id}. Ipotxa] Jaten hasi da... Goxoa!");

                //   Thread.Sleep(random.Next(1000, 2000));
                //     aulkiLibreak++;
                // Console.WriteLine($"[{id}. Ipotxa] Jaten amaitu du. Aulkia libre utzi du\nAulki libreak:{aulkiLibreak}");
                
                // // Beste ipotxei edo Edurnezuriri abisatu egoera aldatu dela
                // Monitor.PulseAll(etxeaLock);
            }

            // Jaten ari da (Blokeotik KANPO, Edurnezurik beste batzuei zerbitzatu ahal izateko)
            Thread.Sleep(random.Next(1000, 2000));

            // KRITIKOA: Amaitzean aulkia askatu
            lock (etxeaLock)
            {
                aulkiLibreak++;
                Console.WriteLine($"[{id}. Ipotxa] Jaten amaitu du. Aulkia libre utzi du\nAulki libreak:{aulkiLibreak}");
                Thread.Sleep(500); // Aulkiak askatzeko denbora pixka bat
                // Beste ipotxei edo Edurnezuriri abisatu egoera aldatu dela
                Monitor.PulseAll(etxeaLock);
            }
        }
    }

    // EDURNEZURIREN LOGIKA
    static void EdurnezuriLana()
    {
        Random random = new Random();

        while (true)
        {
            lock (etxeaLock)
            {
                // Inor ez badago zerbitzatzeko, buelta bat emango du
                while (zerbitzatzekoZainDaudenIpotxak == 0)
                {
                    Console.WriteLine("[Edurnezuri] Bueltaka dabil ipotxen zain...");
                    
                    // Blokeoa askatzen du eta norbaitek Pulse egin arte itxaroten du
                    Monitor.Wait(etxeaLock);
                }

                // Ipotx bat zerbitzatu
                Console.WriteLine("[Edurnezuri] Ipotx bat zerbitzatzen ari da...");
                Thread.Sleep(500); // Zerbitzatzeko denbora pixka bat

                zerbitzatzekoZainDaudenIpotxak--;
                Console.WriteLine("[Edurnezuri] 'Jan dezakezu!' esan dio ipotxari.");

                // Ipotxari esan jaten hasteko
                Monitor.PulseAll(etxeaLock);
            }

            // Edurnezurik atseden pixka bat hartzen du hurrengo ipotxa eseri arte
            Thread.Sleep(300);
        }
    }
}