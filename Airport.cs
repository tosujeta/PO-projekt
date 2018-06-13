using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    class Airport
    {
        private string City;
        private int X, Y;

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
    }
}
