using System.Windows;

namespace NewTicTacTOE
{
    public partial class CustomDialog : Window
    {
        public CustomDialog(string message)
        {
            InitializeComponent();
            MessageTextBlock.Text = message;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
