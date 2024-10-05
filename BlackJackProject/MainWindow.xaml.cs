using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using BlackJackProject;



namespace BlackjackGame
{
    public partial class MainWindow : Window
    {
        public BlackjackGame game;

        private bool isPlayerBusted;

        public MainWindow()
        {
            InitializeComponent();
            StartNewGame();
        }
        

        private void StartNewGame()
        {
            game = new BlackjackGame();
            game.DealCard(true);
            game.DealCard(true);
            game.DealCard(false);
            game.DealCard(false);
            UpdateScores();
            UpdateCardDisplay();
            UpdateGameStatus("Hit or Stand!");

            isPlayerBusted = false;
            HitButton.IsEnabled = true;
        }

        private async void HitButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPlayerBusted) return;

            game.PlayerHit();
            UpdateScores();
            UpdateCardDisplay();

            if (game.CalculateScore(game.PlayerHand) > 21)
            {
                UpdateGameStatus("Player Busted!");
                isPlayerBusted = true;
                HitButton.IsEnabled = false;
            }
        }

        private async void StandButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPlayerBusted) return;

            game.PlayerStand();
            await ShowDealerCardsAsync();
            UpdateCardDisplay();

            int playerScore = game.CalculateScore(game.PlayerHand);
            int dealerScore = game.CalculateScore(game.DealerHand);

            if (dealerScore > 21)
            {
                UpdateGameStatus("Dealer Busted! Player Wins!");
            }
            else if (playerScore > dealerScore)
            {
                UpdateGameStatus("Player Wins!");
            }
            else if (playerScore < dealerScore)
            {
                UpdateGameStatus("Dealer Wins!");
            }
            else
            {
                UpdateGameStatus("It's a Tie!");
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        private void RulesButton_Click(object sender, RoutedEventArgs e)
        {
            RulesPage rulesPage = new RulesPage();
            rulesPage.Show();
        }

        private void UpdateScores()
        {
            int playerScore = game.CalculateScore(game.PlayerHand);
            int dealerScore = game.CalculateScore(game.DealerHand);
            PlayerScoreLabel.Text = $"Player: {playerScore}";
            DealerScoreLabel.Text = $"Dealer: {dealerScore}";
        }

        private void UpdateCardDisplay()
        {
            for (int i = 0; i < 5; i++)
            {
                var imageControl = (Image)this.FindName($"PlayerCardImage{i + 1}");
                if (i < game.PlayerHand.Count && imageControl != null)
                {
                    imageControl.Source = LoadImage($"{game.PlayerHand[i].ImageName}.jpg");
                }
                else if (imageControl != null)
                {
                    imageControl.Source = null;
                }
            }

            for (int i = 0; i < 5; i++)
            {
                var imageControl = (Image)this.FindName($"DealerCardImage{i + 1}");
                if (i < game.DealerHand.Count && imageControl != null)
                {
                    imageControl.Source = LoadImage($"{game.DealerHand[i].ImageName}.jpg");
                }
                else if (imageControl != null)
                {
                    imageControl.Source = null;
                }
            }
        }

        private BitmapImage LoadImage(string imageName)
        {
            try
            {
                var uri = new Uri($"pack://application:,,,/BlackJackProject;component/BlackJackImages/{imageName}", UriKind.Absolute);
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = uri;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                return image;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {imageName}. Exception: {ex.Message}", "Image Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private async Task ShowDealerCardsAsync()
        {
            int cardIndex = 0;
            foreach (var card in game.DealerHand)
            {
                var imageControl = (Image)this.FindName($"DealerCardImage{cardIndex + 1}");
                if (imageControl != null)
                {
                    imageControl.Source = LoadImage($"{card.ImageName}.jpg");
                }

                await Task.Delay(1000);
                cardIndex++;
                UpdateScores();
            }
        }

        private void UpdateGameStatus(string status)
        {
            GameStatusText.Text = status;
        }
        private void BackToMainButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // סגירת חלון הבלאק ג'ק
        }
    }
}
