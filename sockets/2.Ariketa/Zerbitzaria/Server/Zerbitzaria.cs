using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Zerbitzaria;

// Bezero bakoitzaren informazioa gordetzen duen klasea.
public class BezeroInfo
{
    public int Id { get; set; }
    public TcpClient Socket { get; set; } = null!;
    public int Saiakerak { get; set; }
}

// Partida baten egoera eta logika printzipalak kudeatzen ditu.
public class Partida
{
    public int Zenbakia { get; set; }
    public List<BezeroInfo> Partaideak { get; } = new();
    public int? IrabazleaId { get; set; }
    public bool Amaituta { get; set; }
    public object Lock { get; } = new();// Lock: partidak aldi berean hainbat thread-ek erabiltzeko kateatzea.

    public Partida()
    {
        // Partida berri bakoitzean zenbaki ausazkoa sortzen da 1 eta 100 artean.
        Zenbakia = new Random().Next(1, 101);
    }

    // Bezero berria partida erregistroan sartzen da.
    public void Erregistratu(BezeroInfo bezero)
    {
        lock (Lock)
        {
            Partaideak.Add(bezero);
        }
    }

    // Partida berrabiarazten da, zenbakia berriro sortzeko eta egoera berrezartzeko.
    public void Reset()
    {
        lock (Lock)
        {
            Zenbakia = new Random().Next(1, 101);
            IrabazleaId = null;
            Amaituta = false;
        }
    }

    // Bezeroak bidaltzen duen zenbakia ebaluatzen da.
    // mezua: zerbitzariak bezeroari itzultzen dion emaitza.
    // irabazi: bezeroak zenbakia asmatu duen ala ez adierazten du.
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

            // Saiakera bat gertatu da; kontatzen da.
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

            // Zenbakia asmatu bada, partida amaituta egongo da.
            Amaituta = true;
            IrabazleaId = bezero.Id;
            irabazi = true;
            mezua = "Irabazi duzu!! Jokoa amaitu da.";
            return false;
        }
    }
}

// Zerbitzari nagusia: konekzioak onartzen ditu eta thread bakoitzerako bezeroa kudeatzen du.
public class Zerbitzaria
{
    // 5555 portuan entzuten du,
    // IPAddress.Any erabilita, edozein interfaze lokaletik konektatu daiteke.
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
                // Bezero berria konektatzen den zain egongo da.
                socket = await _listener.AcceptTcpClientAsync();
            }
            catch (ObjectDisposedException)
            {
                break;
            }

            // Partidako bezero kopurua muga egiaztatzen da.
            lock (_bezeroakLock)
            {
                if (_partida.Partaideak.Count >= 3)
                {
                    var writer = new StreamWriter(socket.GetStream(), Encoding.UTF8)
                    {
                        AutoFlush = true
                    };
                    writer.WriteLine("Zerbitzaria beteta dago.\nGehienez 3 bezero konekta daitezke.");
                    socket.Close();
                    continue;
                }
            }

            // Bezero berria sortu eta partida erregistroan sartu.
            var bezero = new BezeroInfo
            {
                Id = _hurrengoBezeroId++,
                Socket = socket,
                Saiakerak = 0
            };

            _partida.Erregistratu(bezero);
            Console.WriteLine($"Bezero {bezero.Id} konektatu da.");

            // Bezero bakoitzarentzat thread berezi bat sortzen da, datuak prozesatzeko.
            var thread = new Thread(() => Data(bezero));
            thread.IsBackground = true;
            thread.Start();
        }
    }

    // Bezeroari bidali beharreko zenbakia eta mezua erregistratzeko metodoa.
    public void BidaliZenbakia(BezeroInfo bezero, int zenbakia, string mezua, StreamWriter writer )
    {
        Console.WriteLine($"Bezero {bezero.Id}-ren zenbakia: {zenbakia} => {mezua}");
        writer.WriteLine(mezua);
    }

    // Bezero bakoitzarekin elkarrizketa eta partida logika egiten duen metodoa.
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
            // Bezeroari partida_hasierako mezu bat bidaltzen zaio.
            writer.WriteLine($"Kaixo! Zure identifikatzailea: {bezero.Id}.");
            writer.WriteLine($"Jokoa 1 eta 100 arteko zenbakia asmatu behar duzu. Saiatu zenbat saiakeratan asmatu dezakezun.");
            while (true)
            {
                // Bezeroak bidali duen lerroa irakurtzen da.
                var lerroa = reader.ReadLine();
                if (lerroa is null)
                {
                    break;
                }

                // Zenbakia baliozkoa dela egiaztatzen da.
                if (!int.TryParse(lerroa, out var zenbakia) || zenbakia < 1 || zenbakia > 100)
                {
                    writer.WriteLine("Errorea: 1 eta 100 arteko zenbaki bat sartu behar duzu.");
                    continue;
                }

                // Zenbakia ebaluatu eta emaitza kalkulatzen da.
                var jarraitu = _partida.EginZehaztapena(bezero, zenbakia, out var mezua, out var irabazi);
                BidaliZenbakia(bezero, zenbakia, mezua,writer);
                //writer.WriteLine(mezua);

                // Erabiltzaileak asmatu badu, jokaldia amaituko da.
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

                    // Beste jokalariei ere informazioa bidaltzen zaie partida amaituta dagoela.
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

                // Jarraitu behar ez bada, itxi eta amaitu.
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
