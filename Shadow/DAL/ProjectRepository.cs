using Shadow.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Shadow.DAL
{
    public class ProjectRepository
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public bool CreateProject(string projectName)
        {
            Project project = new Project() { Name = projectName };

            if(db.Projects.Any(p => p.Name == projectName) == false)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditProject(Project pro)
        {
            var project = db.Projects.Find(pro.Id);

            if (project != null)
            {
                project.Name = pro.Name;
                project.ProjectUsers = pro.ProjectUsers;
                project.Tickets = pro.Tickets;
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            } else
            {
                return false;
            }
        }

        public List<Project> ListProjects()
        {
            return db.Projects.Include(p => p.ProjectUsers).Include(q => q.Tickets).ToList();
        }

        public List<Project> ListProjects(string userId)
        {
            return db.ProjectUsers.Where(p => p.UserId == userId).Select(s => s.Project).ToList();
        }

        public bool AssignUserToProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var user = db.Users.Find(userId);
            ProjectUser projectUser = new ProjectUser() { ProjectId = projectId, UserId = userId };

            if (project != null && user != null)
            {
                db.ProjectUsers.Add(projectUser);
                project.ProjectUsers.Add(projectUser);
                user.ProjectUsers.Add(projectUser);
                db.Entry(project).State = EntityState.Modified;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public bool UnAssignUserFromProject(string userId, int projectId)
        {
            var projectUser = db.ProjectUsers.FirstOrDefault(u => u.ProjectId == projectId && u.UserId == userId);

            if (projectUser != null)
            {
                db.ProjectUsers.Remove(projectUser);
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }
    }
}