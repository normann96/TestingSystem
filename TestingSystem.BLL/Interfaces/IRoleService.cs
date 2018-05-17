using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TestingSystem.BLL.EntitiesDto;

namespace TestingSystem.BLL.Interfaces
{
    public interface IRoleService : IDisposable
    {

        Task<RoleDto> FindByIdAsync(string roleId);
        Task<IdentityResult> CreateAsync(string roleName);
        Task<IdentityResult> DeleteAsync(string roleId);
        IQueryable<RoleDto> Roles { get; }
        Task<List<RoleDto>> GetAllAsync();
    }
}