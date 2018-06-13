using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{

    enum FlightFrequency : long
    {
        EVERY_WEEK = TimeSpan.TicksPerDay*7,
        EVERY_DAY = TimeSpan.TicksPerDay,
        EVERY_TWO_DAYS = TimeSpan.TicksPerDay*2,
        EVERY_TREE_DAYS = TimeSpan.TicksPerDay * 3,
        EVERY_12_HOURS = TimeSpan.TicksPerHour*12
    }
    class Schedule
    {
        private DateTime Departuretime;
        private DateTime Arrivaltime;
        private int flightID;
        public int NumberOfTicketsBought { get; private set; }
        private List<Customer> passengers;

        public Schedule(DateTime Departuretime, DateTime Arrivaltime, int flightID)
        {
            this.Departuretime = Departuretime;
            this.Arrivaltime = Arrivaltime;
            this.flightID = flightID;
        }
        public DateTime GetDepartureTime()
        {
            return Departuretime;
        }
        public DateTime GetArrivalTime()
        {
            return Arrivaltime;
        }
        public int GetFlightID()
        {
            return flightID;
        }
        public void SetTime(DateTime Departuretime, DateTime Arrivaltime)
        {
            this.Departuretime = Departuretime;
            this.Arrivaltime = Arrivaltime;
        }

        public void AddPassenger(Customer customer)
        {
            passengers.Add(customer);
            NumberOfTicketsBought += customer.GetTicket().GetNumberOfTicket();
        }

        public void RemovePassenger(Customer customer)
        {
            if(passengers.Remove(customer))
            {
                NumberOfTicketsBought -= customer.GetTicket().GetNumberOfTicket();
            }
        }

    }
}
