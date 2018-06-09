using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po_proj
{
    class Customer
    {
        private Ticket ticket;
        private String name, surname;



        public Customer(String name, String surname)
        {
            this.name = name;
            this.surname = surname;
        }

        public void SetTicket(Ticket ticket)
        {
            this.ticket = ticket;
        }

        public Ticket GetTicket()
        {
            return ticket;
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
            return name;
        }

        public String GetSurname()
        {
            return surname;
        }

        public override string ToString()
        {
            return name + " " + surname;
        }
    }
}
