using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankrablas
{
    public abstract class VarosElem
    {

       public static  Random rand = new Random();
        public int X { get; set; }
         public int Y { get; set; }
        public abstract ConsoleColor Hatterszin { get; }

        public VarosElem(int x, int y)
            {
                X = x;
                Y = y;
            }

            public abstract override string ToString();
        

     
    }
}
