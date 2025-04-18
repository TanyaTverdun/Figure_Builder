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

namespace Figure_Builder
{
    /// <summary>
    /// Interaction logic for WriteToFile.xaml
    /// </summary>
    public partial class WriteToFile : Window
    {
        public int SelectedOption { get; private set; }
        public WriteToFile()
        {
            InitializeComponent();
            writeToFile.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(41, 41, 41));
        }
        private void oneButton_click(object sender, RoutedEventArgs e)
        {
            SelectedOption = 0;
            DialogResult = true;
            Close();
        }
        private void allButton_click(object sender, RoutedEventArgs e)
        {
            SelectedOption = 1;
            DialogResult = true;
            Close();
        }
        private void cancelButton_click(object sender, RoutedEventArgs e)
        {
            SelectedOption = 2;
            DialogResult = false;
            Close();
        }
    }
}
