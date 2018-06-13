using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    class Ticket
    {
        private int routID;
        private int flightID;
        private int price;

        protected bool isSingle = true;

        public Ticket(int routID, int flightID, int price)
        {
            this.routID = routID;
            this.flightID = flightID;
            this.price = price;
        }

        public int GetPrice()
        {
            return price;
        }

        public int GetFlightID()
        {
            return flightID;
        }

        public int GetRoutID()
        {
            return routID;
        }
      
        public bool IsSingle()
        {
            return isSingle;
        }

        public virtual int GetNumberOfTicket()
        {
            return 1;
        }
    }
}
