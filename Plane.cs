using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    class Plane
    {
        private int Numberoftickets;
        private int Range;
        private int Speed;

        public Plane(int Numberoftickets, int Range,  int Speed)
        {
            this.Numberoftickets = Numberoftickets;
            this.Range = Range;
            this.Speed = Speed;
        }
        public int GetNumberoftickets()
        {
            return Numberoftickets;
        }
        public int GetRange()
        {
            return Range;
        }
        public GetSpeed()
        {
            return Speed;
        }
    }
}
