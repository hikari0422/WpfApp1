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

namespace WpfApp1 {
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int num = Convert.ToInt32(input.Text);
                if (isPrime(num))
                {
                    ans.Content = $"{num}是質數。";
                }
                else
                {
                    ans.Content = $"{num}不是質數。";
                }

            } catch (FormatException)
            {
                MessageBox.Show("error");
            }            
        }

        bool isPrime(int n)
        {
            if (n <= 1)
            {
                return false;
            }

            for (int i = 2; i * i <= n; ++i)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}