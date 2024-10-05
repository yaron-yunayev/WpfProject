using System.Collections.Generic;
using System.Windows;

namespace NewTicTacTOE
{
    public partial class HistoryWindow : Window
    {
        public HistoryWindow(List<GameRecord> gameHistory)
        {
            InitializeComponent();
            HistoryListBox.ItemsSource = gameHistory;
        }
    }
}
