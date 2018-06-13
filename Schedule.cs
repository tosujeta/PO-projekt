using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    class Schedule
    {
        private int Departuretime;
        private int Arrivaltime;
        private int flightID;
        private int Numberofticketsbought;
        
        public Schedule(int Departuretime, int Arrivaltime, int flightID)
        {
            this.Departuretime = Departuretime;
            this.Arrivaltime = Arrivaltime;
            this.flightID = flightID;
        }
        public int GetDeparturetime()
        {
            return Departuretime;
        }
        public int GetArrivaltime()
        {
            return Arrivaltime;
        }
        public int GetflightID()
        {
            return flightID;
        }
        public void SetTime(int Departuretime, int Arrivaltime)
        {
            this.Departuretime = Departuretime;
            this.Arrivaltime = Arrivaltime;
        }
        
    }
}
