using FC.SecurityDemo.XSRF.Example04.GoodSite.Data;
using FC.SecurityDemo.XSRF.Example04.GoodSite.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FC.SecurityDemo.XSRF.Example04.GoodSite.Utilities
{
    public static class IdentityDataInitializer
    {
        public static void SeedData(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {            
            SeedUsers(userManager, db);
        }

        public static void SeedUsers(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            createUser("bob", "P@ssw0rd", userManager);
            createUser("alice", "P@ssw0rd", userManager);
            createUser("mallory", "P@ssw0rd", userManager);

            createAccountBalance("bob", 100000, db);
            createAccountBalance("alice", 50000, db);
            createAccountBalance("mallory", 0, db);
        }        

        private static void createAccountBalance(string userName, decimal initialCredit, ApplicationDbContext db)
        {
            AccountBalance acc = new AccountBalance();
            acc.UserName = $"{userName}@localhost.it";
            acc.Credit = initialCredit;

            db.Accounts.Add(acc);
            db.SaveChanges();
        }

        private static void createUser(string userName, string password, UserManager<IdentityUser> userManager)
        {
            try
            {
                var existingUser = userManager.FindByNameAsync(userName).Result;
                if (existingUser == null)
                {
                    IdentityUser user = new IdentityUser();
                    user.UserName = $"{userName}@localhost.it";
                    user.Email = $"{userName}@localhost.it";
                    user.EmailConfirmed = true;
                    IdentityResult result = userManager.CreateAsync(user, password).Result;                    
                }
            }
            catch (Exception)
            {
                //Error
            }
        }
    }
}
