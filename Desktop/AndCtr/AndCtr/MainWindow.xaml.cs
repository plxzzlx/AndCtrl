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

namespace AndCtr
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        NetworkServer server;
        String IP;
        int Port;
        public MainWindow()
        {
            InitializeComponent();

            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in IpEntry.AddressList)
            {
                if (ip.IsIPv6LinkLocal ||
                   ip.IsIPv6Teredo) continue;
                txt_IP.Text = ip.ToString();   
                txt_IP_List.AppendText(ip.ToString() + "\n");
            }
        }

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IP = txt_IP.Text;
                Port = int.Parse(txt_Port.Text);
                server = new NetworkServer(IP, Port);
                server.Start();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            txt_Output.DataContext = server;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (server != null)
            {
                server.Stop();
            }
        }
    }
}
