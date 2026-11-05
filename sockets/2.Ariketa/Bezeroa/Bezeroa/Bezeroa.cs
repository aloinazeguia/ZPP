using System.Net.Sockets;
using System.Text;

namespace Bezeroa;

public class BezeroInfo
{
    public int Id { get; set; }
}

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
            using var client = new TcpClient();
            client.Connect("127.0.0.1", 5555);

            using var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var writer = new StreamWriter(stream, Encoding.UTF8)
            {
                AutoFlush = true
            };

            var bienvenida = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(bienvenida))
            {
                Console.WriteLine("Ezin izan da zerbitzariarekin konektatu.");
                return;
            }

            Console.WriteLine(bienvenida);

            var partida = new BezeroPartida();

            while (true)
            {
                Console.Write("Sartu zenbakia (1-100): ");
                var sarrera = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(sarrera))
                {
                    Console.WriteLine("Zenbakia ezin da hutsik egon.");
                    continue;
                }

                if (!int.TryParse(sarrera, out var zenbakia) || zenbakia < 1 || zenbakia > 100)
                {
                    Console.WriteLine("Errorea: 1 eta 100 arteko zenbaki bat sartu behar duzu.");
                    continue;
                }

                partida.Saiakerak++;
                writer.WriteLine(zenbakia);

                var erantzuna = reader.ReadLine();
                while (erantzuna is not null)
                {
                    Console.WriteLine(erantzuna);

                    if (erantzuna.Contains("Irabazi duzu", StringComparison.OrdinalIgnoreCase) ||
                        erantzuna.Contains("asmatu dute", StringComparison.OrdinalIgnoreCase) ||
                        erantzuna.Contains("Jokoa amaituta da", StringComparison.OrdinalIgnoreCase) ||
                        erantzuna.Contains("Zerbitzaria beteta", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Saiakerak: {partida.Saiakerak}");
                        return;
                    }

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
            Console.WriteLine("Ezin da zerbitzarira konektatu. Mesedez, zerbitzaria exekutatzen ari dela egiaztatu.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Konexio errorea: {ex.Message}");
        }
    }
}

