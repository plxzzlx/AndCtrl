using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Controls;
using System.ComponentModel;

namespace AndCtr
{
    public class NetworkServer : INotifyPropertyChanged
    {
        Socket serverSocket;
        Thread myThread;
        Thread receiveThread;
        bool IsSeverConnected = false;

        String IP = null;
        int Port;

        private String output;
        public String Output {
            get { return output; }
            set
            {
                output = value;
                OnPropertyChanged("Output");
            }
        }

        public NetworkServer(String IP,int Port)
        {
            this.IP = IP;
            this.Port = Port;
        }

        private bool SetUpServer()
        {
            try
            {
                //服务器IP地址
                IPAddress ip = IPAddress.Parse(IP);
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //绑定IP地址：端口
                serverSocket.Bind(new IPEndPoint(ip, Port));
                //设定最多10个排队连接请求
                serverSocket.Listen(1);
                Output += "Start Suscessfully!\n" + "Server IP=" + IP + ", Port=" + Port + "\n";
                
                myThread = new Thread(ListenClientConnect);
                IsSeverConnected = true;
            }
            catch (System.Exception ex)
            {
                IsSeverConnected = false;
                Console.WriteLine(ex.ToString());
            }
            return IsSeverConnected;
        }

        public void Start()
        {
            if(!IsSeverConnected && SetUpServer())
                myThread.Start();
        }

        public void Stop()
        {
            if (IsSeverConnected)
            {
                serverSocket.Close();
                myThread.Abort();
                if (receiveThread != null)
                {
                    receiveThread.Abort();
                }
            }
        }

        /// <summary>
        /// 监听客户端连接
        /// </summary>
        private void ListenClientConnect()
        {
            while (true)
            {
                try
                {
                    Socket clientSocket = serverSocket.Accept();

                    Output += "Accept a client! IP=" + clientSocket.RemoteEndPoint.ToString() + "\n";
                    Output += "Client: " + "Server Say Hello!\n";

                    clientSocket.Send(Encoding.ASCII.GetBytes("Server Say Hello!"));

                    receiveThread = new Thread(ReceiveMessage);

                    receiveThread.Start(clientSocket);
                }
                catch (System.Exception ex)
                {
                    Output += ex.ToString(); 
                }
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="clientSocket"></param>
        private void ReceiveMessage(object clientSocket)
        {
            byte[] result = new byte[1024];
            Socket myClientSocket = (Socket)clientSocket;
            MouseAction mAction = new MouseAction();
            while (true)
            {
                try
                {
                    int receiveNumber = myClientSocket.Receive(result);
                    String Event = Encoding.ASCII.GetString(result, 0, receiveNumber);
                    Output = "Client: " + Event + "\n";
                    mAction.OnEvent(Event);
                    myClientSocket.Send(Encoding.ASCII.GetBytes("Sever Recive : " + Event));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    Output = "断开连接!";
                    break;
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
