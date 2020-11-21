using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shadow.Models
{
    public class TicketAddOns
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }

    public class TicketNotification: TicketAddOns
    {
        public string NotificationDescription { get; set; }
    }
    public class TicketHistorie : TicketAddOns
    {
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
    public class TicketComment : TicketAddOns
    {
        public string Comment { get; set; }
        public DateTime Created { get; set; }
        public TicketComment()
        {
            Created = DateTime.Now;
        }
    }
    public class TicketAttachement : TicketAddOns
    {
        public string FilePath { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string FileUrl { get; set; }
        public TicketAttachement()
        {
            Created = DateTime.Now;
        }
    }
}