using System.Diagnostics;
using System.IO.Pipes;
using System.Security.Cryptography;

namespace pipe
{
    class Bezeroa
    {
        static void Main(String[] args)
        {
            Console.WriteLine("=====> BEZEROA <=====");

            //Segundu bat itxaron Zerbitzariari hasteko denbora emateko,
            // pipea sortu (ZERBITZARIAREN IZEN BERBERA)
            // eta zerbitzarira konektatu
            Task.Delay(1000).Wait();
            var client = new NamedPipeClientStream("ZPP_1UDa_Pipe");
            client.Connect();
            Console.WriteLine("Zerbitzariarekin konektatuta");

            StreamReader reader = new StreamReader(client);
            StreamWriter writer = new StreamWriter(client);

            String mezua = "Kaixo, bezeroa naiz";
            writer.WriteLine(mezua);
            writer.WriteLine(mezua);
            writer.Flush();
            Console.WriteLine("Mezua bidalita");

            var erantzuna = reader.ReadLine();
            Console.WriteLine("Zerbitzariaren erantzuna: {0}", erantzuna); //ERANTZUNA

            Console.ReadLine(); //Bestela lehioa automatikoki isten da amaitzean.

        }
    }
}
