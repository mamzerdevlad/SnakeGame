using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakaGame
{
    enum SnakePartName
    {
        Head = 1,
        Body,
        Tail,
        Apple
    }
    class SnakePart
    {
        public bool IsEaten;
        public int X { get; set; }
        public int Y { get; set; }
        public int Angle { get; set; }
        public SnakePart(SnakePartName Part, int X, int Y)
        {
            //this.Part = Part;
            this.X = X;
            this.Y = Y;
            if (Part == SnakePartName.Head)
            {
                Angle = 1;
            }
            if (Part == SnakePartName.Apple)
            {
                IsEaten = false;
            }
        }
    }
    class SNAKE
    {
        public bool IsAlive = true;
        public SnakePart HEAD;
        public SnakePart[] BODY;
        public SnakePart TAIL;
        public SnakePart Apple;
        public int length = 1;
        int MAX_X, MAX_Y;
        public SNAKE(int MAX_X, int MAX_Y)
        {
            this.MAX_X = MAX_X;
            this.MAX_Y = MAX_Y;
            BODY = new SnakePart[length];
            HEAD = new SnakePart(SnakePartName.Head, MAX_X / 2, MAX_Y / 2);
            BODY[0] = new SnakePart(SnakePartName.Body, MAX_X / 2, (MAX_Y / 2) + 1);
            TAIL = new SnakePart(SnakePartName.Tail, MAX_X / 2, (MAX_Y / 2) + 2);
            Apple = new SnakePart(SnakePartName.Apple, 0, 0);
        }
        public void Step()
        {
            for (int _i = 0; _i < length; _i++)
            {
                if (HEAD.Angle == GetPartAngle(BODY[_i], HEAD))
                {
                    IsAlive = false;
                    return;
                }
                if (_i == 0 && HEAD.Angle == GetPartAngle(TAIL, HEAD))
                {
                    IsAlive = false;
                    return;
                }
            }
            if (HEAD.Angle == GetPartAngle(Apple, HEAD))
            {
                Apple.IsEaten = true;
                length++;
                SnakePart[] _BODY = new SnakePart[length];
                _BODY[0] = new SnakePart(SnakePartName.Body, HEAD.X, HEAD.Y);
                for (int _i = 1; _i < length; _i++)
                {
                    _BODY[_i] = new SnakePart(SnakePartName.Body, BODY[_i - 1].X, BODY[_i - 1].Y);
                }
                BODY = _BODY;
                MakeStep(HEAD, HEAD.Angle);
            }
            else
            {
                MakeStep(TAIL, GetPartAngle(BODY[length - 1], TAIL));
                for (int _i = length - 1; _i >= 0; _i--)
                {
                    if (_i == 0)
                    {
                        MakeStep(BODY[_i], GetPartAngle(HEAD, BODY[_i]));
                    }
                    else
                    {
                        MakeStep(BODY[_i], GetPartAngle(BODY[_i - 1], BODY[_i]));
                    }
                }
                MakeStep(HEAD, HEAD.Angle);
            }
        }
        void MakeStep(SnakePart Part, int angle)
        {
            switch (angle)
            {
                case 0:
                    Part.X = (Part.X + MAX_X + 1) % MAX_X;
                    break;
                case 1:
                    Part.Y = (Part.Y + MAX_Y - 1) % MAX_Y;
                    break;
                case 2:
                    Part.X = (Part.X + MAX_X - 1) % MAX_X;
                    break;
                case 3:
                    Part.Y = (Part.Y + MAX_Y + 1) % MAX_Y;
                    break;
            }
        }
        int GetPartAngle(SnakePart X, SnakePart Y)         //Y - центральный объект, X - целевой объект
        {
            int num;
            if (X.X == (Y.X + MAX_X + 1) % MAX_X && X.Y == Y.Y) num = 0;     //Справа
            else if (X.X == Y.X && X.Y == (Y.Y + MAX_Y - 1) % MAX_Y) num = 1;//Сверху
            else if (X.X == (Y.X + MAX_X - 1) % MAX_X && X.Y == Y.Y) num = 2;//Слева
            else if (X.X == Y.X && X.Y == (Y.Y + MAX_Y + 1) % MAX_Y) num = 3;//Снизу
            else num = -1;                                                   //Не найдено
            return num;
        }
        public void SetApplePosition(int[] Coordinates)
        {
            Apple.X = Coordinates[0];
            Apple.Y = Coordinates[1];
        }
    }
}