using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TestingSystem.DAL.Entities;

namespace TestingSystem.DAL.Interfaces.IRepository
{
    public interface IRoleRepository : IRepository<AppRole>
    {
        AppRole FindByName(string roleName);
        Task<AppRole> FindByNameAsync(string roleName);
        Task<AppRole> FindByIdAsync(string roleId);
        Task<IdentityResult> CreateAsync(AppRole role);
        Task<IdentityResult> DeleteAsync(AppRole role);
        bool RoleExists(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
    }
}