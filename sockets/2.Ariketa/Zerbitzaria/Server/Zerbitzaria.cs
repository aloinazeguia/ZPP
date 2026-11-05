using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Zerbitzaria;

public class BezeroInfo
{
    public int Id { get; set; }
    public TcpClient Socket { get; set; } = null!;
    public int Saiakerak { get; set; }
}

public class Partida
{
    public int Zenbakia { get; set; }
    public List<BezeroInfo> Partaideak { get; } = new();
    public int? IrabazleaId { get; set; }
    public bool Amaituta { get; set; }
    public object Lock { get; } = new();

    public Partida()
    {
        Zenbakia = new Random().Next(1, 101);
    }

    public void Erregistratu(BezeroInfo bezero)
    {
        lock (Lock)
        {
            Partaideak.Add(bezero);
        }
    }

    public void Reset()
    {
        lock (Lock)
        {
            Zenbakia = new Random().Next(1, 101);
            IrabazleaId = null;
            Amaituta = false;
        }
    }

    public bool EginZehaztapena(BezeroInfo bezero, int zenbakia, out string mezua, out bool irabazi)
    {
        lock (Lock)
        {
            irabazi = false;

            if (Amaituta)
            {
                mezua = $"Jokoa amaituta da. Zenbakia {Zenbakia} zen.";
                return false;
            }

            bezero.Saiakerak++;

            if (zenbakia < Zenbakia)
            {
                mezua = "Handiagoa";
                return true;
            }

            if (zenbakia > Zenbakia)
            {
                mezua = "Txikiagoa";
                return true;
            }

            Amaituta = true;
            IrabazleaId = bezero.Id;
            irabazi = true;
            mezua = "Irabazi duzu!! Jokoa amaitu da.";
            return false;
        }
    }
}

public class Zerbitzaria
{
    private readonly TcpListener _listener = new(IPAddress.Any, 5555);
    private readonly Partida _partida = new();
    private readonly object _bezeroakLock = new();
    private int _hurrengoBezeroId = 1;

    public async Task EntzunAsync()
    {
        _listener.Start();
        Console.WriteLine("Zerbitzaria martxan dago. 5555 portuan entzuten.");

        while (true)
        {
            TcpClient socket;

            try
            {
                socket = await _listener.AcceptTcpClientAsync();
            }
            catch (ObjectDisposedException)
            {
                break;
            }

            lock (_bezeroakLock)
            {
                if (_partida.Partaideak.Count >= 3)
                {
                    var writer = new StreamWriter(socket.GetStream(), Encoding.UTF8)
                    {
                        AutoFlush = true
                    };
                    writer.WriteLine("Zerbitzaria beteta dago. Gehienez 5 bezero konekta daitezke.");
                    socket.Close();
                    continue;
                }
               
            }

            var bezero = new BezeroInfo
            {
                Id = _hurrengoBezeroId++,
                Socket = socket,
                Saiakerak = 0
            };

            _partida.Erregistratu(bezero);
            Console.WriteLine($"Bezero {bezero.Id} konektatu da.");

            var thread = new Thread(() => Data(bezero));
            thread.IsBackground = true;
            thread.Start();
        }
    }

    public void BidaliZenbakia(BezeroInfo bezero, int zenbakia, string mezua)
    {
        Console.WriteLine($"Bezero {bezero.Id}-ren zenbakia: {zenbakia} => {mezua}");
       
       
    }

    public void Data(BezeroInfo bezero)
    {
        using var stream = bezero.Socket.GetStream();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        using var writer = new StreamWriter(stream, Encoding.UTF8)
        {
            AutoFlush = true
        };

        try
        {
            writer.WriteLine($"Kaixo! Zure identifikatzailea: {bezero.Id}. 1 eta 100 arteko zenbakia asmatu.");

            while (true)
            {
                var lerroa = reader.ReadLine();
                if (lerroa is null)
                {
                    break;
                }

                if (!int.TryParse(lerroa, out var zenbakia) || zenbakia < 1 || zenbakia > 100)
                {
                    writer.WriteLine("Errorea: 1 eta 100 arteko zenbaki bat sartu behar duzu.");
                    continue;
                }

                var jarraitu = _partida.EginZehaztapena(bezero, zenbakia, out var mezua, out var irabazi);
                BidaliZenbakia(bezero, zenbakia,mezua);
                writer.WriteLine(mezua);
                

                if (irabazi)
                {
                    int irabazleaId;
                    int zenbakiIrabazlea;
                    List<BezeroInfo> partaideak;

                    lock (_partida.Lock)
                    {
                        irabazleaId = _partida.IrabazleaId!.Value;
                        zenbakiIrabazlea = _partida.Zenbakia;
                        partaideak = _partida.Partaideak.ToList();
                    }

                    foreach (var jokalari in partaideak)
                    {
                        if (jokalari.Id == bezero.Id)
                        {
                            continue;
                        }

                        try
                        {
                            using var bestekoStream = jokalari.Socket.GetStream();
                            using var bestekoWriter = new StreamWriter(bestekoStream, Encoding.UTF8)
                            {
                                AutoFlush = true
                            };
                            bestekoWriter.WriteLine($"Jokoa amaituta da, zenbakia asmatu dute. Irabazlea: {irabazleaId}. Zenbakia: {zenbakiIrabazlea}.");
                        }
                        catch
                        {
                            // Besteko bezeroak deskonektatuta egon daitezke.
                        }
                    }

                    writer.WriteLine($"Saiakerak: {bezero.Saiakerak}");
                    break;
                }

                if (!jarraitu)
                {
                    break;
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Bezero {bezero.Id} deskonektatu da: {ex.Message}");
        }
        finally
        {
            try
            {
                bezero.Socket.Close();
            }
            catch
            {
                // Saihestu itxierari lotutako erroreak.
            }

            lock (_partida.Lock)
            {
                _partida.Partaideak.RemoveAll(item => item.Id == bezero.Id);
            }

            Console.WriteLine($"Bezero {bezero.Id} amaituta.");
        }
    }
}

public static class Server
{
    public static async Task Main()
    {
        var zerbitzaria = new Zerbitzaria();
        await zerbitzaria.EntzunAsync();
    }
}
