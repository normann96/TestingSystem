using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TestingSystem.DAL.EF;
using TestingSystem.DAL.Entities;

namespace TestingSystem.DAL.Identity
{
    public class AppRoleManager : RoleManager<AppRole>
    {
        public AppRoleManager(RoleStore<AppRole> store) : base(store)
        {
        }

        public static AppRoleManager CreateWithConfig(AppIdentityDbContext db)
        {
            // Add configuration if need
            return new AppRoleManager(new RoleStore<AppRole>(db));
        }
    }
}