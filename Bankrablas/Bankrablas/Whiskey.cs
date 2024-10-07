using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankrablas
{
    
        public class Whiskey : VarosElem
        {
            public const int HealAmount = 50;

        public override ConsoleColor Hatterszin => ConsoleColor.DarkYellow;
        public Whiskey(int x, int y) : base(x, y)
            {
            }
            public override string ToString()
            {
                return "W"; 
            }
        }
}
