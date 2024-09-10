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
    /// Логика взаимодействия для DealWindow.xaml
    /// </summary>
    public partial class DealWindow : Window
    {
        private Socket client;
        private byte[] buf = new byte[1024];
        private int bytesReceived;
        private string reply;
        private string command;

        private string nameList = string.Empty;
        private string[] deals;
        private string name = "";
        private int ind = 0;
        public DealWindow(string list)
        {
            InitializeComponent();
                        
            nameList = list;
        }
        // Загрузка списка дел по названию
        private void dealsButton_Click(object sender, RoutedEventArgs e)
        {
            // 1.
            command = dealsButton.Content.ToString();
            client.Send(Encoding.UTF8.GetBytes(command));
            // 2.
            command = nameList;
            client.Send(Encoding.UTF8.GetBytes(command));
            // 3.
            bytesReceived = client.Receive(buf);
            reply = Encoding.UTF8.GetString(buf, 0, bytesReceived);
            FillingList(reply);
        }
        // Отметить дело как выполненное (перечеркивание текста)
        private void completedButton_Click(object sender, RoutedEventArgs e)
        {
            dealsListBox.SelectedItem = TextDecorations.Strikethrough;
            // !!! выделить название дела, присвоить command !!!
            client.Send(Encoding.UTF8.GetBytes(command));
        }        
        // Добавление дела в список
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            // 1.
            command = addButton.Content.ToString();
            client.Send(Encoding.UTF8.GetBytes(command));
            // 2.
            OpenAddUpDealWindow();
        }
        // Удаление дела из списка
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            // 1.
            command = deleteButton.Content.ToString();
            client.Send(Encoding.UTF8.GetBytes(command));
            // 2.
            string deal = dealsListBox.SelectedItem.ToString();
            // !!! выделить название дела, присвоить command !!!
            client.Send(Encoding.UTF8.GetBytes(command));
            // 3.
            bytesReceived = client.Receive(buf);
            reply = Encoding.UTF8.GetString(buf, 0, bytesReceived);
            FillingList(reply);
        }
        // Редактирование дела
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            // 1.
            command = updateButton.Content.ToString();
            client.Send(Encoding.UTF8.GetBytes(command));
            // 2.
            OpenAddUpDealWindow();
        }
        // Открытие списка действий для выполнения дела из списка
        private void openDealButton_Click(object sender, RoutedEventArgs e)
        {
            // 1.
            string deal = dealsListBox.SelectedItem.ToString();
            
            // 2.
            ItemWindow itemWindow = new ItemWindow(name);
            this.Hide();
            itemWindow.ShowDialog();
            this.Show();
        }
        // Закрытие окна
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        // Вспомогательный метод: преобразование ответа в список
        private void FillingList(string reply)
        {
            deals = reply.Split('/');
            dealsListBox.Items.Clear();
            foreach (string item in deals)
            {
                dealsListBox.Items.Add($"{ind++}. {item}");
            }
        }
        // Вспомогательный метод для открытия окна добавления/редактирования дела
        private void OpenAddUpDealWindow()
        {
            try
            {
                string deal = dealsListBox.SelectedItem.ToString();
                AddUpDealWindow window = new AddUpDealWindow(deal);
                this.Hide();
                window.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
    }
}
