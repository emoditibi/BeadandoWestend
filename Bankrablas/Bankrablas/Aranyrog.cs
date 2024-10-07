using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankrablas
{
    public class Aranyrog : VarosElem
    {
  
        public override ConsoleColor Hatterszin => ConsoleColor.Yellow;
        public Aranyrog(int x, int y) : base(x, y)
        {
        }

        public override string ToString()
        {
            return "A"; 
        }
    }

}
