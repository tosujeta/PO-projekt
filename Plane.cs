using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    class Plane
    {
        public int NumberOfTickets { get; }
        private int Range;
        private int Speed; //km na godzine
        public bool IsFree { get; private set; } = true;

        public Plane(int Numberoftickets, int Range, int Speed)
        {
            this.NumberOfTickets = Numberoftickets;
            this.Range = Range;
            this.Speed = Speed;
        }
        public int GetRange()
        {
            return Range;
        }
        public int GetSpeed()
        {
            return Speed;
        }

        public void Assign()
        {
            IsFree = false;
        }

        public void Unassign()
        {
            IsFree = true;
        }
    }
}
