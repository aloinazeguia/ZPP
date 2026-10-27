// Server.cs
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TcpEchoServer
{
    const int PORT = 5000;

    static void Main()
    {
        TcpListener listener = null;

        try
        {
            listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();
            Console.WriteLine("Zerbitzaria entzuten dago: port " + PORT);

            while (true)
            {
                TcpClient client = null;
                NetworkStream ns = null;
                StreamReader reader = null;
                StreamWriter writer = null;

                try
                {
                    client = listener.AcceptTcpClient();
                    ns = client.GetStream();
                    reader = new StreamReader(ns, Encoding.UTF8);
                    writer = new StreamWriter(ns, Encoding.UTF8);
                    writer.AutoFlush = true;

                    Console.WriteLine("Bezero batek konektatu da: " + client.Client.RemoteEndPoint);

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine("Jaso: " + line);

                        if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
                        {
                            writer.WriteLine("Agur!");
                            break;
                        }

                        writer.WriteLine("Server-ek jasota: " + line);
                    }

                    Console.WriteLine("Bezeroarekin konexioa amaitu da.");
                }
                catch (Exception exClient)
                {
                    Console.WriteLine("Bezeroaren errorea: " + exClient.Message);
                }
                finally
                {
                    if (writer != null) writer.Close();
                    if (reader != null) reader.Close();
                    if (ns != null) ns.Close();
                    if (client != null) client.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Errorea: " + ex.Message);
        }
        finally
        {
            if (listener != null)
            {
                listener.Stop();
                Console.WriteLine("Zerbitzaria gelditu da.");
            }
        }
    }
}