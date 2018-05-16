using Microsoft.AspNet.Identity.EntityFramework;
using TestingSystem.DAL.Entities;

namespace TestingSystem.DAL.EF
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(string connectionString) : base(connectionString, throwIfV1Schema: false)
        {
        }
    }
}