using ObjectsComparer;
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
        UserAndRolesRepository UserAndRolesRepository = new UserAndRolesRepository();
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

            TicketHistorie history = new TicketHistorie()
            {
                UserId = ownerId,
                TicketId = ticket.Id,
                Property = "Initialized Ticket",
                OldValue = "-",
                NewValue = "-"
            };

            if(!db.Tickets.Any(t => t.Title == ticket.Title))
            {
                ticket.TicketHistories.Add(history);
                db.Tickets.Add(ticket);
                db.TicketHistories.Add(history);
                db.SaveChanges();
                return true;
            } else
            {
                return false;
            }
        }

        public bool EditTicket(Ticket ticket, string UserIdForHistory)
        {
            Ticket oldTicket = db.Tickets.FirstOrDefault(t => t.Id == ticket.Id);

            if(oldTicket != null)
            {
                CompareObjects(oldTicket, ticket, UserIdForHistory);
                oldTicket.OwnerId = ticket.OwnerId;
                oldTicket.ProjectId = ticket.ProjectId;
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

        public void CompareObjects(Ticket ticket1, Ticket ticket2, string UserId)
        {
            var comparer = new Comparer();
            IEnumerable<Difference> differences;
            comparer.AddComparerOverride<ICollection<TicketComment>>(DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride<ICollection<TicketAttachement>>(DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride<ICollection<TicketHistorie>>(DoNotCompareValueComparer.Instance);
            var isEqual = comparer.Compare(ticket1, ticket2, out differences);

            foreach(var diff in differences)
            {
                TicketHistorie historie = new TicketHistorie()
                {
                    UserId = UserId,
                    TicketId = ticket1.Id,
                    Property = diff.MemberPath.ToString(),
                    OldValue = diff.Value1.ToString(),
                    NewValue = diff.Value2.ToString(),
                };

                ticket2.TicketHistories.Add(historie);
                db.TicketHistories.Add(historie);
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

        public bool AssignToDeveloper(int ticketId, string assignedToId)
        {
            var ticket = db.Tickets.FirstOrDefault(t => t.Id == ticketId);

            if (UserAndRolesRepository.CheckIfUserIsInRole(assignedToId, "developer"))
            {
                TicketNotification notification = new TicketNotification()
                {
                    TicketId = ticketId,
                    UserId = assignedToId
                };

                db.TicketNotifications.Add(notification);
                ticket.TicketNotifications.Add(notification);
            }

            if (ticket != null)
            {
                ticket.AssignedToUserId = assignedToId;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UnAssignTicket(int ticketId)
        {
            var ticket = db.Tickets.FirstOrDefault(t => t.Id == ticketId);

            if (ticket != null)
            {
                ticket.AssignedToUserId = null;
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public bool AddComment(string userId, int ticketId, string commentText)
        {
            var ticket = db.Tickets.FirstOrDefault(t => t.Id == ticketId);
            TicketComment comment = new TicketComment() { TicketId = ticketId, UserId = userId, Comment = commentText };

            if (ticket != null)
            {
                ticket.TicketComments.Add(comment);
                db.TicketComments.Add(comment);
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<TicketComment> ShowAllComments(int ticketId)
        {
            return db.TicketComments.Include(i => i.Ticket).Include(i => i.User).Where(t => t.TicketId == ticketId).ToList();
        }

        public bool AddAttachment(string userId, int ticketId, string fileUrl, string filePath, string description)
        {
            var ticket = db.Tickets.FirstOrDefault(t => t.Id == ticketId);
            TicketAttachement attachement = new TicketAttachement()
            {
                UserId = userId,
                TicketId = ticketId,
                FileUrl = fileUrl,
                FilePath = filePath,
                Description = description,
            };

            if (ticket != null)
            {
                ticket.TicketAttachements.Add(attachement);
                db.TicketAttachements.Add(attachement);
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<TicketAttachement> ShowAllAttachments(int ticketId)
        {
            return db.TicketAttachements.Include(i => i.User).Include(i => i.Ticket).Where(t => t.TicketId == ticketId).ToList();
        }

        public TicketStatus TicketStatus(int ticketStatusId)
        {
            return db.TicketStatuses.FirstOrDefault(t => t.Id == ticketStatusId);
        }

        public TicketPrioritie TicketPrioritie(int ticketPrioritieId)
        {
            return db.TicketPriorities.FirstOrDefault(t => t.Id == ticketPrioritieId);
        }

        public TicketType TicketType(int TicketTypeId)
        {
            return db.TicketTypes.FirstOrDefault(t => t.Id == TicketTypeId);
        }

        public List<TicketHistorie> FullHistory(int ticketId)
        {
            return db.TicketHistories.Include(i => i.User).Include(i => i.Ticket).Where(t => t.TicketId == ticketId).ToList();
        }
    }
}