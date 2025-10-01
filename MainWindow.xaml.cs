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

        string buy_type = "內用";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {
            finalOrder.Text = $"用餐方式: {buy_type}\n";
            finalOrder.Text += "餐點訂單如下:\n";
            double total = 0;
            int index = 0;

            foreach (var item in menu)
            {
                string foodItem = item.Key;
                int price = item.Value;

                if (orders.TryGetValue(foodItem, out int amount) && amount > 0)
                {
                    index++;
                    int subTotal = price * amount;
                    finalOrder.Text += $"{index}: {foodItem} {price}元，{amount}個，共{subTotal}元\n";
                    total += subTotal;
                }
            }

            double discount = 0;

            if (total > 500)
            {
                discount = total * 0.2;
                total *= 0.8;
            }
            else if (total > 300)
            {
                discount = total * 0.15;
                total *= 0.85;
            }
            else if (total > 200)
            {
                discount = total * 0.1;
                total *= 0.9;
            }

            finalOrder.Text += $"共折價 {discount:F0} 元\n";
            finalOrder.Text += $"總計: {total:F0} 元";
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var targetSlider = sender as Slider;
            if (targetSlider == null) return;

            int amount = (int)targetSlider.Value;

            var targetStackPanel = targetSlider.Parent as StackPanel;
            if (targetStackPanel == null) return;

            var nameLabel = targetStackPanel.Children[1] as Label; // 餐點名稱
            var checkBox = targetStackPanel.Children[0] as CheckBox;

            if (nameLabel == null || checkBox == null) return;

            string foodName = nameLabel.Content.ToString();

            if (menu.ContainsKey(foodName))
            {
                if (checkBox.IsChecked == true && amount > 0)
                {
                    orders[foodName] = amount;
                }
                else
                {
                    orders.Remove(foodName);
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null) return;

            var parent = checkBox.Parent as StackPanel;
            if (parent == null) return;

            var nameLabel = parent.Children[1] as Label;
            var slider = parent.Children[3] as Slider;

            if (nameLabel == null || slider == null) return;

            string foodName = nameLabel.Content.ToString();

            int amount = (int)slider.Value;
            if (amount == 0)
            {
                slider.Value = 1;
                amount = 1;
            }

            orders[foodName] = amount;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null) return;

            var parent = checkBox.Parent as StackPanel;
            if (parent == null) return;

            var nameLabel = parent.Children[1] as Label;
            if (nameLabel == null) return;

            string foodName = nameLabel.Content.ToString();
            orders.Remove(foodName);
        }

        private void radioBtnChecked(object sender, RoutedEventArgs e)
        {
            RadioButton targetRadioBtn = sender as RadioButton;
            if (targetRadioBtn != null)
            {
                buy_type = targetRadioBtn.Content.ToString();
            }
        }
    }
}
