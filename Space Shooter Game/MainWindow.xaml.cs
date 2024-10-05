using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace space_shooter_game
{
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        bool moveLeft, moveRight;
        List<Rectangle> itemsToRemove = new List<Rectangle>();
        Random rand = new Random();
        int enemySpriteCounter;
        int enemyCounter;
        int playerSpeed = 5; // מהירות השחקן
        int limit;
        int score;
        int damage;
        Rect playerHitBox;

        public MainWindow()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            score = 0;
            damage = 0;
            enemyCounter = 100;
            limit = 50;
            moveLeft = moveRight = false;
            itemsToRemove.Clear();

            MyCanvas.Children.Clear(); // איפוס התצוגה
            MyCanvas.Children.Add(player);
            MyCanvas.Children.Add(scoreText);
            MyCanvas.Children.Add(damageText);
            Button btnBackToMenu = new Button
            {
                Content = "GameCenter",
                Width = 120,
                Height = 30,
                Background = Brushes.Purple,
                Foreground = Brushes.White,
                FontSize = 14,
                IsDefault = false, // This line is optional; it can be set in XAML too
                Focusable = false // Prevent the button from being focused by keyboard input
            };

            // Position the button near the score
            Canvas.SetLeft(btnBackToMenu, 150); // Adjust the X position as needed
            Canvas.SetTop(btnBackToMenu, 10); // Same Y position as score text

            // Add the button to the Canvas
            MyCanvas.Children.Add(btnBackToMenu);

            // Hook up the event handler for the button
            btnBackToMenu.Click += btnBackToMenu_Click;



            Canvas.SetTop(player, 495);
            Canvas.SetLeft(player, 222);

            // עצור את ה-DispatcherTimer אם הוא פועל
            gameTimer.Stop();

            // הגדרת ה-DispatcherTimer מחדש
            gameTimer.Interval = TimeSpan.FromMilliseconds(30); // הגדלת האינטרוול ל-30ms
            gameTimer.Tick += gameEngine;
            gameTimer.Start();

            MyCanvas.Focus();
            ImageBrush bg = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("C:/Users/user/source/repos/Space Shooter Game/ImagesSpaceShooterGame/purple.png")),
                TileMode = TileMode.Tile,
                Viewport = new Rect(0, 0, 0.15, 0.15),
                ViewportUnits = BrushMappingMode.RelativeToBoundingBox
            };
            MyCanvas.Background = bg;

            ImageBrush playerImage = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("C:/Users/user/source/repos/Space Shooter Game/ImagesSpaceShooterGame/player.png"))
            };
            player.Fill = playerImage;
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left) moveLeft = true;
            if (e.Key == Key.Right) moveRight = true;
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left) moveLeft = false;
            if (e.Key == Key.Right) moveRight = false;

            if (e.Key == Key.Space)
            {
                Rectangle newBullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red
                };
                Canvas.SetTop(newBullet, Canvas.GetTop(player) - newBullet.Height);
                Canvas.SetLeft(newBullet, Canvas.GetLeft(player) + player.Width / 2);
                MyCanvas.Children.Add(newBullet);
            }
        }

        private void makeEnemies()
        {
            ImageBrush enemySprite = new ImageBrush();
            enemySpriteCounter = rand.Next(1, 6);
            string imagePath = "C:/Users/user/source/repos/Space Shooter Game/ImagesSpaceShooterGame/";

            switch (enemySpriteCounter)
            {
                case 1:
                    enemySprite.ImageSource = new BitmapImage(new Uri(imagePath + "1.png"));
                    break;
                case 2:
                    enemySprite.ImageSource = new BitmapImage(new Uri(imagePath + "2.png"));
                    break;
                case 3:
                    enemySprite.ImageSource = new BitmapImage(new Uri(imagePath + "3.png"));
                    break;
                case 4:
                    enemySprite.ImageSource = new BitmapImage(new Uri(imagePath + "4.png"));
                    break;
                case 5:
                    enemySprite.ImageSource = new BitmapImage(new Uri(imagePath + "5.png"));
                    break;
                default:
                    enemySprite.ImageSource = new BitmapImage(new Uri(imagePath + "1.png"));
                    break;
            }

            Rectangle newEnemy = new Rectangle
            {
                Tag = "enemy",
                Height = 50,
                Width = 56,
                Fill = enemySprite
            };
            Canvas.SetTop(newEnemy, -100);
            Canvas.SetLeft(newEnemy, rand.Next(30, 430));
            MyCanvas.Children.Add(newEnemy);
        }


        private void gameEngine(object sender, EventArgs e)
        {
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            enemyCounter--;
            scoreText.Content = "Score: " + score;
            damageText.Content = "Damage: " + damage;

            if (enemyCounter < 0)
            {
                makeEnemies();
                enemyCounter = limit;
            }

            if (moveLeft && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }

            if (moveRight && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 7); // הפחתת מהירות היריות
                    Rect bullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (Canvas.GetTop(x) < 10)
                    {
                        itemsToRemove.Add(x);
                    }

                    foreach (var y in MyCanvas.Children.OfType<Rectangle>())
                    {
                        if ((string)y.Tag == "enemy")
                        {
                            Rect enemy = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (bullet.IntersectsWith(enemy))
                            {
                                itemsToRemove.Add(x);
                                itemsToRemove.Add(y);
                                score++;
                            }
                        }
                    }
                }

                if ((string)x.Tag == "enemy")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 4); // הפחתת מהירות האויבים
                    Rect enemy = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (Canvas.GetTop(x) + 150 > 700)
                    {
                        itemsToRemove.Add(x);
                        damage += 10;
                    }

                    if (playerHitBox.IntersectsWith(enemy))
                    {
                        damage += 5;
                        itemsToRemove.Add(x);
                    }
                }
            }

            if (score > 5)
            {
                limit = 20;
            }

            if (damage > 99)
            {
                gameTimer.Stop();
                damageText.Content = "Damaged: 100";
                damageText.Foreground = Brushes.Red;
                MessageBox.Show("Well Done Star Captain!" + Environment.NewLine + "You have destroyed " + score + " Alien ships");
                ResetGame();
            }

            foreach (Rectangle i in itemsToRemove)
            {
                MyCanvas.Children.Remove(i);
            }
            itemsToRemove.Clear();
        }

        private void btnBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Stop();
            this.Close();
        }

        private void ResetGame()
        {
            gameTimer.Stop(); // עצור את ה-DispatcherTimer
            StartGame(); // קריאה לפונקציה שמתחילה מחדש את המשחק
        }
    }
}
