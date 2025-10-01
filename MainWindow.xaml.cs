using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        Dictionary<string, int> menu = new Dictionary<string, int>
        {
            {"food1", 40 },
            {"food2", 60 },
            {"food3", 65 },
            {"food4", 70 },
            {"food5", 80 },
            {"food6", 90 },
        };

        Dictionary<string, int> orders = new Dictionary<string, int>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {
            finalOrder.Text = "飲料訂單如下:\n";
            int total = 0;
            int index = 0;

            foreach (var item in menu)
            {
                string drinkItem = item.Key;
                int price = item.Value;

                if (orders.TryGetValue(drinkItem, out int amount) && amount > 0)
                {
                    index++;
                    int subTotal = price * amount;
                    finalOrder.Text += $"{index} : {drinkItem} {price}元，{amount}個，共{subTotal}元\n";
                    total += subTotal;
                }
            }

            finalOrder.Text += $"總計:{total}元";
        }

        private void TextBox_Changed(object sender, TextChangedEventArgs e)
        {
            var TargetTextBox = sender as TextBox;
            if (TargetTextBox == null) return;

            int amount;
            bool success = int.TryParse(TargetTextBox.Text, out amount);

            var targetStackPanel = TargetTextBox.Parent as StackPanel;
            var targetNameLabel = targetStackPanel?.Children[0] as Label;

            if (targetNameLabel == null) return;

            string drinkName = targetNameLabel.Content.ToString();

            if (!success || amount < 0)
            {
                MessageBox.Show("請輸入正整數");
                return;
            }

            if (menu.ContainsKey(drinkName))
            {
                orders[drinkName] = amount;
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var targetSlider = sender as Slider;
            if (targetSlider == null) return;

            int amount = (int)targetSlider.Value;

            var targetStackPanel = targetSlider.Parent as StackPanel;
            var targetNameLabel = targetStackPanel?.Children[0] as Label;
            if (targetNameLabel == null) return;

            string drinkName = targetNameLabel.Content.ToString();

            if (menu.ContainsKey(drinkName))
            {
                orders[drinkName] = amount;
            }
        }
    }
}
