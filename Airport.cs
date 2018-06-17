using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    [Serializable]
    public class Airport
    {
        public string City { get; private set; } = "Nie ustalone";
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool IsSetUp { get; private set; }

        public Airport()
        {

        }

        public void SetUp(int x, int y, string City)
        {
            if (IsSetUp) return;
            this.City = City;
            this.X = x;
            this.Y = y;
            IsSetUp = true;
        }

        public float GetDistance(Airport airport)
        {
            return (float)Math.Sqrt(Math.Pow(this.X - airport.Y, 2)
            + Math.Pow(this.X - airport.Y, 2));
        }

        public override string ToString()
        {
            String str = City + " (" + X + " : " + Y + ")";
            if (!IsSetUp) str += " (Nieuzupełniony)";
            return str;
        }
    }
}
