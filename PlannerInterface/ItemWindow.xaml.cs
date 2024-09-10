using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlannerInterface
{
    /// <summary>
    /// Логика взаимодействия для ItemWindow.xaml
    /// </summary>
    public partial class ItemWindow : Window
    {
        private Socket client;
        private byte[] buf = new byte[1024];
        private int bytesReceived;
        private string reply;
        private string command;

        private string nameList = string.Empty;
        private string[] items;
        private int ind = 0;
        public ItemWindow(string deal)
        {
            InitializeComponent();

            // ip адрес и порт сокета сервера
            string serverIpStr = "127.0.0.1";
            int serverPort = 1024;

            // 1. подготовить endpoint сервера
            IPAddress serverIp = IPAddress.Parse(serverIpStr);
            IPEndPoint serverEndpoint = new IPEndPoint(serverIp, serverPort);

            // 2. создать сокет клиента
            client = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.IP
            );

            // 3. иницировать подключение к серверу            
            client.Connect(serverEndpoint);

            nameList = deal;
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
