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
        public List<Rout> routs { get; }
        public List<Plane> planes { get; }
        public List<Customer> customers { get; private set; }
        public List<Airport> airports { get; }

        public Central()
        {
            routs = new List<Rout>();
            planes = new List<Plane>();
            customers = new List<Customer>();
            airports = new List<Airport>();
        }
        public Rout GenerateRout(DateTime flightTime, Airport fromAirport, Airport toAirport, FlightFrequency flightFrequency)
        {
            float distance = fromAirport.GetDistance(toAirport);
            Plane plane = FindPlane(distance);

            if (plane == null) throw new ApplicationException("There is no plain that meet requirements");

            Rout rout = new Rout(flightTime, fromAirport, toAirport, plane, flightFrequency);

            AddAirport(fromAirport);
            AddAirport(toAirport);
            AddRout(rout);

            return rout;
        }

        private Plane FindPlane(float distance)
        {
            Plane planeReturn = null;
            planes.ForEach(p =>
            {
                if (p.IsFree && p.GetRange() >= distance)
                {
                    p = planeReturn;
                    return;
                }
            });
            return planeReturn;
        }

        public void AddPlain(Plane plane)
        {
            if (planes.Contains(plane)) throw new ApplicationException("Plane already exist");
            planes.Add(plane);
        }

        public void AddCustomer(Customer customer)
        {
            if (customers.Contains(customer)) throw new ApplicationException("Customer already exist");
            customers.Add(customer);
        }

        public void AddRout(Rout route)
        {
            if(routs.Contains(route)) throw new ApplicationException("Rout already exist");
            routs.Add(route);
        }

        public void AddAirport(Airport airport)
        {
            if (airports.Contains(airport)) throw new ApplicationException("Aiport already exist");
            airports.Add(airport);
        }

        public void RemovePlain(Plane plane)
        {
            planes.Remove(plane);
            //TODO: Sprawdz czy samolot jest juz gdzies przypisany
        }
        
        public void RemoveCustomer(Customer customer)
        {
            customers.Remove(customer);
            //TODO: Sprawdz czy on jest gdzies przypisany
        }

        public void RemoveRout(Rout rout)
        {
            routs.Remove(rout);
            //TODO: Czy przypisany
        }

        public void RemoveAiport(Airport airport)
        {
            airports.Remove(airport);
            //TODO: czy przypisany
        }
    }
}
