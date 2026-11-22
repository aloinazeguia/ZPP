using System;
using System.Threading;
class Program
{
    static void Main(string[] args)
    {
        Bankua bankua = new Bankua(1000);
        Thread h1 = new Thread(() =>
        {
            int id=1;
            Console.WriteLine($"========[H{id}] Saldo => {bankua.GetSaldo()}========");
            bankua.Depositar(500,id);
            bankua.Retirar(200,id);
            
        });
        Thread h2 = new Thread(() =>
        {
            int id=2;
            Console.WriteLine($"========[H{id}] Saldo => {bankua.GetSaldo()}========");
            bankua.Depositar(300,id);
            bankua.Retirar(100,id);
        });
        Thread h3 = new Thread(() =>
        {
            int id = 3;
            Console.WriteLine($"========[H{id}] Saldo => {bankua.GetSaldo()}========");
            bankua.Depositar(400,id);
            bankua.Retirar(200,id);
        });
        h1.Start(); 
        h2.Start();
        h3.Start();

        h1.Join();
        h2.Join();
        h3.Join();
        


        Console.WriteLine($"El saldo es: {bankua.GetSaldo()}");


    }
}
class Bankua
{
    private decimal Saldo { get; set; }
    private readonly object blokeoa = new object();
    public Bankua(decimal saldoIncial)
    {


        Saldo = saldoIncial;
       // Console.WriteLine($"SaldoInicial: {Saldo}");

    }
    public void Depositar(decimal monto,int id)
    {
        lock (blokeoa)
        {
            Saldo += monto;
            Console.WriteLine($"[H{id}] Deposito:{monto} => {Saldo}");
        }
    }
    public void Retirar(decimal monto,int id)
    {
        lock (blokeoa)
        {
            Saldo -= monto;
            Console.WriteLine($"[H{id}] Retiro:{monto} => {Saldo}");
        }
    }
    public decimal GetSaldo()
    {
        lock (blokeoa)
        {
            return Saldo;
        }
    }


}