using System;
using System.Threading;

class Program
{
    // Aimar metodoa: "Aimar" 10 aldiz inprimatzen du, 300ms-ko tartearekin
    static void Aimar()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("Aimar");
            Thread.Sleep(300);
        }
    }

    // Nerea metodoa: "Nerea" 10 aldiz inprimatzen du, 1000ms-ko tartearekin
    static void Nerea()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("Nerea");
            Thread.Sleep(1000);
        }
    }

    // Jurgi metodoa: "Jurgi" 10 aldiz inprimatzen du, 500ms-ko tartearekin
    static void Jurgi()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("Jurgi");
            Thread.Sleep(500);
        }
    }

    static void Main()
    {
        // Hiru hariak sortu
        Thread h1 = new Thread(Aimar);
        Thread h2 = new Thread(Nerea);
        Thread h3 = new Thread(Jurgi);

        // Hariak exekutatzen hasi
        h1.Start();
        h2.Start();
        h3.Start();

        // Hari guztiak amaitu arte itxaron
        h1.Join();
        h2.Join();
        h3.Join();

        Console.WriteLine("Hari guztiak amaitu dira. Programa bukatu da.");
    }
}