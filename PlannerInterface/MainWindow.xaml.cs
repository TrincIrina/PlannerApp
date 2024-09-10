using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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

namespace PlannerInterface
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Socket client;
        private byte[] buf = new byte[1024];
        private int bytesReceived;
        private string reply;
        private string command;
        
        public MainWindow()
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

            bytesReceived = client.Receive(buf);
            reply = Encoding.UTF8.GetString(buf, 0, bytesReceived);
            MessageBox.Show(reply);

            //bytesReceived = client.Receive(buf);
            //reply = Encoding.UTF8.GetString(buf, 0, bytesReceived);
            //FillingList(reply);
        }
        // Загрузка существующих списков
        private void todoListButton_Click(object sender, RoutedEventArgs e)
        {
            // 1.
            command = todoListButton.Content.ToString();
            client.Send(Encoding.UTF8.GetBytes(command));
            //2.
            bytesReceived = client.Receive(buf);
            reply = Encoding.UTF8.GetString(buf, 0, bytesReceived);
            FillingList(reply);
        }        
        // Открытие списка дел
        private void openTodoListButton_Click(object sender, RoutedEventArgs e)
        {            
            DealWindow dealWindow = new DealWindow(todoListBox.SelectedItem.ToString());
            this.Hide();
            dealWindow.ShowDialog();
            this.Show();
        }
        // Добавление списка
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            // проверка на существование списка
            bool f = true;
            foreach(string item in todoListBox.Items)
            {
                if(item == itemTextBox.Text)
                {
                    f = false;
                    break;
                }
            }
            if (f)
            {
                // 1.
                command = addButton.Content.ToString();
                client.Send(Encoding.UTF8.GetBytes(command));
                // 2.
                command = itemTextBox.Text;
                client.Send(Encoding.UTF8.GetBytes(command));
                // 3.
                bytesReceived = client.Receive(buf);
                reply = Encoding.UTF8.GetString(buf, 0, bytesReceived);
                FillingList(reply);
            } else
            {
                MessageBox.Show("List already exists");
            }
        }        
        // Удаление списка
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            // 1.
            command = deleteButton.Content.ToString();
            client.Send(Encoding.UTF8.GetBytes(command));
            // 2.
            command = todoListBox.SelectedItem.ToString();
            client.Send(Encoding.UTF8.GetBytes(command));
            // 3.
            bytesReceived = client.Receive(buf);
            reply = Encoding.UTF8.GetString(buf, 0, bytesReceived);
            FillingList(reply);
        }
        // Обновление названия списка
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            // проверка на существование списка
            bool f = true;
            foreach (string item in todoListBox.Items)
            {
                if (item == itemTextBox.Text)
                {
                    f = false;
                    break;
                }
            }
            if (f)
            {
                // 1.
                command = updateButton.Content.ToString();
                client.Send(Encoding.UTF8.GetBytes(command));
                // 2.
                command = todoListBox.SelectedItem.ToString() + "." + itemTextBox.Text;
                client.Send(Encoding.UTF8.GetBytes(command));
                // 3.
                bytesReceived = client.Receive(buf);
                reply = Encoding.UTF8.GetString(buf, 0, bytesReceived);
                FillingList(reply);
            }
            else
            {
                MessageBox.Show("List already exists");
            }
        }
        // Выход из приложения
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Do you really want to close the app?",
                "Exit",
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                // 1.
                command = exitButton.Content.ToString();
                client.Send(Encoding.UTF8.GetBytes(command));
                // 2.            
                client.Shutdown(SocketShutdown.Both);   // завершить общение в 2 стороны
                client.Close();
                Close();
            }
        }
        // Вспомогательный метод: преобразование ответа в список
        private void FillingList(string reply)
        {
            todoListBox.Items.Clear();
            string[] list = reply.Split('/');
            for (int j = 0; j < list.Length; j++)
            {
                todoListBox.Items.Add(list[j]);
            }
        }
    }
}
