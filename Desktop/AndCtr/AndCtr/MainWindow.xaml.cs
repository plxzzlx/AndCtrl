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
using System.Windows.Forms;

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

        WindowState ws;
        WindowState wsl;
        NotifyIcon notifyIcon;


        public MainWindow()
        {
            InitializeComponent();

            wsl = WindowState;

            InitIcon();

            InitIpConfig();

        }



        private void InitIpConfig()
        {
            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in IpEntry.AddressList)
            {
                if (ip.IsIPv6LinkLocal ||
                   ip.IsIPv6Teredo) continue;
                txt_IP.Text = ip.ToString();
                txt_IP_List.AppendText(ip.ToString() + "\n");
            }
        }

        private void InitIcon()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.BalloonTipText = "AndCtr已经启动！";             //设置程序启动时显示的文本
            this.notifyIcon.Text = "AndCtr";                                //最小化到托盘时，鼠标点击时显示的文本
            this.notifyIcon.Icon = new System.Drawing.Icon("desktop.ico");   //程序图标
            this.notifyIcon.Visible = true;
            notifyIcon.MouseDoubleClick += OnNotifyIconDoubleClick;
            //this.notifyIcon.ShowBalloonTip(1000);
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
                System.Windows.MessageBox.Show(ex.ToString());
            }
            txt_Output.DataContext = server;
        }

        private void OnNotifyIconDoubleClick(object sender, EventArgs e)
        {
            this.Show();
            WindowState = wsl;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            ws = WindowState;
            if (ws == WindowState.Minimized)
                this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (server != null)
                server.Stop();
            this.notifyIcon.Visible = false;
        }
    }
}
