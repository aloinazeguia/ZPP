using System; // Oinarrizko motak eta kontsolako eragiketak erabiltzeko.
using System.IO; // Datu-korronteak irakurri eta idazteko klaseak erabiltzeko.
using System.Net.Sockets; // TCP bezeroaren eta sare-korrontearen klaseak erabiltzeko.
using System.Text; // UTF-8 testu-kodifikazioa erabiltzeko.

namespace Bezeroa 
{
    public class TCPClient
    {
        private const string Server = "127.0.0.1"; // Zerbitzariaren IP helbidea gordetzen du.
        private const int Port = 13000; // Zerbitzariarekin konektatzeko ataka gordetzen du.

        public static void Main()
        {
            try
            {
                using TcpClient client = new TcpClient(); // TCP bezero-socket bat sortzen du; amaitzean automatikoki itxiko da.
                client.Connect(Server, Port); // Zerbitzariaren IP helbidera eta atakara konektatzen da.

                using NetworkStream stream = client.GetStream(); // Bezeroaren eta zerbitzariaren arteko datu-korrontea lortzen du.
                using StreamReader reader = new StreamReader(stream, Encoding.UTF8); // Sare-korronteko testua UTF-8 formatuan irakurtzeko prestatzen du.
                using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) // Sare-korrontera UTF-8 testua idazteko prestatzen du.
                {
                    AutoFlush = true // Idatzitako erantzun bakoitza berehala bidaltzea aktibatzen du.
                };
                Console.WriteLine($"Zerbitzariarekin konektatuta: {Server}:{Port}"); // Konexioa ezarri dela eta helbidea kontsolan erakusten du.
                Console.WriteLine("Idatzi esaldiak zerbitzariari bidaltzeko. Idatzi 'ATERA' bukatzeko."); // Erabiltzaileari erabilera-argibideak erakusten dizkio.

                while (true) // Erabiltzaileari esaldiak behin eta berriro sartzeko aukera ematen dio.
                {
                    Console.Write("esaldia: "); // Erabiltzaileari esaldi bat idazteko eskatzen dio.
                    string? mensaje = Console.ReadLine(); // Kontsolatik idatzitako esaldia irakurtzen du.

                    if (string.IsNullOrWhiteSpace(mensaje)) // Sarrera hutsik edo zuriunez soilik osatuta dagoen egiaztatzen du.
                    { // if blokearen kodea hasten du.
                        continue; // Sarrera hutsarekin jarraitu gabe, hurrengo esaldia eskatzera itzultzen da.
                    }

                    writer.WriteLine(mensaje); // Erabiltzailearen esaldia zerbitzariari bidaltzen dio.

                    string? respuesta = reader.ReadLine(); // Zerbitzariaren erantzuna irakurtzen du.
                    if (respuesta == null) // Zerbitzariak konexioa itxi edo erantzunik bidali ez duen egiaztatzen du.
                    {
                        break; // Komunikazioa bukatutzat ematen du eta begiztatik ateratzen da.
                    }
                    Console.WriteLine($"Servidor: {respuesta}"); // Zerbitzariaren erantzuna kontsolan erakusten du.

                    if (mensaje.Contains("ATERA", StringComparison.OrdinalIgnoreCase)) // Esaldiak ATERA hitza duen egiaztatzen du, maiuskulak bereizi gabe.
                    {
                        Console.WriteLine("Komunikazioa amaitu da."); // Komunikazioa amaituko dela erabiltzaileari jakinarazten dio.
                        break; // Begiztatik ateratzen da, komunikazioa amaitzeko.
                    }
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"Bezeroa: {ex.Message}"); // Errorearen azalpena kontsolan erakusten du.
            }
            finally // try amaitzean, errorea gertatu ala ez, exekutatzen den blokea hasten du.
            {
                Console.WriteLine("Bezeroak konexioa itxi du.");
            }
        } 
    }
}
