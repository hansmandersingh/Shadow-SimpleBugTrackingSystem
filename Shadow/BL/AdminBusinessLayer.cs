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
                if (!UserAndRolesRepository.CheckIfUserIsInRole(userId, roleName))
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
    }
}