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