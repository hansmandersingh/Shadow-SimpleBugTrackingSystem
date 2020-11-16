using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shadow.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int TicketStatusId { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public int TicketPrioritieId { get; set; }
        public TicketPrioritie TicketPrioritie { get; set; }
        public int TicketTypeId { get; set; }
        public TicketType TicketType { get; set; }
    }
}