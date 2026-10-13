class Program
{
    private static readonly object TxandaLock =new object();
    private  static bool txandaPelotari= true;
    static void Main(string[] args)
    {
        Thread pelotari1Thread = new Thread(Pelotari1Lana);
        Thread pelotari2Thread = new Thread(Pelotari2Lana);

        pelotari1Thread.Start();
        pelotari2Thread.Start();

        pelotari1Thread.Join();
        pelotari2Thread.Join();
    }
    static void Pelotari1Lana()
    {
        txandaPelotari = true;
        while (true)
        {
            lock(TxandaLock)
            {
                while (!txandaPelotari)
                {
                    Monitor.Wait(TxandaLock);
                }
                Console.WriteLine("1.Pelotariak jolasten du");
                Thread.Sleep(1000);
                txandaPelotari = false;
                Monitor.PulseAll(TxandaLock);
            }
        }
    }
    static void Pelotari2Lana()
    {
        txandaPelotari = false;
        while (true)
        {
            lock(TxandaLock)
            {
                while (txandaPelotari)
                {
                    Monitor.Wait(TxandaLock);
                }
                Console.WriteLine("2.Pelotariak jolasten du");
                Thread.Sleep(1000);
                txandaPelotari = true;
                Monitor.PulseAll(TxandaLock);
            }
        }
    }
}
