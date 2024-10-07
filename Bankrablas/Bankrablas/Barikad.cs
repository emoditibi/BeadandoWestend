using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankrablas
{
    public class Barikad : VarosElem
    {
        public override ConsoleColor Hatterszin => ConsoleColor.DarkGray;
        public Barikad(int x, int y) : base(x, y)
        {
        }
        public override string ToString()
        {
            return "X";  
        }
    }

}
