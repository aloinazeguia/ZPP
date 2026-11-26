using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace BerzeroTxat
{
    public class Bezeroa
    {
        private const string ServerIp = "127.0.0.1";
        private const int Port = 2000;
         
        public void Main()
        {
            
           try
           {
                TcpClient bezero = new TcpClient();
                bezero.Connect(ServerIp, Port);

                NetworkStream stream = bezero.GetStream();
                StreamReader reader= new StreamReader(stream);
                 StreamWriter writer= new StreamWriter(stream) { AutoFlush = true};

                Console.WriteLine($"Txatera konektatuta");
                while (true)
                {
                  
                }

           }
           catch { }
           finally { }
         }
    }
}
