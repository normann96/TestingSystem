using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TestingSystem.BLL.EntitiesDto;
using TestingSystem.BLL.Interfaces;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Interfaces;

namespace TestingSystem.BLL.Services
{
    public class RoleService : IRoleService
    {
        private IIdentityUnitOfWork Database { get; }
        public RoleService(IIdentityUnitOfWork uow)
        {
            Database = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        #region IRoleService Members

        public async Task<RoleDto> FindByIdAsync(string roleId)
        {
            var appRole = await Database.RoleRepository.GetByIdAsync(roleId);
            if (appRole == null)
                return null;

            return new RoleDto { Id = appRole.Id, Name = appRole.Name};
        }

        public async Task<IdentityResult> CreateAsync(string roleName)
        {
            AppRole appRole = new AppRole(roleName);
            return await Database.RoleRepository.CreateAsync(appRole);
        }

        public async Task<IdentityResult> DeleteAsync(string roleId)
        {
            var role = await Database.RoleRepository.FindByIdAsync(roleId);
            if(role == null) 
                return IdentityResult.Failed("Role not found", roleId);

            return await Database.RoleRepository.DeleteAsync(role);
        }

        public IQueryable<RoleDto> Roles
        {
            get
            {
                var appRoles = Database.RoleRepository.GetQueryable();
                if (appRoles == null)
                    return null;

                var roleDtos = new List<RoleDto>();
                foreach (var appRole in appRoles)
                {
                    roleDtos.Add(new RoleDto
                    {
                        Id = appRole.Id,
                        Name = appRole.Name,
                    });
                }

                return roleDtos.AsQueryable();
            }
        }

        #endregion

        public void Dispose()
        {
            Database?.Dispose();
        }
    }
}