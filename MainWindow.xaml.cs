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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_Changed(object sender, TextChangedEventArgs e)
        {
            var TargetTextBox = sender as TextBox;
            int amount;
            bool success = int.TryParse(TargetTextBox.Text, out amount);
            if (!success || amount <= 0)
            {

            }
            else
            {

            }
        }
    }
}