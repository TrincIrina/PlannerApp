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
using System.Windows.Shapes;

namespace PlannerInterface
{
    /// <summary>
    /// Логика взаимодействия для AddUpDealWindow.xaml
    /// </summary>
    public partial class AddUpDealWindow : Window
    {
        public AddUpDealWindow(string deal)
        {
            InitializeComponent();
        }

        private void addUpButton_Click(object sender, RoutedEventArgs e)
        {

            Close();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
