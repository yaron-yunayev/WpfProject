using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Cripto_Coin
{
    public partial class MainWindow : Window
    {
        private List<Coin> _allCoins; // Store the complete list of coins

        public MainWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load data and bind to the DataGrid
            _allCoins = await GetCryptoData();
            CryptoGrid.ItemsSource = _allCoins;
        }

        private async Task<List<Coin>> GetCryptoData()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://api.coinlore.net/api/tickers/";
                var response = await client.GetStringAsync(url);
                var data = JsonDocument.Parse(response);
                var coins = data.RootElement.GetProperty("data").Deserialize<List<Coin>>();
                return coins;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();
            var filteredCoins = _allCoins
                .Where(c => c.symbol.ToLower().Contains(searchText) || c.name.ToLower().Contains(searchText))
                .ToList();

            UpdateCoinsCollection(filteredCoins);
        }

        private void UpdateCoinsCollection(List<Coin> coins)
        {
            CryptoGrid.ItemsSource = coins;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    // Converter to handle placeholder visibility
    public class EmptyStringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
