using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using ZerbitzariaTxat;
using BerzeroTxat;


namespace ZerbitzariaTxat
{
    /// <summary>
    /// Interaction logic for HasierakoLehioa.xaml
    /// </summary>
    public partial class HasierakoLehioa : Window
    {
        private Zerbitzaria zer;
        private Thread zerbitzariaThread;
        public HasierakoLehioa()
        {
            InitializeComponent();
        }

        private void btnArrancar_Click(object sender, RoutedEventArgs e)
        {
            btnArrancar.IsEnabled = false;
            btnArrancar.Content = "Zerbitzaria pizten...";
            btnParar.IsEnabled = true;

            zer = new Zerbitzaria();
            zerbitzariaThread = new Thread(zer.ZerbitzariaPiztu);
            zerbitzariaThread.IsBackground = true;
            zerbitzariaThread.Start();

            Console.WriteLine("Pizten saiatzen");
            BerzeroTxat.Txata bezeroa = new BerzeroTxat.Txata();
            bezeroa.Show();

        }

        private void btnParar_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
