using System.Net.Sockets;
using System.Text;

namespace Bezeroa;

// Bezeroaren informazio orokorra, adibidez bere identifikatzailea.
public class BezeroInfo
{
    public int Id { get; set; }
}

// Partida honen egoera gordetzeko klasea.
// Saiakerak: jokalariak egin dituen saiakera kopurua.
// Partaideak: partidan parte hartzen duten jokalarien zerrenda.
public class BezeroPartida
{
    public int Saiakerak { get; set; }
    public List<int> Partaideak { get; } = new();
}

public static class Program
{
    public static void Main()
    {
        try
        {
            // Zerbitzariarekin konexioa sortzen da, localhost-ean eta 5555 portuan.
            using var client = new TcpClient();
            client.Connect("127.0.0.1", 5555);

            // I/O fluxua sortu eta kodeketa UTF-8 erabiliz.
            using var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var writer = new StreamWriter(stream, Encoding.UTF8)
            {
                AutoFlush = true
            };

            // Zerbitzariak bidaltzen duen ongietorria irakurtzen da.
            var bienvenida = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(bienvenida))
            {
                Console.WriteLine("Ezin izan da zerbitzariarekin konektatu.");
                return;
            }

            Console.WriteLine(bienvenida);

            // Partidaren egoera erregistratzen da kasu honetan saiakerak bakarrik.
            var partida = new BezeroPartida();

            // Jokoa amaitu arte jarraituko du.
            while (true)
            {
                //Console.Write("Sartu zenbakia (1-100): ");
                var sarrera = Console.ReadLine();

                // Sarrerak eduki behar du balio bat; hutsik uztea ez da onartzen.
                if (string.IsNullOrWhiteSpace(sarrera))
                {
                    Console.WriteLine("Zenbakia ezin da hutsik egon.");
                    continue;
                }

                // Zenbakia 1 eta 100 arteko baliozko zenbaki bat izan behar da.
                if (!int.TryParse(sarrera, out var zenbakia) || zenbakia < 1 || zenbakia > 100)
                {
                    Console.WriteLine("Errorea: 1 eta 100 arteko zenbaki bat sartu behar duzu.");
                    continue;
                }

                // Saiakera bat egin dela erregistratzen da.
                partida.Saiakerak++;

                // Zerbitzariari zenbakia bidaltzen zaio.
                writer.WriteLine(zenbakia);

                // Zerbitzariak itzultzen duen erantzuna irakurtzen da.
                var erantzuna = reader.ReadLine();
                while (erantzuna is not null)
                {
                    Console.WriteLine(erantzuna);

                    // Jokoa amaitzea adierazten duten mezuak kontrolatzen dira.
                    if (erantzuna.Contains("Irabazi duzu", StringComparison.OrdinalIgnoreCase) ||
                        erantzuna.Contains("asmatu dute", StringComparison.OrdinalIgnoreCase) ||
                        erantzuna.Contains("Jokoa amaituta da", StringComparison.OrdinalIgnoreCase) ||
                        erantzuna.Contains("Zerbitzaria beteta", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Saiakerak: {partida.Saiakerak}");
                        return;
                    }

                    // Zerbitzariak "Handiagoa" edo "Txikiagoa" bueltatzen badu,
                    // erabiltzaileak hurrengo saiakera egin ahal izango du.
                    if (erantzuna.Equals("Handiagoa", StringComparison.OrdinalIgnoreCase) ||
                        erantzuna.Equals("Txikiagoa", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    erantzuna = reader.ReadLine();
                }
            }
        }
        catch (SocketException)
        {
            // Zerbitzaria ez bada martxan edo konexioa ez bada ezarri, errorea erakusten da.
            Console.WriteLine("Ezin da zerbitzarira konektatu. Mesedez, zerbitzaria exekutatzen ari dela egiaztatu.");
        }
        catch (IOException ex)
        {
            // Fluxuarekin edo konexioarekin lotutako irakurketa/idazketa arazoak kudeatzen dira.
            Console.WriteLine($"Konexio errorea: {ex.Message}");
        }
    }
}

