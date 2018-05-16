using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TestingSystem.Constants;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Identity;

namespace TestingSystem.DAL.EF
{
    public class InitializerDb : DropCreateDatabaseIfModelChanges<AppIdentityDbContext>
    {
        protected override void Seed(AppIdentityDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(AppIdentityDbContext context)
        {
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole>(context));

            string userName = "Admin";
            string password = "mypassword";
            string email = "admin@gmail.com";

            if (!roleMgr.RoleExists(RoleName.Admin))
                roleMgr.Create(new AppRole(RoleName.Admin));

            if (!roleMgr.RoleExists(RoleName.User))
                roleMgr.Create(new AppRole(RoleName.User));


            AppUser user = userMgr.FindByName(userName);
            if (user == null)
            {
                userMgr.Create(new AppUser { UserName = userName, Email = email }, password);
                user = userMgr.FindByName(userName);
            }

            if (!userMgr.IsInRole(user.Id, RoleName.Admin))
                userMgr.AddToRole(user.Id, RoleName.Admin);

            if (!userMgr.IsInRole(user.Id, RoleName.User))
                userMgr.AddToRole(user.Id, RoleName.User);

            context.SaveChanges();
        }
    }
}