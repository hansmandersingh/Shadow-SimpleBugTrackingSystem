using Shadow.DAL;
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
        public bool CreateTicket(string title, string ownerId, int projectId, string description)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(ownerId, "submitter"))
            {
                var result = TicketRepository.CreateTicket(title, ownerId, projectId, description);

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
    }
}