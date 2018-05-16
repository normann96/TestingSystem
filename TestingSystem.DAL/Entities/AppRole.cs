using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TestingSystem.DAL.Entities
{
    public class AppRole : IdentityRole
    {
        public AppRole() { }

        public AppRole(string name) : base(name)
        {
        }
    }
}