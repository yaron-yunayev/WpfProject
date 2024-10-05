using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Packman_Game
{
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        bool goLeft, goRight, goDown, goUp;
        bool noLeft, noRight, noDown, noUp;
        int speed = 8;
        Rect pacmanHitBox;
        int ghostSpeed = 10;
        int ghostMoveStep = 160;
        int currentGhostStep;
        int score = 0;

        private void btnBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Stop();
            this.Close();
        }

        public MainWindow()
        {
            InitializeComponent();
            GameSetUp();
        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && !noLeft)
            {
                goRight = goUp = goDown = false;
                noRight = noUp = noDown = false;
                goLeft = true;
                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2);
            }
            if (e.Key == Key.Right && !noRight)
            {
                noLeft = noUp = noDown = false;
                goLeft = goUp = goDown = false;
                goRight = true;
                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2);
            }
            if (e.Key == Key.Up && !noUp)
            {
                noRight = noDown = noLeft = false;
                goRight = goDown = goLeft = false;
                goUp = true;
                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2);
            }
            if (e.Key == Key.Down && !noDown)
            {
                noUp = noLeft = noRight = false;
                goUp = goLeft = goRight = false;
                goDown = true;
                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2);
            }
        }

        private void GameSetUp()
        {
            MyCanvas.Focus();
            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();
            currentGhostStep = ghostMoveStep;

            try
            {
                // Load images using local file paths
                ImageBrush pacmanImage = new ImageBrush();
                pacmanImage.ImageSource = new BitmapImage(new Uri(@"C:\Users\user\source\repos\Packmen\PackmanImages\pacman.jpg", UriKind.Absolute));
                pacman.Fill = pacmanImage;

                ImageBrush redGhost = new ImageBrush();
                redGhost.ImageSource = new BitmapImage(new Uri(@"C:\Users\user\source\repos\Packmen\PackmanImages\red.jpg", UriKind.Absolute));
                redGuy.Fill = redGhost;

                ImageBrush orangeGhost = new ImageBrush();
                orangeGhost.ImageSource = new BitmapImage(new Uri(@"C:\Users\user\source\repos\Packmen\PackmanImages\orange.jpg", UriKind.Absolute));
                orangeGuy.Fill = orangeGhost;

                ImageBrush pinkGhost = new ImageBrush();
                pinkGhost.ImageSource = new BitmapImage(new Uri(@"C:\Users\user\source\repos\Packmen\PackmanImages\pink.jpg", UriKind.Absolute));
                pinkGuy.Fill = pinkGhost;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בטעינת תמונות: {ex.Message}");
            }
        }


        private void GameLoop(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;
            if (goRight) { Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speed); }
            if (goLeft) { Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speed); }
            if (goUp) { Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speed); }
            if (goDown) { Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speed); }

            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height);

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                if ((string)x.Tag == "wall")
                {
                    if (goLeft && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        noLeft = true;
                        goLeft = false;
                    }
                    if (goRight && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        noRight = true;
                        goRight = false;
                    }
                    if (goDown && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        noDown = true;
                        goDown = false;
                    }
                    if (goUp && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        noUp = true;
                        goUp = false;
                    }
                }

                if ((string)x.Tag == "coin")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                    {
                        x.Visibility = Visibility.Hidden;
                        score++;
                    }
                }

                if ((string)x.Tag == "ghost")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox))
                    {
                        GameOver("Ghosts got you, click OK to play again");
                    }

                    if (x.Name == "orangeGuy")
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);
                    }
                    else
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + ghostSpeed);
                    }

                    currentGhostStep--;
                    if (currentGhostStep < 1)
                    {
                        currentGhostStep = ghostMoveStep;
                        ghostSpeed = -ghostSpeed;
                    }
                }
            }

            if (score == 85)
            {
                GameOver("You Win, you collected all of the coins");
            }
        }

        private void GameOver(string message)
        {
            gameTimer.Stop();
            MessageBox.Show(message, "PACKMAN GAME ");

            MessageBoxResult result = MessageBox.Show("Do you want to play again?", "Play Again", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                ResetGame();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void ResetGame()
        {
            score = 0;
            goLeft = goRight = goUp = goDown = false;
            noLeft = noRight = noUp = noDown = false;

            Canvas.SetLeft(pacman, 50);
            Canvas.SetTop(pacman, 104);

            Canvas.SetLeft(redGuy, 173);
            Canvas.SetTop(redGuy, 29);

            Canvas.SetLeft(orangeGuy, 651);
            Canvas.SetTop(orangeGuy, 104);

            Canvas.SetLeft(pinkGuy, 173);
            Canvas.SetTop(pinkGuy, 404);

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "coin")
                {
                    x.Visibility = Visibility.Visible;
                }
            }

            ghostSpeed = 10;
            currentGhostStep = ghostMoveStep;

            gameTimer.Start();
        }
    }
}
