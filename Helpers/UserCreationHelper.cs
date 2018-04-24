using CustomerManager.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;

namespace CustomerManager.Helpers
{
    public static class UserActionsHelper
    {
        public static void CreateUser(string username, string email, string password, string userRole)
        {
            var context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            if (!roleManager.RoleExists(userRole))
            {
                var role = new IdentityRole();
                role.Name = userRole;
                roleManager.Create(role);
            }

            var user = new ApplicationUser();
            user.UserName = username;
            user.Email = email;
            var chkUser = UserManager.Create(user, password);

            if (chkUser.Succeeded)
            {
                UserManager.AddToRole(user.Id, userRole);

            }

        }

        public static void ChangeUserData(string username, string newUsername, string newEmail, string newPassword)
        {
            var context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var user = userManager.FindByName(username);

            user.UserName = newUsername;
            user.Email = newEmail;

            userManager.RemovePassword(user.Id);
            userManager.AddPassword(user.Id, newPassword);
            userManager.Update(user);
        }
    }
}