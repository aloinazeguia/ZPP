using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1.ariketa
{
 
    public partial class MainWindow : Window
    {
        // Abiarazitako FTP prozesua gordetzeko aldagai globala
        private Process ftpProcess;

        public MainWindow()
        {
            InitializeComponent();
        }

        // 1. "FTP Prozesua Abiarazi" botoiaren gertaera
        private void btnStartFtp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // C:\Windows\System32\ftp.exe prozesua abiarazi
                ftpProcess = Process.Start(@"C:\Windows\System32\ftp.exe");
                MessageBox.Show("FTP prozesua ondo abiarazi da.", "Arrakasta", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea prozesua hastean: {ex.Message}", "Errorea", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 2. "Prozesua Hil" botoiaren gertaera
        private void btnKillFtp_Click(object sender, RoutedEventArgs e)
        {
            // Egiaztatu prozesua sortuta dagoen eta oraindik exekutatzen ari den
            if (ftpProcess != null && !ftpProcess.HasExited)
            {
                // Hil aurretik bere IDa Label-ean erakutsi
                lblProcessId.Content = $"Ezabatutako prozesuaren IDa: {ftpProcess.Id}";

                // Prozesua amaitu
                ftpProcess.Kill();
              
            }
            else
            {
                MessageBox.Show("Ez dago abiarazitako FTP prozesu aktiborik.", "Oharra", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // 3. "Sistemako Prozesuak Erakutsi" botoiaren gertaera
        private void btnShowProcesses_Click(object sender, RoutedEventArgs e)
        {
            // ComboBox-a garbitu kargatu aurretik
            cmbProcesses.Items.Clear();

            // Sistemako prozesu guztiak berreskuratu
            Process[] processes = Process.GetProcesses();

            // Prozesu bakoitza ComboBox-ean gehitu
            foreach (Process p in processes)
            {
                try
                {
                    System.Windows.Controls.ComboBoxItem item = new System.Windows.Controls.ComboBoxItem
                    {
                        Content = $"{p.ProcessName} (ID: {p.Id})"
                    };

                    cmbProcesses.Items.Add(item);
                }
                catch
                {
                    // Sistemako prozesuren batek baimen murriztua badu, ez du aplikazioa geldituko
                }
            }

            if (cmbProcesses.Items.Count > 0)
            {
                cmbProcesses.SelectedIndex = 0;
            }
        }
    }
}