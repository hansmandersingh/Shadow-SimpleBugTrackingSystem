using Shadow.DAL;
using Shadow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shadow.BL
{
    public class ProjectManagerBusinessLayer
    {
        UserAndRolesRepository UserAndRolesRepository = new UserAndRolesRepository();
        ProjectRepository ProjectRepository = new ProjectRepository();
        TicketRepository TicketRepository = new TicketRepository();

        public bool AddANewProject(string projectManagerId, string projectName)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(projectManagerId, "project manager"))
            {
                var result = ProjectRepository.CreateProject(projectName);
                if (result)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool EditProject(string projectManagerId, Project project)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(projectManagerId, "project manager"))
            {
                var result = ProjectRepository.EditProject(project);
                if (result)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public List<Project> GetAllofMyProjects(string userId)
        {
            return ProjectRepository.ListProjects(userId);
        }

        public List<Project> AllProject(string userId)
        {
            List<Project> projects = new List<Project>();

            if (UserAndRolesRepository.CheckIfUserIsInRole(userId, "project manager"))
            {
                projects = ProjectRepository.ListProjects();
                return projects;
            }
            else
                return projects;
        }

        public Project GetProject(int projectId)
        {
            return ProjectRepository.GetAProject(projectId);
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return UserAndRolesRepository.GetAllUsers();
        }

        public bool AssignUserToAProject(string projectMgrId, string userId, int projectId)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(projectMgrId, "project manager"))
            {
                var result = ProjectRepository.AssignUserToProject(userId, projectId);

                if (result)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool UnAssignUserFromProject(string projectMgrId, string userId, int projectId)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(projectMgrId, "project manager"))
            {
                var result = ProjectRepository.UnAssignUserFromProject(userId, projectId);

                if (result)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public List<Ticket> GetAllTickets(string userId)
        {
            List<Ticket> tickets = new List<Ticket>();

            if (UserAndRolesRepository.CheckIfUserIsInRole(userId, "project manager"))
            {
                tickets = TicketRepository.GetAllTicketsFromProject(userId);
            }

            return tickets;
        }

        public bool EditTicket(string userId, Ticket ticket)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(userId, "project manager"))
            {
                TicketRepository.EditTicket(ticket);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}