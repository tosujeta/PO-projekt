using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    class Central
    {
        public List<Rout> Routs;
        public List<Plane> Planes;
        public List<Customer> Customers { get; }
        private List<Airport> Airports;

        public Central()
        {
            Routs = new List<Rout>();
            Planes = new List<Plane>();
            Customers = new List<Customer>();
            Airports = new List<Airport>();
        }

    }
}
