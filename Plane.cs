using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    [Serializable]
    public class Plane
    {

        public String Name { get; set; }
        public int NumberOfTickets { get; set; }
        public int Range { get; set; }
        public int Speed { get; set; }
        public bool IsFree { get; private set; } = true;

        public Plane(String name,int Numberoftickets, int Range, int Speed)
        {
            this.Name = name;
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

        public override string ToString()
        {
            return Name + " (" + NumberOfTickets + ")";
        }
    }
}
