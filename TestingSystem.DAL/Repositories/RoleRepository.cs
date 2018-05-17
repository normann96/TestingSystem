using System.Data.Entity;
using System.Linq;
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
    public class RoleRepository : BaseRepository<AppRole>, IRoleRepository
    {
        public AppRoleManager RoleManager { get; }
        public RoleRepository(AppIdentityDbContext db) : base(db)
        {
            RoleManager = AppRoleManager.CreateWithConfig(db);
        }

        #region IRoleRepository Members

        public AppRole FindByName(string roleName)
        {
            return Set.FirstOrDefault(x => x.Name == roleName);
        }

        public async Task<AppRole> FindByNameAsync(string roleName)
        {
            return await Set.FirstOrDefaultAsync(x => x.Name == roleName);
        }

        public async Task<AppRole> FindByIdAsync(string roleId)
        {
            return await Set.FirstOrDefaultAsync(x => x.Id == roleId);
        }

        public async Task<IdentityResult> CreateAsync(AppRole role)
        {
            return await RoleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> DeleteAsync(AppRole role)
        {
            return await RoleManager.DeleteAsync(role);
        }

        public bool RoleExists(string roleName)
        {
            return RoleManager.RoleExists(roleName);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await RoleManager.RoleExistsAsync(roleName);
        }
        #endregion
    }
}