using Shadow.DAL;
using Shadow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shadow.BL
{
    public class DeveloperBusinessLayer
    {
        UserAndRolesRepository UserAndRolesRepository = new UserAndRolesRepository();
        ProjectRepository ProjectRepository = new ProjectRepository();
        TicketRepository TicketRepository = new TicketRepository();

       
        public List<Project> AllProject(string userId)
        {
            List<Project> projects = new List<Project>();

            if (UserAndRolesRepository.CheckIfUserIsInRole(userId, "developer"))
            {
                projects = ProjectRepository.ListProjects();
                return projects;
            }
            else
                return projects;
        }
        public List<TicketType> TicketTypes()
        {
            return TicketRepository.AllTicketTypes();
        }
        public List<Ticket> GetAllTickets(string userId)
        {
            List<Ticket> tickets = new List<Ticket>();

            if (UserAndRolesRepository.CheckIfUserIsInRole(userId, "developer"))
            {
                tickets = TicketRepository.GetAllTicketsFromProject(userId);
            }

            return tickets;
        }
        public List<Ticket> ticketsAssignToDeveloper(string userId)
        {
            List<Ticket> tickets = new List<Ticket>();

            if (UserAndRolesRepository.CheckIfUserIsInRole(userId, "developer"))
            {
                tickets = TicketRepository.GetAllTickets();
            }

            return tickets;
        }
        public List<TicketPrioritie> AllTicketPriorities()
        {
            return TicketRepository.TicketPriorities();
        }

        public List<TicketStatus> TicketStatuses()
        {
            return TicketRepository.TicketStatuses();
        }
        public bool AddComments(string userId, int ticketId, string commentText)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(userId, "developer"))
            {
                var result = TicketRepository.AddComment(userId, ticketId, commentText);

                if (result)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public List<TicketComment> AllComments(int ticketId)
        {
            return TicketRepository.ShowAllComments(ticketId);
        }
        public bool AddAttachment(string userId, int ticketId, string fileUrl, string filePath, string description)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(userId, "developer"))
            {
                var result = TicketRepository.AddAttachment(userId, ticketId, fileUrl, filePath, description);

                if (result)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public List<TicketAttachement> ShowAllAttachments(int ticketId)
        {
            return TicketRepository.ShowAllAttachments(ticketId);
        }
        public List<TicketHistorie> FullHistory(int ticketId)
        {
            return TicketRepository.FullHistory(ticketId);
        }
        public List<TicketNotification> AllNotification(int ticketId)
        {
            return TicketRepository.AllNotificaion(ticketId);
        }
    }
}