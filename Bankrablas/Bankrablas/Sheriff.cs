using System;

namespace Bankrablas
{
    public class Sheriff : VarosElem
    {
        public int Eletero { get; set; }
        private const int maxEletero = 100;
        public int AranyRogokSzama { get; set; }
        private VarosElem[,] palya;
        public Bandita currentBandita;
        public override ConsoleColor Hatterszin => ConsoleColor.Blue;

        public Sheriff(int x, int y, VarosElem[,] palya) : base(x, y)
        {
            Eletero = maxEletero;
            this.palya = palya;
            this.AranyRogokSzama = 0;
            this.X = x;
            this.Y = y;
        }

        public void Parbaj(Bandita bandita)
        {
            Random rand = new Random();
            int seriffSebzes = rand.Next(20, 36);
            bandita.Eletero -= seriffSebzes;
            Console.WriteLine($"Seriff megsebezte a banditát {seriffSebzes} életerővel.");
            if (bandita.Eletero <= 0)
            {
                Console.WriteLine("A bandita meghalt!");
                palya[bandita.X, bandita.Y] = null;
                return;
            }
            int banditaSebzes = rand.Next(4, 16);
            Eletero -= banditaSebzes;
            Console.WriteLine($"A bandita megsebezte a seriffet {banditaSebzes} életerővel.");
            if (Eletero <= 0)
            {
                Console.WriteLine("A seriff meghalt! A játék véget ért.");
                Environment.Exit(0);
            }
        }


   

        public override string ToString()
        {
            return "S";
        }
    }
}
