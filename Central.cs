using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    [Serializable]
    public class Central
    {
        public List<Rout> Routs { get; }
        public List<Plane> Planes { get; }
        public List<Customer> Customers { get; }
        public List<Airport> Airports { get; }

        public Central()
        {
            Routs = new List<Rout>();
            Planes = new List<Plane>();
            Customers = new List<Customer>();
            Airports = new List<Airport>();
        }
        public void GenerateRout(Rout rout, DateTime date, Airport fromAirport, Airport toAirport, int seats, FlightFrequency flightFrequency)
        {
            float distance = fromAirport.GetDistance(toAirport);
            Plane plane = FindPlane(distance, seats);

            if (plane == null) throw new Exception("Brak samolotu spełniającego kryteria :(");

            rout.SetUpFlight(date, fromAirport, toAirport, plane, flightFrequency);
        }

        private Plane FindPlane(float distance, int seats)
        {
            Plane planeReturn = null;
            Planes.ForEach(p =>
            {
                if (p.IsFree && p.Range >= distance && p.NumberOfTickets >= seats)
                {
                    planeReturn = p;
                    return;
                }
            });
            return planeReturn;
        }

        
        public void AddPlain(Plane plane)
        {
            if (Planes.Contains(plane)) throw new ApplicationException("Plane already exist");
            Planes.Add(plane);
        }

        public void AddCustomer(Customer customer)
        {
            if (Customers.Contains(customer)) throw new ApplicationException("Customer already exist");
            Customers.Add(customer);
        }

        public void AddRout(Rout route)
        {
            if (Routs.Contains(route)) throw new ApplicationException("Rout already exist");
            Routs.Add(route);
        }

        public void AddAirport(Airport airport)
        {
            if (Airports.Contains(airport)) throw new ApplicationException("Aiport already exist");
            Airports.Add(airport);
        }
        
        public void RemovePlain(Plane plane)
        {
            if (!plane.IsFree)
            {
                throw new PlaneInUseException("Samolot jest w użyciu. Zmien trasę która wykorzystuje ten samolot, albo usuń trasę");
            }
            Planes.Remove(plane);
        }

        public void RemoveCustomer(Customer customer)
        {
            if (customer.FlightSchedule != null)
                customer.FlightSchedule.RemovePassenger(customer);
            Customers.Remove(customer);
        }

        public void RemoveRout(Rout rout)
        {
            //Gdy trasa jest stworzona i ustalona
            if (rout.IsSetUp())
            {
                //Lista psażerów do usunięcia
                List<Customer> cusToRemove = new List<Customer>();
                rout.GetSchedules().ForEach(s =>
                {
                    s.GetPassengersList().ForEach(p =>
                    {
                        cusToRemove.Add(p);
                        //p.SetFlightSchedule(null);
                    });
                });
                //Usuń loty dla każdego pasażera
                cusToRemove.ForEach(c => c.SetFlightSchedule(null));
            }
            Routs.Remove(rout);

            //Ustaw status samolotu na wolny i sprawdź czy jest wykorzystywany w innych lotach,
            //Jak tak to zmień jego status ponownie
            rout.Plane.Unassign();
            Routs.ForEach(r =>
            {
                if (r.Plane == rout.Plane)
                {
                    rout.Plane.Assign();
                    return;
                }
            });
        }

        public void RemoveAiport(Airport airport)
        {
            if (airport.IsSetUp)
            {
                //Czy przypisany do lotu?
                bool inUse = false;
                Routs.ForEach(r =>
                {
                    if (r.FromAirport == airport || r.ToAirport == airport)
                    {
                        inUse = true;
                        return;
                    }
                });
                if (inUse) throw new AirportInUseException("Lotnisko jest w użyciu. Zmień trasę która wykorzystuje to lotnisko, albo unuń trasę");
            }
            Airports.Remove(airport);
        }
    }
}
