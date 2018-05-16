using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TestingSystem.BLL.EntitiesDto;


namespace TestingSystem.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<UserDto> FindAsync(string userName, string password);
        Task<UserDto> FindByIdAsync(string userId);
        Task<ClaimsIdentity> CreateIdentityAsync(UserDto user, string authenticationType);
        Task<IdentityResult> CreateAsync(UserDto userDto, string password);
        Task<IdentityResult> UpdateAsync(UserDto user);
        Task<IdentityResult> DeleteAsync(string userId);
        Task<IdentityResult> AddToRoleAsync(string userId, string role);
        Task<IdentityResult> RemoveFromRoleAsync(string userId, string role);
        Task<List<UserDto>> GetAllAsync();
    }
}