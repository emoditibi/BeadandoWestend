using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Bankrablas
{
    public class Varos
    {
        public int AranyRogokSzama;
        private VarosElem[,] palya;
        private Sheriff sheriff;
        private bool[,] felfedezettTerulet;
        private Timer lepesTimer;
        Random rand = new Random();

        public Varos()
        {
            palya = new VarosElem[25, 25];
            felfedezettTerulet = new bool[25, 25];
            InitializePalyat();

            lepesTimer = new Timer(1000);
            lepesTimer.Elapsed += OnTimedEvent;
            lepesTimer.AutoReset = true;
            lepesTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Lepes();
            KirajzolPalya();
        }

        public void SpawnNewWhiskey()
        {
            int x, y;
            do
            {
                x = rand.Next(0, 25);
                y = rand.Next(0, 25);
            } while (palya[x, y] != null);

            Whiskey whiskey = new Whiskey(x, y);
            palya[x, y] = whiskey;
            Console.WriteLine("Új whiskey jelent meg a pályán!");
        }

        private void InitializePalyat()
        {
            sheriff = new Sheriff(rand.Next(0, 25), rand.Next(0, 25), palya);
            palya[sheriff.X, sheriff.Y] = sheriff;

            for (int i = 0; i < 4; i++)
            {
                int x, y;
                do
                {
                    x = rand.Next(0, 25);
                    y = rand.Next(0, 25);
                } while (palya[x, y] != null);

                Bandita bandita = new Bandita(x, y);
                palya[x, y] = bandita;
            }

            for (int i = 0; i < 5; i++)
            {
                int x, y;
                do
                {
                    x = rand.Next(0, 25);
                    y = rand.Next(0, 25);
                } while (palya[x, y] != null);

                Aranyrog aranyrog = new Aranyrog(x, y);
                palya[x, y] = aranyrog;
            }

            for (int i = 0; i < 3; i++)
            {
                int x, y;
                do
                {
                    x = rand.Next(0, 25);
                    y = rand.Next(0, 25);
                } while (palya[x, y] != null);

                Whiskey whiskey = new Whiskey(x, y);
                palya[x, y] = whiskey;
            }

            for (int i = 0; i < 40; i++)
            {
                int x, y;
                do
                {
                    x = rand.Next(0, 25);
                    y = rand.Next(0, 25);
                } while (palya[x, y] != null);

                Barikad barikad = new Barikad(x, y);
                palya[x, y] = barikad;
            }
        }

        public void Initialize()
        {
            felfedezettTerulet = new bool[palya.GetLength(0), palya.GetLength(1)];
        }

        private List<Tuple<int, int>> GetNeighbors(int x, int y)
        {
            List<Tuple<int, int>> neighbors = new List<Tuple<int, int>>();
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int ujX = x + dx;
                    int ujY = y + dy;

                    if (ujX >= 0 && ujX < palya.GetLength(0) && ujY >= 0 && ujY < palya.GetLength(1))
                    {
                        neighbors.Add(new Tuple<int, int>(ujX, ujY));
                    }
                }
            }
            return neighbors;
        }

        private Tuple<int, int> FindClosestUnexploredArea()
        {
            Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
            bool[,] visited = new bool[palya.GetLength(0), palya.GetLength(1)];

            queue.Enqueue(new Tuple<int, int>(sheriff.X, sheriff.Y));
            visited[sheriff.X, sheriff.Y] = true;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                int currentX = current.Item1;
                int currentY = current.Item2;

                if (!felfedezettTerulet[currentX, currentY])
                {
                    return current;
                }

                foreach (var neighbor in GetNeighbors(currentX, currentY))
                {
                    int neighborX = neighbor.Item1;
                    int neighborY = neighbor.Item2;

                    if (!visited[neighborX, neighborY] && !(palya[neighborX, neighborY] is Barikad))
                    {
                        queue.Enqueue(neighbor);
                        visited[neighborX, neighborY] = true;
                    }
                }
            }

            return null;
        }

        private void FedezFel3x3(int x, int y)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int ujX = x + dx;
                    int ujY = y + dy;

                    if (ujX >= 0 && ujX < palya.GetLength(0) && ujY >= 0 && ujY < palya.GetLength(1))
                    {
                        felfedezettTerulet[ujX, ujY] = true;
                    }
                }
            }
        }

        private bool CheckIfAllExplored()
        {
            for (int i = 0; i < felfedezettTerulet.GetLength(0); i++)
            {
                for (int j = 0; j < felfedezettTerulet.GetLength(1); j++)
                {
                    if (!felfedezettTerulet[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void Lepes()
        {
            FedezFel3x3(sheriff.X, sheriff.Y);
            bool talaltBandita = false;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int ujX = sheriff.X + dx;
                    int ujY = sheriff.Y + dy;

                    if (ujX >= 0 && ujX < palya.GetLength(0) && ujY >= 0 && ujY < palya.GetLength(1))
                    {
                        VarosElem elem = palya[ujX, ujY];

                        if (elem is Bandita bandita)
                        {
                            Console.WriteLine("A seriff meglátott egy banditát! Megáll és harcol.");
                            talaltBandita = true;
                            sheriff.Parbaj(bandita);

                            if (bandita.Eletero <= 0)
                            {
                                palya[ujX, ujY] = null;
                                Console.WriteLine("A seriff legyőzte a banditát! Folytathatja az útját.");
                                talaltBandita = false;
                            }
                            break;
                        }
                    }
                }

                if (talaltBandita) return;
            }

            MoveBandits();

            if (!talaltBandita)
            {
                var closestUnexplored = FindClosestUnexploredArea();
                if (closestUnexplored != null)
                {
                    int targetX = closestUnexplored.Item1;
                    int targetY = closestUnexplored.Item2;
                    if (!(palya[targetX, targetY] is Barikad))
                    {
                        palya[sheriff.X, sheriff.Y] = null;
                        sheriff.X = targetX;
                        sheriff.Y = targetY;
                        palya[sheriff.X, sheriff.Y] = sheriff;
                    }
                }
            }

            CheckSurroundings(sheriff.X, sheriff.Y);

            if (sheriff.AranyRogokSzama >= 5)
            {
                Console.WriteLine("Gratulálok! A seriff összegyűjtötte az összes aranyrögöt és megnyerte a játékot!");
                VegeJateknak();
                return;
            }

            if (CheckIfAllExplored())
            {
                Console.WriteLine("Minden terület felfedezve! A játék véget ért.");
                VegeJateknak();
                return;
            }
        }

        private void VegeJateknak()
        {
            lepesTimer.Stop();
            lepesTimer.Dispose();
            Environment.Exit(0);
        }

        private void MoveBandits()
        {
            for (int i = 0; i < palya.GetLength(0); i++)
            {
                for (int j = 0; j < palya.GetLength(1); j++)
                {
                    if (palya[i, j] is Bandita bandita)
                    {
                        List<Tuple<int, int>> neighbors = GetNeighbors(bandita.X, bandita.Y);
                        var randomNeighbor = neighbors[rand.Next(neighbors.Count)];

                        int targetX = randomNeighbor.Item1;
                        int targetY = randomNeighbor.Item2;

                        if (palya[targetX, targetY] == null)
                        {
                            palya[bandita.X, bandita.Y] = null;
                            bandita.X = targetX;
                            bandita.Y = targetY;
                            palya[targetX, targetY] = bandita;
                        }
                    }
                }
            }
        }


        public void KirajzolPalya()
        {
            Console.Clear();

            for (int i = 0; i < palya.GetLength(0); i++)
            {
                for (int j = 0; j < palya.GetLength(1); j++)
                {
                     
                    if (palya[i, j] != null)
                    {
                   
                        Console.BackgroundColor = palya[i, j].Hatterszin; 
                        Console.ForegroundColor = ConsoleColor.White; 
                        Console.Write(palya[i, j].ToString() + " ");
                    }
                    else
                    {
                        if (i >= 0 )
                        {
                    
                            if (felfedezettTerulet[i, j])
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGreen; 
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Green; 
                            }
                            Console.Write("  ");
                        }
                    }
                }
                Console.WriteLine();
            }

            Console.ResetColor(); 
            Console.WriteLine($"Sheriff életereje: {sheriff.Eletero}");
        }
        public void CheckSurroundings(int x, int y)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int ujX = x + dx;
                    int ujY = y + dy;

                    if (ujX >= 0 && ujX < palya.GetLength(0) && ujY >= 0 && ujY < palya.GetLength(1))
                    {
                        VarosElem elem = palya[ujX, ujY];
                        if (elem is Aranyrog aranyrog)
                        {
                            Console.WriteLine("A seriff aranyrögöt talált!");
                            sheriff.AranyRogokSzama++;
                            palya[ujX, ujY] = null;
                        }
                        else if (elem is Whiskey whiskey)
                        {
                            Console.WriteLine("A seriff whiskey-t talált!");
                            if (sheriff.Eletero+50>100)
                            {
                                sheriff.Eletero = 100;
                            }
                            else
                            {
                                sheriff.Eletero += 50;
                            }
                            palya[ujX, ujY] = null;
                            SpawnNewWhiskey();
                        }
                    }
                }
            }
        }

    }

}
