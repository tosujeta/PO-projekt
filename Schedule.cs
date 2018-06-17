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
        private DateTime Departuretime;
        private DateTime Arrivaltime;
        private Rout flight;
        public int NumberOfTicketsBought { get; private set; }
        private List<Customer> passengers = new List<Customer>();

        public Schedule(DateTime Departuretime, DateTime Arrivaltime, Rout flightID)
        {
            this.Departuretime = Departuretime;
            this.Arrivaltime = Arrivaltime;
            this.flight = flightID;
        }
        public DateTime GetDepartureTime()
        {
            return Departuretime;
        }

        internal void UpdatePassenger(int newTicketsSize, int oldTicketSize)
        {
            NumberOfTicketsBought -= oldTicketSize;
            if (NumberOfTicketsBought + newTicketsSize <= flight.GetPlain().NumberOfTickets)
            {
                NumberOfTicketsBought += newTicketsSize;
            }
            else
            {
                NumberOfTicketsBought += oldTicketSize;
                throw new MaxPassengersReached("Osiągnięto limit miejsc. Przed dodaniem =" + NumberOfTicketsBought + ", Po dodaniu="
               + (int)(NumberOfTicketsBought + newTicketsSize) + ", Do dodania=" + newTicketsSize + "Max=" + flight.GetPlain().NumberOfTickets);
            }

            ;
        }

        public DateTime GetArrivalTime()
        {
            return Arrivaltime;
        }
        public Rout GetFlightID()
        {
            return flight;
        }
        public void SetTime(DateTime Departuretime, DateTime Arrivaltime)
        {
            this.Departuretime = Departuretime;
            this.Arrivaltime = Arrivaltime;
        }

        public void AddPassenger(Customer customer)
        {
            passengers.Add(customer);
            UpdatePassenger(customer.GetTicket().GetNumberOfTicket(), 0);
        }

        public void RemovePassenger(Customer customer)
        {
            if (passengers.Remove(customer))
            {
                NumberOfTicketsBought -= customer.GetTicket().GetNumberOfTicket();
            }
        }

        public override string ToString()
        {
            return Departuretime.ToShortDateString() + " - " + Departuretime.ToLongTimeString();
        }

        public List<Customer> GetPassengersList()
        {
            return passengers;
        }

    }
}
