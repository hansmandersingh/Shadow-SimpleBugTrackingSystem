namespace Shadow.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Shadow.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Shadow.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Shadow.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            if (!context.Roles.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                //Add missing roles
                //admin role
                var adminRole = roleManager.FindByName("admin");
                if (adminRole == null)
                {
                    adminRole = new IdentityRole("admin");
                    roleManager.Create(adminRole);
                }

                //Project Manager role
                var projectManagerRole = roleManager.FindByName("project manager");
                if (projectManagerRole == null)
                {
                    projectManagerRole = new IdentityRole("admin");
                    roleManager.Create(projectManagerRole);
                }

                //developer role
                var developerRole = roleManager.FindByName("developer");
                if (developerRole == null)
                {
                    developerRole = new IdentityRole("admin");
                    roleManager.Create(developerRole);
                }

                //submitter role
                var submitterRole = roleManager.FindByName("submitter");
                if (submitterRole == null)
                {
                    submitterRole = new IdentityRole("admin");
                    roleManager.Create(submitterRole);
                }
            }
        }
    }
}
