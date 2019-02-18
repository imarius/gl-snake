using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeV1
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            

            InitializeComponent();
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            new Settings();

            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += updateScreen;
            gameTimer.Start();

            startGame();
        }

        private void updateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver == true)
            {
                if (Input.KeyPress(Keys.Enter))
                {
                    startGame();
                }
            }
            else
            {
                if (Input.KeyPress(Keys.Right) && Settings.Direction != Directions.Left)
                {
                    Settings.Direction = Directions.Right;
                    label4.Visible = false;
                    label5.Visible = false;
                    label6.Visible = true;
                    label7.Visible = false;
                }
                else if (Input.KeyPress(Keys.Left) && Settings.Direction != Directions.Right)
                {
                    Settings.Direction = Directions.Left;
                    label4.Visible = false;
                    label5.Visible = false;
                    label6.Visible = false;
                    label7.Visible = true;
                }
                else if (Input.KeyPress(Keys.Up) && Settings.Direction != Directions.Down)
                {
                    Settings.Direction = Directions.Up;
                    label4.Visible = true;
                    label5.Visible = false;
                    label6.Visible = false;
                    label7.Visible = false;
                }
                else if (Input.KeyPress(Keys.Down) && Settings.Direction != Directions.Up)
                {
                    Settings.Direction = Directions.Down;
                    label4.Visible = false;
                    label5.Visible = true;
                    label6.Visible = false;
                    label7.Visible = false;
                }

                movePlayer();
            }

            pbCanvas.Invalidate();
        }

        private void movePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.Direction)
                    {
                        case Directions.Right:
                            Snake[i].X++;
                            break;
                        case Directions.Left:
                            Snake[i].X--;
                            break;
                        case Directions.Up:
                            Snake[i].Y--;
                            break;
                        case Directions.Down:
                            Snake[i].Y++;
                            break;
                    }

                    int maxXposition = pbCanvas.Size.Width / Settings.Width;
                    int maxYposition = pbCanvas.Size.Height / Settings.Height;

                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X > maxXposition || Snake[i].Y > maxYposition)
                    {
                        die();
                    }

                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            die();
                        }
                    }

                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        eat();
                    }
                }
                else {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void keyIsDown(object sender, KeyEventArgs e)
        {
            Input.changeState(e.KeyCode, true);
        }

        private void keyIsUp(object sender, KeyEventArgs e)
        {
            Input.changeState(e.KeyCode, false);
        }

        private void updateGraphics(object sender, PaintEventArgs e)
        {
            if (Settings.GameOver == false)
            {
                Brush snakeColor;

                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                    {
                        snakeColor = Brushes.Black;
                    }
                    else
                    {
                        snakeColor = Brushes.Green;
                    }

                    e.Graphics.FillEllipse(snakeColor, new Rectangle(Snake[i].X * Settings.Width
                        , Snake[i].Y * Settings.Height, Settings.Width, Settings.Height));

                    e.Graphics.FillEllipse(Brushes.Red, new Rectangle(food.X * Settings.Width, food.Y * Settings.Height, Settings.Width, Settings.Height));

                    
                }
            }
            else
            {
                string gameOver = "You dead, Score : " + Settings.Score;
                label3.Text = gameOver;
                label3.Visible = true;
            }
        }

        private void startGame()
        {
            label3.Visible = false;
            new Settings();
            Snake.Clear();
            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);

            label2.Text = Settings.Score.ToString();

            generateFood();
        }

        public void generateFood()
        {
            int maxX = pbCanvas.Size.Width / Settings.Width;
            int maxY = pbCanvas.Size.Height / Settings.Height;

            Random rnd = new Random();

            food = new Circle { X = rnd.Next(0, maxX), Y = rnd.Next(0, maxY) };
        }

        private void eat()
        {
            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };

            Snake.Add(body);
            Settings.Score += Settings.Points;
            label2.Text = Settings.Score.ToString();
            generateFood();
        }

        private void die()
        {
            Settings.GameOver = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
