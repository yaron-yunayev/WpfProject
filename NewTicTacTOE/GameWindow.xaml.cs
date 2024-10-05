using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NewTicTacTOE
{
    public partial class GameWindow : Window
    {
        private bool isPlayerXTurn = true;
        private bool isSinglePlayerMode;
        private Button[,] buttons;
        private List<GameRecord> gameHistory;
        private string player1Name;
        private string player2Name;

        public GameWindow(string playerName1, string playerName2, bool singlePlayerMode, List<GameRecord> history)
        {
            InitializeComponent();
            player1Name = playerName1;
            player2Name = playerName2;
            PlayerNameText.Text = $"{player1Name} vs {player2Name}";
            isSinglePlayerMode = singlePlayerMode;
            gameHistory = history;
            InitializeGame();
        }

        private void InitializeGame()
        {
            buttons = new Button[3, 3] {
                { Button1, Button2, Button3 },
                { Button4, Button5, Button6 },
                { Button7, Button8, Button9 }
            };
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button.Content == null && (isPlayerXTurn || !isSinglePlayerMode))
            {
                button.Content = isPlayerXTurn ? "X" : "O";
                isPlayerXTurn = !isPlayerXTurn;

                if (CheckForWinner())
                {
                    string winner = isPlayerXTurn ? player2Name : player1Name;
                    ShowWinner(winner);
                }
                else if (IsBoardFull())
                {
                    ShowDraw();
                }

                if (isSinglePlayerMode && !isPlayerXTurn)
                {
                    await Task.Delay(2000); // Wait 2 seconds
                    ComputerMove();
                }
            }
        }

        private void ComputerMove()
        {
            Random random = new Random();
            int row, col;
            do
            {
                row = random.Next(3);
                col = random.Next(3);
            } while (buttons[row, col].Content != null);

            buttons[row, col].Content = "O";
            isPlayerXTurn = true;

            if (CheckForWinner())
            {
                ShowWinner(player2Name);
            }
            else if (IsBoardFull())
            {
                ShowDraw();
            }
        }

        private bool CheckForWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                if (buttons[i, 0].Content != null && buttons[i, 0].Content == buttons[i, 1].Content && buttons[i, 1].Content == buttons[i, 2].Content)
                {
                    return true;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (buttons[0, i].Content != null && buttons[0, i].Content == buttons[1, i].Content && buttons[1, i].Content == buttons[2, i].Content)
                {
                    return true;
                }
            }

            if (buttons[0, 0].Content != null && buttons[0, 0].Content == buttons[1, 1].Content && buttons[1, 1].Content == buttons[2, 2].Content)
            {
                return true;
            }

            if (buttons[0, 2].Content != null && buttons[0, 2].Content == buttons[1, 1].Content && buttons[1, 1].Content == buttons[2, 0].Content)
            {
                return true;
            }

            return false;
        }

        private bool IsBoardFull()
        {
            foreach (Button button in buttons)
            {
                if (button.Content == null)
                {
                    return false;
                }
            }
            return true;
        }

        private void ShowWinner(string winner)
        {
            var message = $"{winner} wins!";
            var dialog = new CustomDialog(message);
            dialog.Owner = this; // Set owner to ensure it appears in front of the main window
            dialog.ShowDialog();
            RecordGameResult(winner);
            ResetBoard();
        }

        private void ShowDraw()
        {
            var dialog = new CustomDialog("Draw!");
            dialog.Owner = this;
            dialog.ShowDialog();
            RecordGameResult("Draw");
            ResetBoard();
        }

        private void RecordGameResult(string result)
        {
            var gameRecord = new GameRecord
            {
                Date = DateTime.Now.ToString("dd/MM/yyyy"),
                Player1 = player1Name,
                Player2 = player2Name,
                Result = result
            };
            gameHistory.Add(gameRecord);
        }

        private void ResetBoard()
        {
            foreach (Button button in buttons)
            {
                button.Content = null;
            }
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow historyWindow = new HistoryWindow(gameHistory);
            historyWindow.Show();
        }
    }

    public class GameRecord
    {
        public string Date { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string Result { get; set; }
    }
}
