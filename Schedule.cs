using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{

    [Serializable]
    public enum FlightFrequency : long
    {
        EVERY_WEEK = TimeSpan.TicksPerDay * 7,
        EVERY_DAY = TimeSpan.TicksPerDay,
        EVERY_TWO_DAYS = TimeSpan.TicksPerDay * 2,
        EVERY_TREE_DAYS = TimeSpan.TicksPerDay * 3,
        EVERY_12_HOURS = TimeSpan.TicksPerHour * 12,
        ONE_FLIGHT = 0
    }

    [Serializable]
    public class Schedule
    {
        public DateTime Departuretime { get; private set; }
        public DateTime Arrivaltime { get; private set; }
        public Rout Rout { get; private set; }
        public int NumberOfTicketsBought { get; private set; }
        private List<Customer> passengers = new List<Customer>();

        public Schedule(DateTime Departuretime, DateTime Arrivaltime, Rout rout)
        {
            this.Departuretime = Departuretime;
            this.Arrivaltime = Arrivaltime;
            this.Rout = rout;
        }

        internal void UpdatePassenger(int newTicketsSize, int oldTicketSize)
        {
            NumberOfTicketsBought -= oldTicketSize;
            if (NumberOfTicketsBought + newTicketsSize <= Rout.Plane.NumberOfTickets)
            {
                NumberOfTicketsBought += newTicketsSize;
            }
            else
            {
                NumberOfTicketsBought += oldTicketSize;
                throw new MaxPassengersReached("Osiągnięto limit miejsc. Przed dodaniem=" + NumberOfTicketsBought + ", Po dodaniu="
               + (int)(NumberOfTicketsBought + newTicketsSize) + ", Do dodania=" + newTicketsSize + ", Max=" + Rout.Plane.NumberOfTickets);
            }

            ;
        }

        public void SetTime(DateTime Departuretime, DateTime Arrivaltime)
        {
            this.Departuretime = Departuretime;
            this.Arrivaltime = Arrivaltime;
        }

        public void AddPassenger(Customer customer)
        {
            if (passengers.Contains(customer)) return;
            UpdatePassenger(customer.Ticket.GetNumberOfTicket(), 0);
            passengers.Add(customer);
        }

        public void RemovePassenger(Customer customer)
        {
            if (passengers.Remove(customer))
            {
                NumberOfTicketsBought -= customer.Ticket.GetNumberOfTicket();
            }
        }

        public override string ToString()
        {
            return Departuretime.ToShortDateString() + " - " + Departuretime.ToLongTimeString();
        }

        public string GetArrivalTimeAsString()
        {
            return Arrivaltime.ToShortDateString() + " - " + Arrivaltime.ToLongTimeString();
        }

        public List<Customer> GetPassengersList()
        {
            return passengers;
        }

    }
}
