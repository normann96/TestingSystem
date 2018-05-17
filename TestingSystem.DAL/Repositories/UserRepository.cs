using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TestingSystem.DAL.EF;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Identity;
using TestingSystem.DAL.Interfaces.IRepository;
using TestingSystem.DAL.Repositories.Base;

namespace TestingSystem.DAL.Repositories
{
    public class UserRepository : BaseRepository<AppUser>, IUserRepository
    {
        public AppUserManager UserManager { get; }

        public UserRepository(AppIdentityDbContext db) : base(db)
        {
            UserManager = AppUserManager.CreateWithConfig(db);
        }

        #region IUserRepository Members
        public async Task<AppUser> FindAsync(string userName, string password)
        {
            return await UserManager.FindAsync(userName, password);
        }

        public async Task<AppUser> FindByIdAsync(string userId)
        {
            return await UserManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            return await UserManager.CreateAsync(user, password);
        }

        public async Task<ClaimsIdentity> CreateIdentityAsync(AppUser user, string authenticationType)
        {
            return await UserManager.CreateIdentityAsync(user, authenticationType);
        }

        public async Task<IdentityResult> UpdateAsync(AppUser user)
        {
            return await UserManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(AppUser user)
        {
            return await UserManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> AddToRoleAsync(string userId, string role)
        {
            return await UserManager.AddToRoleAsync(userId, role);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(string userId, string role)
        {
            return await UserManager.RemoveFromRoleAsync(userId, role);
        }
        #endregion
    }
}