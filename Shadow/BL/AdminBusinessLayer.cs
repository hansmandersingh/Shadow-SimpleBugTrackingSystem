using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shadow.DAL;
using Shadow.Models;

namespace Shadow.BL
{
    public class AdminBusinessLayer
    {
        UserAndRolesRepository UserAndRolesRepository = new UserAndRolesRepository();
        ProjectRepository ProjectRepository = new ProjectRepository();
        TicketRepository TicketRepository = new TicketRepository();
        public bool AssignUserToRole(string adminId, string userId, string roleName)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(adminId, "admin"))
            {
                if (!UserAndRolesRepository.CheckIfUserIsInRole(userId, roleName))
                {
                    UserAndRolesRepository.AssignRoleToUser(userId, roleName);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool UnAssignUserFromRole(string adminId, string userId, string roleName)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(adminId, "admin"))
            {
                if (UserAndRolesRepository.CheckIfUserIsInRole(userId, roleName))
                {
                    UserAndRolesRepository.DeleteUserFromRole(userId, roleName);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return UserAndRolesRepository.GetAllUsers();
        }

        public bool AddANewProject(string adminId, string projectName)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(adminId, "admin"))
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

        public bool EditProject(string adminId, Project project)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(adminId, "admin"))
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

            if (UserAndRolesRepository.CheckIfUserIsInRole(userId, "admin"))
            {
                projects = ProjectRepository.ListProjects();
                return projects;
            } else
                return projects;
        }

        public Project GetProject(int projectId)
        {
            return ProjectRepository.GetAProject(projectId);
        }

        public bool AssignUserToAProject(string adminId, string userId, int projectId)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(adminId, "admin"))
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

        public bool UnAssignUserFromProject(string adminId, string userId, int projectId)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(adminId, "admin"))
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

        public List<Ticket> GetAllTickets()
        {
            return TicketRepository.GetAllTickets();
        }

        public bool AddCommentOnTicket(string userId,int ticketId , string commentText)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(userId, "admin"))
            {
                var result = TicketRepository.AddComment(userId, ticketId, commentText);

                if (result)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public List<TicketComment> AllComments(int ticketId)
        {
            return TicketRepository.ShowAllComments(ticketId);
        }

        public bool AddAttachment(string userId, int ticketId, string fileUrl, string filePath, string description)
        {
            if(UserAndRolesRepository.CheckIfUserIsInRole(userId, "admin"))
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

        public bool EditTicket(string userId, Ticket ticket)
        {
            if (UserAndRolesRepository.CheckIfUserIsInRole(userId, "admin"))
            {
                TicketRepository.EditTicket(ticket, userId);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Ticket GetTicket(int ticketId)
        {
            return TicketRepository.GetTicket(ticketId);
        }

        public List<TicketType> TicketTypes()
        {
            return TicketRepository.AllTicketTypes();
        }

        public List<TicketStatus> TicketStatuses()
        {
            return TicketRepository.TicketStatuses();
        }

        public List<TicketPrioritie> TicketPriorities()
        {
            return TicketRepository.TicketPriorities();
        }
    }
}