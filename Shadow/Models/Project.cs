using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shadow.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public Project()
        {
            ProjectUsers = new HashSet<ProjectUser>();
            Tickets = new HashSet<Ticket>();
        }
    }
}