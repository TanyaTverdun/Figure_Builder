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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            infoFrame.Background = new SolidColorBrush(Color.FromRgb(41, 41, 41));
        }
        private void writeButton_click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        public void setInfo(string[] info)
        {
            label_type.Content = info[0];
            label_subType.Content = info[1];
            label_color.Content = info[2];
            label_sideA.Content = info[3];
            label_sideB.Content = info[4];
            label_sideC.Content = info[5];
            label_sideD.Content = info[6];
            label_angleA.Content = info[7];
            label_angleB.Content = info[8];
            label_angleC.Content = info[9];
            label_angleD.Content = info[10];
            label_radius_R.Content = info[11];
            label_radius_r.Content = info[12];
            label_perimetr.Content = info[13];
            label_area.Content = info[14];
            label_R.Content = info[15];
            label_r.Content = info[16];
            label_middleLine.Content = info[17];
        }
    }
}
