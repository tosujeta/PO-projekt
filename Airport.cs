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
            int a = this.X;
            int b = this.Y;
            int c = airport.X;
            int d = airport.Y;
            int e = a - c;
            int f = c - d;
            float x = (float) Math.Pow(airport.X - this.X, 2);
            float y = (float) Math.Pow(airport.Y - this.Y, 2);
            return (float)Math.Sqrt(x + y);
        }

        public override string ToString()
        {
            String str = City + " (" + X + " : " + Y + ")";
            if (!IsSetUp) str += " (Nieuzupełniony)";
            return str;
        }
    }
}
