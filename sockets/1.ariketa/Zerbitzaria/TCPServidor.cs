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
        private const string ServerIp = "127.0.0.1"; // Zerbitzariak erabiliko duen IP.
        private const int ServerPort = 13000; // Bezeroek konektatzeko erabiliko duten ataka .


        public static void Main()
        {
            int cont = 0; // Bezeroen kontagailua hasieratzen du.
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
                    cont++; // Bezero berri bat konektatzen den bakoitzean kontagailua handitzen du.
                    Console.WriteLine($"{cont}-bezeroa konektatu da."); // Bezero bat konektatu dela jakinarazten du.

                    Thread t = new Thread(() => ControlCliente(client, cont)); // Bezero hori kudeatzeko hari independente bat sortzen du.
                    //t.IsBackground = true; // Hariak aplikazio nagusiaren amaiera ez blokeatzea ezartzen du.
                    t.Start(); // Bezeroaren kudeaketa-haria martxan jartzen du.
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Zerbitzariaren errorea: {ex.Message}");
            }
            finally // try amaitzean edo errorea gertatzean exekutatzen den kodea definitzen du.
            {
                listener?.Stop(); // Entzulea badago, konexio berriak entzuteari uzten dio.
                Console.WriteLine("Zerbitzaria gelditu da."); // Zerbitzaria gelditu dela kontsolan jakinarazten du.
            }
        } // Main metodoaren kode-blokea amaitzen du.

        private static void ControlCliente(TcpClient bezeroSocket, int cont) // Bezero jakin baten komunikazioa kudeatzen du.
        {
            try
            {
                using NetworkStream stream = bezeroSocket.GetStream(); // Bezeroarekin datuak trukatzeko sare-korrontea lortzen du.
                using StreamReader reader = new StreamReader(stream, Encoding.UTF8); // Korronteko testua UTF-8 formatuan irakurtzeko prestatzen du.
                using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) // Korrontean UTF-8 testua idazteko prestatzen du.
                {
                    AutoFlush = true // Idatzitako erantzun bakoitza berehala bidaltzea aktibatzen du.
                };

                while (true) // Bezeroak mezuak bidaltzen jarraitzen duen bitartean irakurtzen du.
                {
                    string? mezua = reader.ReadLine(); // Bezeroaren hurrengo mezua irakurtzen du, edo null konexioa itxita badago.
                    if (mezua == null) // Bezeroak konexioa itxi duen egiaztatzen du.
                    {
                        break; // Bezero honen mezuak irakurtzeari uzten dio.
                    }

                    Console.WriteLine($"[{cont}-BEZEROA]: {mezua}"); // Bezeroak bidalitako mezua kontsolan erakusten du.

                    if (Amaiera(mezua)) // Mezuak amaiera-adierazleren bat duen egiaztatzen du.
                    {
                        writer.WriteLine("konexioa amaitu da."); // Bezeroari komunikazioa amaituko dela erantzuten dio.
                        Console.WriteLine($"{cont}-Bezeroaren komunikazioa amaitu da."); // Bezero honen komunikazioa amaituko dela erakusten du.
                        break;
                    }
                    int contBoc = 0;
                    foreach (char ca in mezua)
                    {
                        char c = char.ToLower(ca);
                        if ((c == 'a') || c == 'e' || c == 'i' || c == 'o' || c == 'u') { contBoc++; }
                    }


                    int karaktere = mezua.Count(c => "aeiouAEIOU".IndexOf(c) >= 0);
                    int luzeera = mezua.Trim().Length; // Jasotako mezuaren karaktere kopurua kalkulatzen du.
                    writer.WriteLine($"Esaldiak {luzeera} Karaktere eta {contBoc}({karaktere}) bokal ditu"); // Karaktere kopurua bezeroari bidaltzen dio.
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bezeroan errorea: {ex.Message}"); // Bezeroaren errorearen azalpena kontsolan erakusten du.
            }
            finally // Bezeroaren komunikazioa amaitzean beti exekutatzen den kodea definitzen du.
            {
                bezeroSocket.Close(); // Bezero honen TCP socket-a ixten du.
                Console.WriteLine($"{cont}-Bezeroa deskonektatu da"); // Bezeroa deskonektatu dela kontsolan erakusten du.
            }
        }
        private static bool Amaiera(string mezua) // Mezuak komunikazioa amaitzeko hitza duen aztertzen du.
        { // EsFin metodoaren kode-blokea hasten du.
            return mezua.Contains("ATERA", StringComparison.OrdinalIgnoreCase); // ATERA hitza maiuskulak bereizi gabe bilatzen du.
        }
    }
}
