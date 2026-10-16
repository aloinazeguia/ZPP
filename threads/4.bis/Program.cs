using System;
using System.Threading;

class Program
{
    private static readonly object TxandaLock = new object();
    private static bool txandaPelotari = true;
    public class Pelotaria
    {
        public int Id { get; set; }
        public bool Txanda { get; set; }

        public Pelotaria(int id, bool txanda)
        {
            Id = id;
            Txanda = txanda;
        }
    }
    static void Main(string[] args)
    {
        Pelotaria pelotari1 = new Pelotaria(1, true);
        Pelotaria pelotari2 = new Pelotaria(2, false);

        Thread pelotari1Thread = new Thread(() => PelotariLana(pelotari1.Id, pelotari1.Txanda));
        Thread pelotari2Thread = new Thread(() => PelotariLana(pelotari2.Id, pelotari2.Txanda));

        pelotari1Thread.Start();
        pelotari2Thread.Start();

        pelotari1Thread.Join();
        pelotari2Thread.Join();
    }
    static void PelotariLana(int Id, bool Txanda)
    {
        while (true)
        {
            lock (TxandaLock)
            {
                while (Txanda != txandaPelotari)
                {
                    Monitor.Wait(TxandaLock);
                }
                Console.WriteLine($"{Id}. Pelotariak jolasten du");
                Thread.Sleep(1000);
                txandaPelotari = !txandaPelotari;
                Monitor.PulseAll(TxandaLock);
            }
        }
    }
}