using System.Windows;
using space_shooter_game;
using NewTicTacTOE;
using Packman_Game;
using Cripto_Coin;
using BlackJackProject;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace GameCenter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FlappyBird_Click(object sender, RoutedEventArgs e)
        {
            var flappyBirdWindow = new Flappy_Bird_Game.MainWindow();
            flappyBirdWindow.Closed += (s, args) => this.Show();
            this.Hide();
            flappyBirdWindow.Show();
        }

        private void NewTicTacToe_Click(object sender, RoutedEventArgs e)
        {
            var newTicTacToeWindow = new NewTicTacTOE.MainWindow();
            newTicTacToeWindow.Closed += (s, args) => this.Show();
            this.Hide();
            newTicTacToeWindow.Show();
        }

        private void Pacman_Click(object sender, RoutedEventArgs e)
        {
            var pacmanWindow = new Packman_Game.MainWindow();
            pacmanWindow.Closed += (s, args) => this.Show();
            this.Hide();
            pacmanWindow.Show();
        }

        private void SpaceShooter_Click(object sender, RoutedEventArgs e)
        {
            var spaceShooterWindow = new space_shooter_game.MainWindow();
            spaceShooterWindow.Closed += (s, args) => this.Show();
            this.Hide();
            spaceShooterWindow.Show();
        }

        private void BlackJack_Click(object sender, RoutedEventArgs e)
        {
            var blackjackWindow = new BlackjackGame.MainWindow();
            blackjackWindow.Closed += (s, args) => this.Show();
            this.Hide();
            blackjackWindow.Show();
        }

        private void Countries_Click(object sender, RoutedEventArgs e)
        {
            var countriesWindow = new CountriesProject.MainWindow();
            countriesWindow.Closed += (s, args) => this.Show();
            this.Hide();
            countriesWindow.Show();
        }

        private void CriptoCoin_Click(object sender, RoutedEventArgs e)
        {
            var criptoCoinWindow = new Cripto_Coin.MainWindow();
            criptoCoinWindow.Closed += (s, args) => this.Show();
            this.Hide();
            criptoCoinWindow.Show();
        }

        // Breathing Effect for Buttons
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                AnimateButton(button, 1.2); // Scale up
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                AnimateButton(button, 1.0); // Scale back to original
            }
        }

        private void AnimateButton(Button button, double scaleTo)
        {
            ScaleTransform scaleTransform = new ScaleTransform(1, 1);
            button.RenderTransform = scaleTransform;

            DoubleAnimation scaleAnimation = new DoubleAnimation
            {
                To = scaleTo,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                AutoReverse = true
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Title breathing effect
            AnimateTitle();
        }

        private void AnimateTitle()
        {
            DoubleAnimation titleAnimation = new DoubleAnimation
            {
                From = 1,
                To = 1.2,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            TitleScale.BeginAnimation(ScaleTransform.ScaleXProperty, titleAnimation);
            TitleScale.BeginAnimation(ScaleTransform.ScaleYProperty, titleAnimation);
        }
    }
}
