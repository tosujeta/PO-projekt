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
        public string City { get; set; }
        public int X { get; set; }
        public int  Y {get; set;}

        public  Airport( int X,int Y,string City)
        {
            this.City = City;
            this.X = X;
            this.Y = Y; 
        }
        public int GetX()
        {
            return X;
        }
        public int GetY()
        {
            return Y;
        }
        public string GetCity()
        {
            return City;
        }

        public float GetDistance(Airport airport)
        {
            return (float)Math.Sqrt(Math.Pow(this.GetX() - airport.GetX(), 2)
            + Math.Pow(this.GetY() - airport.GetY(), 2));
        }

        public override string ToString()
        {
            return City + " (" + X + " : " + Y + ")";
        }
    }
}
