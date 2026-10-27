using System; // Oinarrizko .NET motak erabiltzeko.
using System.IO; // Korronteak irakurri eta idazteko klaseak erabiltzeko.
using System.Net; // IP helbideak kudeatzeko klaseak erabiltzeko.
using System.Net.Sockets; // TCP socket-en klaseak erabiltzeko.
using System.Text; // UTF-8 kodifikazioa erabiltzeko.
using System.Threading; // Bezero bakoitzarentzat hariak sortzeko.

namespace Zerbitzaria
{
    public class TCPServidor // TCP bidezko zerbitzariaren klasea definitzen du.
    {
        private const string ServerIp = "127.0.0.1"; // Zerbitzariak erabiliko duen IP lokala gordetzen du.
        private const int ServerPort = 13000; // Bezeroek konektatzeko erabiliko duten ataka ezartzen du.
        

        public static void Main()
        {
            int contador = 0; // Bezeroen kontagailua hasieratzen du.
            TcpListener listener = null!; // TCP konexio berriak entzuteko zerbitzaria definitzen du.

            try
            {
                var ip = IPAddress.Parse(ServerIp); // IP helbidearen testua IPAddress objektu bihurtzen du.
                listener = new TcpListener(ip, ServerPort); // IP eta atakarekin TCP entzulea sortzen du.
                listener.Start(); // Bezeroen konexioak onartzeko entzuten hasten da.

                Console.WriteLine($"Zerbitzaria {ServerIp}:{ServerPort} helbidean hasi da."); // Zerbitzaria non dagoen kontsolan erakusten du.

                while (true) // Zerbitzaria etengabe konektatzeko prest mantentzen du.
                {
                    TcpClient client = listener.AcceptTcpClient(); // Bezero baten konexioa onartu arte itxaroten du.
                    contador++; // Bezero berri bat konektatzen den bakoitzean kontagailua handitzen du.
                    Console.WriteLine($"{contador}-bezeroa konektatu da."); // Bezero bat konektatu dela jakinarazten du.

                    Thread t = new Thread(() => ControlCliente(client,contador)); // Bezero hori kudeatzeko hari independente bat sortzen du.
                    //t.IsBackground = true; // Hariak aplikazio nagusiaren amaiera ez blokeatzea ezartzen du.
                    t.Start(); // Bezeroaren kudeaketa-haria martxan jartzen du.
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el servidor: {ex.Message}");
            }
            finally // try amaitzean edo errorea gertatzean exekutatzen den kodea definitzen du.
            {
                listener?.Stop(); // Entzulea badago, konexio berriak entzuteari uzten dio.
                Console.WriteLine("Zerbitzaria gelditu da."); // Zerbitzaria gelditu dela kontsolan jakinarazten du.
            }
        } // Main metodoaren kode-blokea amaitzen du.

        private static void ControlCliente(TcpClient socketCliente,int contador) // Bezero jakin baten komunikazioa kudeatzen du.
        {
            try
            {
                using NetworkStream stream = socketCliente.GetStream(); // Bezeroarekin datuak trukatzeko sare-korrontea lortzen du.
                using StreamReader reader = new StreamReader(stream, Encoding.UTF8); // Korronteko testua UTF-8 formatuan irakurtzeko prestatzen du.
                using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) // Korrontean UTF-8 testua idazteko prestatzen du.
                {
                    AutoFlush = true // Idatzitako erantzun bakoitza berehala bidaltzea aktibatzen du.
                };

                while (true) // Bezeroak mezuak bidaltzen jarraitzen duen bitartean irakurtzen du.
                {
                    string? mensaje = reader.ReadLine(); // Bezeroaren hurrengo mezua irakurtzen du, edo null konexioa itxita badago.
                    if (mensaje == null) // Bezeroak konexioa itxi duen egiaztatzen du.
                    {
                        break; // Bezero honen mezuak irakurtzeari uzten dio.
                    }

                    Console.WriteLine($"{contador}-Bezeroa: {mensaje}"); // Bezeroak bidalitako mezua kontsolan erakusten du.

                    if (Amaiera(mensaje)) // Mezuak amaiera-adierazleren bat duen egiaztatzen du.
                    {
                        writer.WriteLine("konexioa amaitu da."); // Bezeroari komunikazioa amaituko dela erantzuten dio.
                        Console.WriteLine($"{contador}-Bezeroaren komunikazioa amaitu da."); // Bezero honen komunikazioa amaituko dela erakusten du.
                        break;
                    }

                    int longitud = mensaje.Length; // Jasotako mezuaren karaktere kopurua kalkulatzen du.
                    writer.WriteLine($"La frase tiene {longitud} caracteres."); // Karaktere kopurua bezeroari bidaltzen dio.
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el cliente: {ex.Message}"); // Bezeroaren errorearen azalpena kontsolan erakusten du.
            }
            finally // Bezeroaren komunikazioa amaitzean beti exekutatzen den kodea definitzen du.
            {
                socketCliente.Close(); // Bezero honen TCP socket-a ixten du.
                Console.WriteLine($"{contador}-Bezeroa deskonektatu da"); // Bezeroa deskonektatu dela kontsolan erakusten du.
            }
        }
        private static bool Amaiera(string mensaje) // Mezuak komunikazioa amaitzeko hitza duen aztertzen du.
        { // EsFin metodoaren kode-blokea hasten du.
            return mensaje.Contains("ATERA", StringComparison.OrdinalIgnoreCase); // ATERA hitza maiuskulak bereizi gabe bilatzen du.
        }
    }
}
