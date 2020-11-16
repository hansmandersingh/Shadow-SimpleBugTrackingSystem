using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shadow.Models
{
    public class TicketAccessories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public TicketAccessories()
        {
            Tickets = new HashSet<Ticket>();
        }
    }

    public class TicketStatus : TicketAccessories
    {

    }
    public class TicketPrioritie : TicketAccessories
    {

    }
    public class TicketType : TicketAccessories
    {

    }
}