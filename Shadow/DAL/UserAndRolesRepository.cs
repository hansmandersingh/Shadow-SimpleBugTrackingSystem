using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Shadow.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Shadow.DAL
{
    public static class UserAndRolesRepository
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        private static UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        private static RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

        public static List<ApplicationUser> GetAllUsers()
        {
            return db.Users.Include(r => r.Roles).ToList();
        }
        public static bool AddUser(string Email, string pwdHash)// requires Email and pwd in hash format to work.
        {

            var user = new ApplicationUser { UserName = Email, Email = Email };

            var result = userManager.Create(user, pwdHash);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool DeleteUser(string UserId)
        {
            var user = db.Users.Find(UserId);

            if (user != null)
            {
                var result = userManager.Delete(user);

                if (result.Succeeded)
                {
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

        public static bool UpdateUser(ApplicationUser user)
        {
            var userToUpdate = userManager.FindById(user.Id);

            if (userToUpdate != null)
            {
                userToUpdate.UserName = user.UserName;
                userToUpdate.Email = user.Email;
                userToUpdate.PhoneNumber = user.PhoneNumber;
                userToUpdate.PasswordHash = user.PasswordHash;
            }

            var result = userManager.Update(userToUpdate);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static List<string> GetAllRolesForUser(string userId)
        {
            return userManager.GetRoles(userId).ToList();
        }
        public static bool AssignRoleToUser(string userId, string roleName)
        {
            roleName = roleName.ToLower();
            var result = userManager.AddToRole(userId, roleName);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool DeleteUserFromRole(string userId, string roleName)
        {
            roleName = roleName.ToLower();
            if (roleManager.RoleExists(roleName) && userManager.FindById(userId) != null)
            {
                if (userManager.IsInRole(userId, roleName))
                {
                    var result = userManager.RemoveFromRole(userId, roleName);

                    if (result.Succeeded)
                    {
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
            else
            {
                return false;
            }
        }
        public static bool CheckIfUserIsInRole(string userId, string roleName)
        {

            roleName = roleName.ToLower();
            return userManager.IsInRole(userId, roleName);
        }

        public static bool CreateRole(string roleName)
        {
            roleName = roleName.ToLower();
            if (roleManager.RoleExists(roleName))
            {
                return true;
            }
            else
            {
                var result = roleManager.Create(new IdentityRole(roleName));
                db.SaveChanges();
                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool DeleteRole(string roleName)
        {
            roleName = roleName.ToLower();
            if (roleManager.RoleExists(roleName))
            {
                var users = roleManager.FindByName(roleName).Users.Select(u => u.UserId).ToList();

                users.ForEach(userId => { userManager.RemoveFromRole(userId, roleName); });

                var result = roleManager.Delete(roleManager.FindByName(roleName));

                if (result.Succeeded)
                {
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
    }
}