using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    [Serializable]
    public class Rout
    {
        public Airport FromAirport { get; private set; }
        public Airport ToAirport { get; private set; }
        public Plane Plane { get; private set; }
        private int routID;
        public FlightFrequency FlightFrequency { get; private set; }
        public DateTime FirstDeparturTime { get; private set; }
        private List<Schedule> schedules = new List<Schedule>();

        public Rout(DateTime firstDeparturTime, Airport fromAirport, Airport toAirport, Plane plane, FlightFrequency flightFrequency)
        {
            this.FirstDeparturTime = firstDeparturTime;
            this.FromAirport = fromAirport;
            this.ToAirport = toAirport;
            this.Plane = plane;
            this.FlightFrequency = flightFrequency;
        }

        public void SetUpFlight()
        {
            schedules.Add(new Schedule(FirstDeparturTime, CalculateArriveDateTime(FirstDeparturTime), this));
        }

        public bool IsSetUp()
        {
            return schedules.Count != 0;
        }

        public void ChangeDepartureTime(DateTime date)
        {
            long tics = date.Ticks - FirstDeparturTime.Ticks;
            schedules.ForEach(s =>
            {
                s.SetTime(s.GetDepartureTime().AddTicks(tics), s.GetArrivalTime().AddTicks(tics));
            });
            FirstDeparturTime = date;
        }

        public void SetFromAirport(Airport airport)
        {
            FromAirport = airport;
        }

        public void SetToAirport(Airport airport)
        {
            ToAirport = airport;
        }

        public float GetDistance()
        {
            return FromAirport.GetDistance(ToAirport);
        }

        public int GetRoutID()
        {
            return routID;
        }

        public void SetFlightFrequency(FlightFrequency flightFrequency)
        {
            this.FlightFrequency = flightFrequency;
        }

        public FlightFrequency GetFlightFrequency()
        {
            return FlightFrequency;
        }

        public Plane GetPlain()
        {
            return Plane;
        }

        public void SetPlain(Plane plane)
        {
            if (Plane != null) this.Plane.Unassign();
            plane.Assign();
            this.Plane = plane;

        }

        public bool IsOneFlight()
        {
            return FlightFrequency == FlightFrequency.ONE_FLIGHT;
        }

        public bool WillFlight(DateTime date)
        {
            if (IsOneFlight())
            {
                if (date == FirstDeparturTime) return true;
                throw new FlightFrequencyException("Lot jest jednorazowy");
            }

            DateTime dateC = new DateTime(date.Ticks);

            while (dateC > FirstDeparturTime)
            {
                dateC = dateC.AddTicks(-(long)FlightFrequency);
            }
            if (dateC == FirstDeparturTime) return true;
            return false;
        }

        public Schedule NextSchedule(Schedule dateContext)
        {
            DateTime time = dateContext.GetDepartureTime().AddTicks((long)FlightFrequency);
            Schedule schedule = FindSchedule(time);
            if (schedule == null) schedule = new Schedule(time, CalculateArriveDateTime(time), this);
            return schedule;
        }
        public Schedule PreviousSchedule(Schedule dateContext)
        {
            DateTime time = new DateTime(dateContext.GetDepartureTime().Ticks - (long)FlightFrequency);
            if (time < DateTime.MinValue || time > DateTime.MaxValue) throw new DateIncorrect("Data jest nieprawidłowa");
            if (time < FirstDeparturTime) throw new DateIncorrect("Data wylotu wcześniejsza niż data pierwszego wylotu");
            Schedule schedule = FindSchedule(time);
            if (schedule == null) schedule = new Schedule(time, CalculateArriveDateTime(time), this);
            return schedule;
        }

        public Schedule FindSchedule(DateTime date)
        {
            if (!WillFlight(date)) throw new Exception("Data not found");

            Schedule returningValue = null;
            schedules.ForEach(schedule =>
           {
               if (schedule.GetDepartureTime() == date)
               {
                   returningValue = schedule;
                   return;
               }
           });

            return returningValue;
        }

        public void AddPassenger(Customer customer, DateTime date)
        {
            if (!WillFlight(date)) return; //Throw error

            Schedule schedule = FindSchedule(date);
            DateTime arriveTime = date.AddTicks((long)(TimeSpan.TicksPerHour * Plane.GetSpeed() / GetDistance()));

            if (schedule == null)
            {
                schedule = new Schedule(date, arriveTime, this);
                schedules.Add(schedule);
            }

            AddPassanger(customer, schedule);
        }

        public void AddPassenger(Customer customer)
        {
            AddPassenger(customer, FirstDeparturTime);
        }

        public void AddPassanger(Customer customer, Schedule schedule)
        {
            if (!schedules.Contains(schedule)) schedules.Add(schedule);


            customer.SetFlightSchedule(schedule);
        }

        public DateTime CalculateArriveDateTime(DateTime time)
        {
            return time.AddTicks((long)(TimeSpan.TicksPerHour * ((float)Plane.GetSpeed() / GetDistance())));
        }


        internal List<Schedule> GetSchedules()
        {
            return schedules;
        }

        public int GetSchedulesSize()
        {
            return schedules.Count;
        }

        public override string ToString()
        {
            if (FromAirport == null || ToAirport == null)
            {
                return "Nieuzupełniony";
            }
            return FromAirport.City + " - " + ToAirport.City;
        }

    }
}
