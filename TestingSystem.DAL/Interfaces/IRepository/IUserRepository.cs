using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TestingSystem.DAL.Entities;

namespace TestingSystem.DAL.Interfaces.IRepository
{
    public interface IUserRepository : IRepository<AppUser>
    {
        Task<AppUser> FindAsync(string userName, string password);
        Task<AppUser> FindByIdAsync(string userId);
        Task<AppUser> FindByNameAsync(string userName);
        Task<IdentityResult> CreateAsync(AppUser user, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(AppUser user, string authenticationType);
        Task<IdentityResult> UpdateAsync(AppUser user);
        Task<IdentityResult> DeleteAsync(AppUser user);
        Task<IdentityResult> AddToRoleAsync(string userId, string role);
        Task<IdentityResult> RemoveFromRoleAsync(string userId, string role);
    }
}