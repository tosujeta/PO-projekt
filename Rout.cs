using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    class Rout
    {
        private Airport fromAirport, toAirport;
        private Plane plane;
        private int routID;
        private bool isOneRout = true;
        private FlightFrequency flightFrequency;
        private DateTime firstDeparturTime;
        private List<Schedule> schedules = new List<Schedule>();

        public Rout(DateTime firstDeparturTime, Airport fromAirport, Airport toAirport, Plane plane, int routID)
        {
            this.firstDeparturTime = firstDeparturTime;
            this.fromAirport = fromAirport;
            this.toAirport = toAirport;
            this.plane = plane;
            this.routID = routID;

            schedules.Add(new Schedule(firstDeparturTime, CalculateArriveDateTime(), 0));
        }

        public float GetDistacne()
        {
            return (float)Math.Sqrt(Math.Pow(fromAirport.GetX() - toAirport.GetX(), 2)
                + Math.Pow(fromAirport.GetY() - toAirport.GetY(), 2));
        }

        public int GetRoutID()
        {
            return routID;
        }

        public void SetFlightFrequency(FlightFrequency flightFrequency)
        {
            this.flightFrequency = flightFrequency;
            isOneRout = false;
        }

        public FlightFrequency GetFlightFrequency()
        {
            return flightFrequency;
        }

        public Plane GetPlain()
        {
            return plane;
        }

        public void SetSchedule(Schedule schedule)
        {

        }

        public void SetPlain(Plane plane)
        {
            this.plane = plane;
        }

        public bool IsOneRout()
        {
            return isOneRout;
        }

        public bool WillFlight(DateTime date)
        {
            if (IsOneRout())
            {
                if (date == firstDeparturTime) return true;
                return false;
            }

            DateTime dateC = new DateTime(date.Ticks);

            while (dateC > firstDeparturTime)
            {
                dateC = dateC.AddTicks((long)flightFrequency);
            }
            if (dateC == firstDeparturTime) return true;
            return false;
        }

        public Schedule FindSchedule(DateTime date)
        {
            if (!WillFlight(date)) throw new Exception("Data not found");

            Schedule returningValue = null;
            schedules.ForEach(schedule =>
           {
               if (schedule.GetDepartureTime() == date) returningValue = schedule;
           });

            return returningValue;
        }

        public void AddPassenger(Customer customer, DateTime date)
        {
            if (!WillFlight(date)) return; //Throw error

            Schedule schedule = FindSchedule(date);
            DateTime arriveTime = date.AddTicks((long)(TimeSpan.TicksPerHour * plane.GetSpeed() / GetDistacne()));

            if (schedule == null)
            {
                schedule = new Schedule(date, arriveTime, 0);
            }
            else if (schedule.NumberOfTicketsBought + schedule.NumberOfTicketsBought < plane.NumberOfTickets)
            {
                throw new System.ApplicationException("Seats limit reached");
            }

            schedules.Add(new Schedule(date, arriveTime, 0));
        }

        public void AddPassenger(Customer customer)
        {
            AddPassenger(customer, firstDeparturTime);
        }

        public DateTime CalculateArriveDateTime()
        {
            return firstDeparturTime.AddTicks((long)(TimeSpan.TicksPerHour * plane.GetSpeed() / GetDistacne()));
        }
    }
}
