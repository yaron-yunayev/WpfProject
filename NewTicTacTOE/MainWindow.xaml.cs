using System.Windows;

namespace NewTicTacTOE
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlayWithComputer_Click(object sender, RoutedEventArgs e)
        {
            string player1Name = Player1NameTextBox.Text;
            string player2Name = "Computer";
            GameWindow gameWindow = new GameWindow(player1Name, player2Name, true, new List<GameRecord>());
            gameWindow.Show();
        }

        private void PlayWithAnotherPlayer_Click(object sender, RoutedEventArgs e)
        {
            string player1Name = Player1NameTextBox.Text;
            string player2Name = Player2NameTextBox.Text;
            GameWindow gameWindow = new GameWindow(player1Name, player2Name, false, new List<GameRecord>());
            gameWindow.Show();
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
