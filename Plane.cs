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

        public String Name { get; set; } = "Bez nazwy";
        public int NumberOfTickets { get; private set; }
        public int Range { get; private set; }
        public int Speed { get; private set; }
        public bool IsFree { get; private set; } = true;
        public bool IsSetUp { get; private set; } = false;

        public Plane()
        {

        }

        public void SetUp(String name, int NumberOfTickets, int Range, int Speed)
        {
            if (IsSetUp) return;
            this.Name = name;
            this.NumberOfTickets = NumberOfTickets;
            this.Range = Range;
            this.Speed = Speed;
            IsSetUp = true;
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
            String str = Name + " (" + NumberOfTickets + ")";
            if (!IsSetUp) str += " (Nieuzupełniony)";
            return str;
        }
    }
}
