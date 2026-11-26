using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZerbitzariaTxat
{
    public class Zerbitzaria
    {
        private const string ServerIp = "172.0.0.1";
        private const int ServerPort = 2000;

        public void ZerbitzariaPiztu()
        {
            TcpListener listener = null;
            int cont = 0;
            try
            {

                listener = new TcpListener(IPAddress.Any, ServerPort);
                listener.Start();
                Console.WriteLine($"Zerbitzaria martxan dago {ServerIp}:{ServerPort} helbidean.");

                while (true)
                {
                    TcpClient bezeroa = listener.AcceptTcpClient();
                    cont++;
                    Console.WriteLine($"Bezero berria konektatu da. Konektatutako bezero kopurua: {cont}");

                    Thread bezero = new Thread(() => BezeroKontroladorea(bezeroa, cont));
                    bezero.Start();



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
        }
        public void BezeroKontroladorea(TcpClient bezeroa, int cont)
        { 
            NetworkStream network= bezeroa.GetStream();
            StreamReader reader = new StreamReader(network);
            StreamWriter writer = new StreamWriter(network) { AutoFlush=true};

            writer.WriteLine($"");
        }
    }
}
