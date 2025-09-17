using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            ansBox.Clear();

            try
            {
                int n = Convert.ToInt32(input.Text);
                for (int i = 1; i <= 9; i++)
                {
                    ansBox.Text += $"{i} x {n} = {i * n}\n";
                }
            }
            catch (FormatException)
            {
                input.Clear();
                MessageBox.Show("請輸入有效數字！");
            }
        }
    }
}
