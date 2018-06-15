using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    public class Customer
    {
        public Ticket Ticket { get; set; }
        public String Name {get; set; }
        public String Surname {get;set; }



        public Customer(String name, String surname)
        {
            this.Name = name;
            this.Surname = surname;
        }

        public void SetTicket(Ticket ticket)
        {
            this.Ticket = ticket;
        }

        public Ticket GetTicket()
        {
            return Ticket;
        }

        /**
         * TODO: TO chyba nie bedzie potrzebne
         * mozna uyzc SetTicket.
         * */
        public void ChangeBilet()
        {

        }

        public String GetName()
        {
            return Name;
        }

        public String GetSurname()
        {
            return Surname;
        }

        public override string ToString()
        {
            return Name + " " + Surname;
        }
    }
}
