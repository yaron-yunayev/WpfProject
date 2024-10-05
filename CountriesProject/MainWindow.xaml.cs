using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CountriesProject
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Country> Countries { get; set; } = new ObservableCollection<Country>();
        private ObservableCollection<Country> _allCountries = new ObservableCollection<Country>();

        public static HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            LoadCountriesDataAsync();
            // Set placeholder visibility initially
            PlaceholderTextBlock.Visibility = string.IsNullOrWhiteSpace(SearchTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private async Task LoadCountriesDataAsync()
        {
            try
            {
                string json = await client.GetStringAsync("https://restcountries.com/v3.1/all");

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var countries = JsonSerializer.Deserialize<ObservableCollection<Country>>(json, options);
                if (countries != null)
                {
                    foreach (var c in countries)
                    {
                        _allCountries.Add(c);
                    }
                    // Bind the DataGrid to the Countries collection
                    CountriesDataGrid.ItemsSource = Countries;
                    // Initialize the Countries collection
                    UpdateCountriesCollection(_allCountries.ToList());
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log or show a message)
                MessageBox.Show($"Error loading countries data: {ex.Message}");
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Update placeholder visibility
            PlaceholderTextBlock.Visibility = string.IsNullOrWhiteSpace(SearchTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;

            // Filter the countries based on search text
            string searchText = SearchTextBox.Text.ToLower();
            var filteredCountries = _allCountries
                .Where(c => c.Name.Common.ToLower().Contains(searchText))
                .ToList();

            UpdateCountriesCollection(filteredCountries);
        }

        private void RegionFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedRegion = (RegionFilterComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedRegion == "All Regions")
            {
                UpdateCountriesCollection(_allCountries.ToList());
            }
            else
            {
                var filteredCountries = _allCountries
                    .Where(c => c.Region.ToLower() == selectedRegion.ToLower())
                    .ToList();

                UpdateCountriesCollection(filteredCountries);
            }
        }

        private void UpdateCountriesCollection(List<Country> countries)
        {
            // Clear and re-add items to the ObservableCollection
            Countries.Clear();
            foreach (var country in countries)
            {
                Countries.Add(country);
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
           this.Close();
        }

    }
}
