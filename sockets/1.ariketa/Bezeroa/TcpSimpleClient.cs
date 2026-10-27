using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

class TcpSimpleClient
{
    const string SERVER = "127.0.0.1"; // edo zerbitzariaren IP helbidea
    const int PORT = 13000;

    static void Main()
    {
        TcpClient client = null;
        NetworkStream ns = null;
        StreamReader reader = null;
        StreamWriter writer = null;

        try
        {
            client = new TcpClient();
            client.Connect(SERVER, PORT);

            ns = client.GetStream();
            reader = new StreamReader(ns, Encoding.UTF8);
            writer = new StreamWriter(ns, Encoding.UTF8);
            writer.AutoFlush = true;

            Console.WriteLine($"Konektatuta {SERVER}:{PORT}");
            Console.WriteLine("Idatzi testua bidaltzeko. 'exit' idatziz amaitzen da.");

            while (true)
            {
                string line = Console.ReadLine();
                if (line == null) break;

                writer.WriteLine(line);
                string response = reader.ReadLine();
                if (response == null) break;

                Console.WriteLine("Server-etik: " + response);

                if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Errorea: " + ex.Message);
        }
        finally
        {
            // Itxi dena eskuz
            if (writer != null) writer.Close();
            if (reader != null) reader.Close();
            if (ns != null) ns.Close();
            if (client != null) client.Close();
        }
    }
}