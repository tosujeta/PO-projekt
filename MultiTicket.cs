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

        public MultiTicket(int price, int numberOfSeats) : base( price)
        {
            this.NumberOfSeats = numberOfSeats;
            IsSingle = false;
        }

        public override int GetNumberOfTicket()
        {
            Console.WriteLine();
            return NumberOfSeats;
        }

       
    }
}
