using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        Dictionary<string, int> menu = new Dictionary<string, int>();
        Dictionary<string, int> orders = new Dictionary<string, int>();
        string buy_type = "內用";

        public MainWindow()
        {
            InitializeComponent();
            inputDrinkItem(menu);
            displayDrinkMenu(menu);
        }

        private void displayDrinkMenu(Dictionary<string, int> menu)
        {
            drinkItemList.Height = menu.Count * 50;

            foreach (var drink in menu)
            {
                var sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(2),
                    Background = Brushes.LightBlue,
                    Height = 35
                };

                var cb = new CheckBox
                {
                    Content = drink.Key,
                    FontFamily = new FontFamily("微軟正黑體"),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.DarkBlue,
                    Width = 150,
                    Margin = new Thickness(2),
                    VerticalContentAlignment = VerticalAlignment.Center
                };

                cb.Checked += CheckBox_Checked;
                cb.Unchecked += CheckBox_Unchecked;

                var lb_price = new Label
                {
                    Content = $"{drink.Value} 元",
                    FontFamily = new FontFamily("微軟正黑體"),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.DarkRed,
                    Width = 60,
                    Margin = new Thickness(2),
                    VerticalContentAlignment = VerticalAlignment.Center
                };

                var sl = new Slider
                {
                    Value = 0,
                    Minimum = 0,
                    Maximum = 20,
                    Margin = new Thickness(2),
                    Width = 150,
                    VerticalAlignment = VerticalAlignment.Center,
                    IsSnapToTickEnabled = true
                };

                sl.ValueChanged += slider_ValueChanged;

                var lb_amount = new Label
                {
                    Content = "0",
                    FontFamily = new FontFamily("微軟正黑體"),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.DarkGreen,
                    Width = 30,
                    Margin = new Thickness(2),
                    VerticalContentAlignment = VerticalAlignment.Center
                };

                Binding myBinding = new Binding("Value");
                myBinding.Source = sl;
                lb_amount.SetBinding(Label.ContentProperty, myBinding);

                sp.Children.Add(cb);
                sp.Children.Add(lb_price);
                sp.Children.Add(sl);
                sp.Children.Add(lb_amount);

                drinkItemList.Children.Add(sp);
            }
        }

        private void inputDrinkItem(Dictionary<string, int> menu)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "選擇飲料品項檔案";
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                string[] lines = File.ReadAllLines(fileName);

                foreach (var line in lines)
                {
                    string[] tokens = line.Split(',');
                    string drinkName = tokens[0];
                    int price = int.Parse(tokens[1]);
                    menu.Add(drinkName, price);
                }
            }
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

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFile.FileName = "訂購內容";
            if (saveFile.ShowDialog() == true)
            {
                File.WriteAllText(saveFile.FileName, finalOrder.Text);
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var targetSlider = sender as Slider;
            if (targetSlider == null) return;

            int amount = (int)targetSlider.Value;

            var targetStackPanel = targetSlider.Parent as StackPanel;
            if (targetStackPanel == null) return;

            var checkBox = targetStackPanel.Children[0] as CheckBox;
            if (checkBox == null) return;

            string foodName = checkBox.Content.ToString();

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

            var slider = parent.Children[2] as Slider;
            if (slider == null) return;

            string foodName = checkBox.Content.ToString();

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

            string foodName = checkBox.Content.ToString();
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
