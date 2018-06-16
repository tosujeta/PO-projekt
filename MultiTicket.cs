using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    [Serializable]
    public class MultiTicket : Ticket
    {
        public int NumberOfSeats { get; }

        public MultiTicket(int routID, int flightID, int price, int numberOfSeats) : base(routID, flightID, price)
        {
            this.NumberOfSeats = numberOfSeats;
            IsSingle = false;
        }

        public override int GetNumberOfTicket()
        {
            return NumberOfSeats;
        }
    }
}
