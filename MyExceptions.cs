using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    public class AirportDistanceException : Exception
    {
        public AirportDistanceException(String str) : base(str) { }
    }

    public class MaxPassengersReachedException : Exception
    {
        public MaxPassengersReachedException(String str) : base(str) { }
    }

    public class DateIncorrectException : Exception
    {
        public DateIncorrectException(String str) : base(str) { }
    }

    public class FlightFrequencyException : Exception
    {
        public FlightFrequencyException(String str) : base(str) { }
    }

    public class PlaneInUseException : Exception
    {
        public PlaneInUseException(String str) : base(str) { }
    }

    public class AirportInUseException : Exception
    {
        public AirportInUseException(String str) : base(str) { }
    }
}
