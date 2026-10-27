using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClienteMultiple
{
    public class TCPServidor
    {
        public static void Main(string[] args)
        {
            string servidor = "127.0.0.1";
            IPAddress ipserver = IPAddress.Parse(servidor);
            int port = 13000;

            TcpListener listener = new TcpListener(ipserver, port);
            Console.WriteLine("Servidor iniciado en {0}:{1}", servidor, port);
            listener.Start(10);

            while (true)
            {
                TcpClient socketCliente = listener.AcceptTcpClient(); // bloqueante
                Console.WriteLine("Cliente conectado.");

                Thread t = new Thread(() => ControlCliente(socketCliente));
                t.Start();
            }
        }

        private static void ControlCliente(TcpClient socket)
        {
            try
            {
                using NetworkStream network = socket.GetStream();
                using StreamWriter writer = new StreamWriter(network);
                using StreamReader reader = new StreamReader(network);

                writer.WriteLine("Conexión establecida con el servidor.");
                writer.Flush();

                string mensaje;
                while ((mensaje = reader.ReadLine()) != null)
                {
                    Console.WriteLine("Mensaje del cliente: " + mensaje);
                    writer.WriteLine("Servidor recibió: " + mensaje);
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error con el cliente: " + e.Message);
            }
            finally
            {
                socket.Close();
                Console.WriteLine("Cliente desconectado.");
            }
        }
    }
}
