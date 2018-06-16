using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    [Serializable]
    public class Ticket
    {
        private int routID;
        private int flightID;
        public int Price { get; set; }

        public bool IsSingle { get; set; } = true;

        public Ticket(int routID, int flightID, int price)
        {
            this.routID = routID;
            this.flightID = flightID;
            this.Price = price;
        }

        public int GetPrice()
        {
            return Price;
        }

        public int GetFlightID()
        {
            return flightID;
        }

        public int GetRoutID()
        {
            return routID;
        }

        public virtual int GetNumberOfTicket()
        {
            return 1;
        }

        public Ticket Change(int priceN, int numberOfSeatsN)
        {
            if (numberOfSeatsN == 0) return new Ticket(routID, flightID, priceN);
            else return new MultiTicket(routID, flightID, priceN, numberOfSeatsN);
        }
    }
}
