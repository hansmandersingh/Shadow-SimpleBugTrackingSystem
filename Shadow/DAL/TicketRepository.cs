using Shadow.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Shadow.DAL
{
    public class TicketRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public bool CreateTicket(string title, string ownerId, int projectId, string description, int ticketTypeId, int ticketPrioritiesId, int ticketStatusId)
        {
            Ticket ticket = new Ticket()
            {
                Title = title,
                Created = DateTime.Now,
                OwnerId = ownerId,
                ProjectId = projectId,
                Description = description,
                TicketTypeId = ticketTypeId,
                TicketPrioritieId = ticketPrioritiesId,
                TicketStatusId = ticketStatusId
            };

            if(!db.Tickets.Any(t => t.Title == ticket.Title))
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return true;
            } else
            {
                return false;
            }
        }

        public bool EditTicket(Ticket ticket)
        {
            var oldTicket = db.Tickets.Find(ticket.Id);

            if(oldTicket != null)
            {
                oldTicket.OwnerId = ticket.OwnerId;
                oldTicket.ProjectId = ticket.ProjectId;
                oldTicket.TicketAttachements = ticket.TicketAttachements;
                oldTicket.TicketComments = ticket.TicketComments;
                oldTicket.TicketHistories = ticket.TicketHistories;
                oldTicket.TicketNotifications = ticket.TicketNotifications;
                oldTicket.TicketPrioritieId = ticket.TicketPrioritieId;
                oldTicket.TicketStatusId = ticket.TicketStatusId;
                oldTicket.TicketTypeId = ticket.TicketTypeId;
                oldTicket.Updated = ticket.Updated;
                oldTicket.Title = ticket.Title;
                oldTicket.Description = ticket.Description;
                oldTicket.Created = ticket.Created;
                oldTicket.AssignedToUserId = ticket.AssignedToUserId;
                db.Entry(oldTicket).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public Ticket GetTicket(int ticketId)
        {
            return db.Tickets.FirstOrDefault(t => t.Id == ticketId);
        }

        public List<Ticket> GetAllTickets()
        {
            return db.Tickets.Include(i => i.Project).Include(i => i.Owner).Include(i => i.TicketStatus).Include(i => i.TicketPrioritie).Include(i => i.AssignedToUser).ToList();
        }

        public List<Ticket> GetAllTicketsFromProject(string userId)
        {
            return db.ProjectUsers.Where(s => s.UserId == userId).Select(s => s.Project).SelectMany(s => s.Tickets).ToList(); 
        }

        public bool DeleteTicket(int ticketId)
        {
            var ticket = db.Tickets.Find(ticketId);

            if(ticket != null)
            {
                db.Tickets.Remove(ticket);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<TicketType> AllTicketTypes()
        {
            return db.TicketTypes.ToList();
        }

        public List<TicketPrioritie> TicketPriorities()
        {
            return db.TicketPriorities.ToList();
        }

        public List<TicketStatus> TicketStatuses()
        {
            return db.TicketStatuses.ToList();
        }
    }
}