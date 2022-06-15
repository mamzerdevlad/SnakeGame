using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakaGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SNAKE Snake;
        Random rand;
        Graphics graphics;
        int X, Y;
        int [,] IsFree;
        int [] AppleCoordinates;
        readonly int CellSize = 39;
        bool IsApple;
        bool IsStep = false;
        private int GetScore(double value)
        {
            value = Math.Abs(value);
            value = 7 * value + 0.7 * Math.Sin(3 * value);
            return (int)value;
        }
        private int GetCellIndex(int x, int y)
        {
            int INDEX = (Y / CellSize * y + x + 1) * IsFree[y, x];
            return INDEX;
        }
        private void ChangeAngle(int a)
        {
            if (IsStep)
            {
                if (!(Math.Abs(Snake.HEAD.Angle - a) == 2 || Snake.HEAD.Angle == a))
                {
                    Snake.HEAD.Angle = a;
                }
            }
            IsStep = false;
        }
        private void InitializeIsFreeArray(int stage)
        {
            if (stage == 0)
            {
                for (int i = 0; i < Y / CellSize; i++)
                {
                    for (int o = 0; o < X / CellSize; o++)
                    {
                        IsFree[i, o] = 1;
                    }
                }
            }
            else
            {
                IsFree[Snake.HEAD.Y, Snake.HEAD.X] = 0;
                IsFree[Snake.TAIL.Y, Snake.TAIL.X] = 0;
                for (int i = 0; i < Snake.length; i++)
                {
                    IsFree[Snake.BODY[i].Y, Snake.BODY[i].X] = 0;
                }
            }
        }
        private void GenerateApple(bool IsAppleNotExist)
        {
            if(IsAppleNotExist)
            {
                for (; ; )
                {
                    AppleCoordinates[0] = rand.Next(X / CellSize);
                    AppleCoordinates[1] = rand.Next(Y / CellSize);
                    if (GetCellIndex(AppleCoordinates[0], AppleCoordinates[1]) > 0)
                    {
                        IsFree[AppleCoordinates[1], AppleCoordinates[0]] = 0;
                        Snake.SetApplePosition(AppleCoordinates);
                        Snake.Apple.IsEaten = false;
                        IsApple = true;
                        break;
                    }
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Snake.Step();
            if (!Snake.IsAlive)
            {
                button1.Text += " (Restart)";
                button1_Click(sender, e);
                return;
            }

            InitializeIsFreeArray(1);
            GenerateApple(!IsApple || Snake.Apple.IsEaten);

            graphics.FillRectangle(Brushes.YellowGreen, 0, 0, CellSize * (X / CellSize), CellSize * (Y / CellSize));
            graphics.DrawRectangle(Pens.Red, 0, 0, CellSize * (X / CellSize), CellSize * (Y / CellSize));

            graphics.FillRectangle(Brushes.Green, Snake.HEAD.X * CellSize + 1, Snake.HEAD.Y * CellSize + 1, CellSize - 2, CellSize - 2);
            graphics.DrawRectangle(Pens.Black, Snake.HEAD.X * CellSize + 1, Snake.HEAD.Y * CellSize + 1, CellSize - 2, CellSize - 2);
            for (int i = 0; i < Snake.length; i++)
            {
                graphics.FillRectangle(Brushes.Green, Snake.BODY[i].X * CellSize + 1, Snake.BODY[i].Y * CellSize + 1, CellSize - 2, CellSize - 2);
                graphics.DrawRectangle(Pens.Black, Snake.BODY[i].X * CellSize + 1, Snake.BODY[i].Y * CellSize + 1, CellSize - 2, CellSize - 2);
            }
            graphics.FillRectangle(Brushes.Green, Snake.TAIL.X * CellSize + 1, Snake.TAIL.Y * CellSize + 1, CellSize - 2, CellSize - 2);
            graphics.DrawRectangle(Pens.Black, Snake.TAIL.X * CellSize + 1, Snake.TAIL.Y * CellSize + 1, CellSize - 2, CellSize - 2);

            graphics.FillEllipse(Brushes.Red, AppleCoordinates[0] * CellSize + 8, AppleCoordinates[1] * CellSize + 8, CellSize - 16, CellSize - 16);
            graphics.DrawEllipse(Pens.Black, AppleCoordinates[0] * CellSize + 8, AppleCoordinates[1] * CellSize + 8, CellSize - 16, CellSize - 16);

            pictureBoxField.Refresh();
            button1.Text = "Score: " + Convert.ToString(GetScore(Snake.length - 1));
            IsStep = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = !true;
                button1.Enabled = true;
            }
            else
            {
                timer1.Enabled = !false;
                button1.Enabled = false;

                X = pictureBoxField.Width;
                Y = pictureBoxField.Height;

                IsApple = false;
                AppleCoordinates = new int[2];

                pictureBoxField.Image = new Bitmap(X, Y);
                rand = new Random();
                Snake = new SNAKE(X / CellSize, Y / CellSize);
                graphics = Graphics.FromImage(pictureBoxField.Image);

                IsFree = new int[Y / CellSize, X / CellSize];
                InitializeIsFreeArray(0);
                GenerateApple(!IsApple || Snake.Apple.IsEaten);
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                ChangeAngle(0);
            }
            else if (e.KeyCode == Keys.Up)
            {
                ChangeAngle(1);
            }
            else if (e.KeyCode == Keys.Left)
            {
                ChangeAngle(2);
            }
            else if (e.KeyCode == Keys.Down)
            {
                ChangeAngle(3);
            }
            else return;
            //if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            //{
            //    ChangeAngle(0);
            //}
            //else if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
            //{
            //    ChangeAngle(1);
            //}
            //else if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            //{
            //    ChangeAngle(2);
            //}
            //else if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            //{
            //    ChangeAngle(3);
            //}
            //else return;
        }
    }
}