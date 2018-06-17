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
        public int Price { get; set; }

        public bool IsSingle { get; set; } = true;

        public Ticket(int price)
        {
            this.Price = price;
        }

        public int GetPrice()
        {
            return Price;
        }

        public virtual int GetNumberOfTicket()
        {
            return 1;
        }

        public Ticket Change(int priceN, int numberOfSeatsN)
        {
            if (numberOfSeatsN == 0) return new Ticket(priceN);
            else return new MultiTicket(priceN, numberOfSeatsN);
        }
    }
}
