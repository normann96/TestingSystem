using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TestingSystem.DAL.Entities;

namespace TestingSystem.DAL.Identity
{
    public class AppRoleManager : RoleManager<AppRole>
    {
        public AppRoleManager(RoleStore<AppRole> store) : base(store)
        {
        }
    }
}