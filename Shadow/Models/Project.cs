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
        public ICollection<ProjectUser> ProjectUsers { get; set; }
        public Project()
        {
            ProjectUsers = new HashSet<ProjectUser>();
        }
    }
}