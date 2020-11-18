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
                    projectManagerRole = new IdentityRole("project manager");
                    roleManager.Create(projectManagerRole);
                }

                //developer role
                var developerRole = roleManager.FindByName("developer");
                if (developerRole == null)
                {
                    developerRole = new IdentityRole("developer");
                    roleManager.Create(developerRole);
                }

                //submitter role
                var submitterRole = roleManager.FindByName("submitter");
                if (submitterRole == null)
                {
                    submitterRole = new IdentityRole("submitter");
                    roleManager.Create(submitterRole);
                }

                //admin test user added here 
                var adminUser = userManager.FindByName("admin@admin.net");
                if (adminUser == null)
                {
                    var newUser = new ApplicationUser()
                    {
                        UserName = "admin@admin.net",
                        Email = "admin@admin.net",
                        PhoneNumber = "010101010101",
                    };

                    userManager.Create(newUser, "Password@1");
                    userManager.SetLockoutEnabled(newUser.Id, false);
                    userManager.AddToRole(newUser.Id, "admin");
                }

                //Seeding Ticket-- AddOns
                var ticketTypes = context.TicketTypes.Any();
                if(ticketTypes == false)
                {
                    TicketType BugTicket = new TicketType() { Name = "Bug" };
                    TicketType ErrorTicket = new TicketType() { Name = "Error" };

                    context.TicketTypes.Add(BugTicket);
                    context.TicketTypes.Add(ErrorTicket);
                    context.SaveChanges();
                }

                var ticketPriorities = context.TicketPriorities.Any();
                if(ticketPriorities == false)
                {
                    TicketPrioritie Low = new TicketPrioritie() { Name = "Low" };
                    TicketPrioritie Medium = new TicketPrioritie() { Name = "Medium" };
                    TicketPrioritie High = new TicketPrioritie() { Name = "High" };

                    context.TicketPriorities.Add(Low);
                    context.TicketPriorities.Add(Medium);
                    context.TicketPriorities.Add(High);
                    context.SaveChanges();
                }

                var ticketStatus = context.TicketStatuses.Any();
                if(ticketStatus == false)
                {
                    TicketStatus NotFixed = new TicketStatus() { Name = "Not Fixed" };
                    TicketStatus Fixed = new TicketStatus() { Name = "Fixed" };
                    TicketStatus BeingFixed = new TicketStatus() { Name = "Being Fixed" };

                    context.TicketStatuses.Add(NotFixed);
                    context.TicketStatuses.Add(Fixed);
                    context.TicketStatuses.Add(BeingFixed);
                    context.SaveChanges();
                }
            }
        }
    }
}
