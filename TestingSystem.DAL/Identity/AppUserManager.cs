using Microsoft.AspNet.Identity;
using TestingSystem.DAL.Entities;

namespace TestingSystem.DAL.Identity
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store) : base(store)
        {
        }
    }
}