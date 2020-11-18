﻿using Shadow.DAL;
using Shadow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shadow.BL
{
    public class SubmitterBusinessLayer
    {
        TicketRepository TicketRepository = new TicketRepository();
        UserAndRolesRepository UserAndRolesRepository = new UserAndRolesRepository();
        ProjectRepository ProjectRepository = new ProjectRepository();
        public bool CreateTicket(string title, string ownerId, int projectId, string description, int ticketTypeId, int ticketPrioritiesId, int ticketStatusId)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(ownerId, "submitter"))
            {
                var result = TicketRepository.CreateTicket(title, ownerId, projectId, description, ticketTypeId, ticketPrioritiesId, ticketStatusId);

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

        public List<Project> ListProjects(string userId)
        {
            return ProjectRepository.ListProjects(userId);
        }

        public List<TicketType> TicketTypes()
        {
            return TicketRepository.AllTicketTypes();
        }

        public List<TicketPrioritie> AllTicketPriorities()
        {
            return TicketRepository.TicketPriorities();
        }

        public List<TicketStatus> TicketStatuses()
        {
            return TicketRepository.TicketStatuses();
        }
    }
}