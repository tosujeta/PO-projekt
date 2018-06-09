using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    class MultiTicket : Ticket
    {
        private int numberOfSeats;

        MultiTicket(int routID, int flightID, int price, int numberOfSeats) : base(routID, flightID, price)
        {
            this.numberOfSeats = numberOfSeats;
            isSingle = false;
        }
    }
}
