using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    [Serializable]
    public class Customer
    {
        public Ticket Ticket { get; private set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public Schedule FlightSchedule { get; private set; }

        public Customer(String name, String surname)
        {
            SetTicket(new Ticket(0));
            this.Name = name;
            this.Surname = surname;
        }

        public void SetFlightSchedule(Schedule schedule)
        {
            if (schedule != null) schedule.AddPassenger(this);
            if (FlightSchedule != null)
            {
                FlightSchedule.RemovePassenger(this);
            }
            FlightSchedule = schedule;
        }

        public void SetTicket(Ticket ticket)
        {
            if (FlightSchedule != null)
            {
                FlightSchedule.UpdatePassenger(ticket.GetNumberOfTicket(), Ticket.GetNumberOfTicket());
            }
            this.Ticket = ticket;
        }

        public override string ToString()
        {
            return Name + " " + Surname;
        }

        public Rout GetFlight()
        {
            if (FlightSchedule != null) return FlightSchedule.Rout;
            return null;
        }
    }
}
