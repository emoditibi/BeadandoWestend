using System;

namespace Bankrablas
{
    public class Bandita : VarosElem
    {
   
        public int Eletero { get; set; }  
        private const int maxEletero = 100;
        public override ConsoleColor Hatterszin => ConsoleColor.Red;
        public Bandita(int x, int y) : base(x, y)
        {
            Eletero = maxEletero;
        }
        public void Lepes(VarosElem[,] palya)
        {
            int[] directions = { -1, 0, 1 };
            int newX, newY;

            do
            {
               
                int dx = directions[rand.Next(0, 3)];
                int dy = directions[rand.Next(0, 3)];

                newX = X + dx;
                newY = Y + dy;
            } while ((newX == X && newY == Y) ||
                     newX < 0 || newX >= palya.GetLength(0) ||
                     newY < 0 || newY >= palya.GetLength(1) ||
                     palya[newX, newY] != null);  

         
            palya[X, Y] = null;  
            X = newX;
            Y = newY;
            palya[X, Y] = this;
        }

        public override string ToString()
        {
            return "B";
        }
    }
}

