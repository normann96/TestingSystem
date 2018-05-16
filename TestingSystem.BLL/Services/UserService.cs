using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TestingSystem.BLL.EntitiesDto;
using TestingSystem.BLL.Interfaces;
using TestingSystem.Constants;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Interfaces;

namespace TestingSystem.BLL.Services
{
    public class UserService : IUserService
    {
        private IIdentityUnitOfWork Database { get; }

        public UserService(IIdentityUnitOfWork uow)
        {
            Database = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        #region IUserService Members

        public async Task<UserDto> FindAsync(string userName, string password)
        {
            AppUser appUser = await Database.UserRepository.FindAsync(userName, password);

            if (appUser == null)
                return null;

            UserDto userDto = new UserDto
            {
                Id = appUser.Id,
                Email = appUser.Email,
                UserName = appUser.UserName,
                Password = password,
            };

            return userDto;
        }

        public async Task<UserDto> FindByIdAsync(string userId)
        {
            var appUser = await Database.UserRepository.FindByIdAsync(userId);
            var userDto = new UserDto
            {
                Id = appUser.Id,
                Email = appUser.Email,
                UserName = appUser.UserName
            };
            return userDto;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var appusers = await Database.UserRepository.GetAllAsync();
            var userDtos = new List<UserDto>();
            foreach (var appUser in appusers)
            {
                userDtos.Add(new UserDto
                {
                    Id = appUser.Id,
                    Email = appUser.Email,
                    UserName = appUser.UserName,
                    RolesId = appUser.Roles.Select(x => x.RoleId).ToList()
                });
            }
            return userDtos;
        }

        public async Task<ClaimsIdentity> CreateIdentityAsync(UserDto user, string authenticationType)
        {
            var appUser = await Database.UserRepository.FindByIdAsync(user.Id);
            return await Database.UserRepository.CreateIdentityAsync(appUser, authenticationType);
        }

        public async Task<IdentityResult> CreateAsync(UserDto userDto, string password)
        {
            var appUser = await Database.UserRepository.GetSingleAsync(x => x.UserName == userDto.UserName);
            if (appUser != null)
                return IdentityResult.Failed("User with such login already exists", userDto.UserName);

            appUser = new AppUser { Email = userDto.Email, UserName = userDto.UserName };
            var result = await Database.UserRepository.CreateAsync(appUser, password);
            if (result.Errors?.Count() > 0)
                return new IdentityResult(result.Errors.FirstOrDefault());

            if (await Database.RoleRepository.RoleExistsAsync(RoleName.User))
                return await AddToRoleAsync(appUser.Id, RoleName.User);


            result = await Database.RoleRepository.CreateAsync(new AppRole(RoleName.User));
            if (result.Errors?.Count() > 0)
                return new IdentityResult(result.Errors.FirstOrDefault());

            return await AddToRoleAsync(appUser.Id, RoleName.User);
        }

        public async Task<IdentityResult> UpdateAsync(UserDto user)
        {
            var appUser = await Database.UserRepository.FindByIdAsync(user.Id);
            if (appUser == null)
                return IdentityResult.Failed("User not found", user.Id);

            appUser.Email = user.Email;

            return await Database.UserRepository.UpdateAsync(appUser); 
        }

        public async Task<IdentityResult> DeleteAsync(string userId)
        {
            var user = await Database.UserRepository.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed("User not found", userId);

            return await Database.UserRepository.DeleteAsync(user);
        }

        public async Task<IdentityResult> AddToRoleAsync(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
                return IdentityResult.Failed("UserId or role can't be a null or empty");

            if (!await Database.RoleRepository.RoleExistsAsync(role))
                return IdentityResult.Failed("Role not found ", role);

            return await Database.UserRepository.AddToRoleAsync(userId, role);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
                return IdentityResult.Failed("UserId or role can't be a null or empty");

            if (!await Database.RoleRepository.RoleExistsAsync(role))
                return IdentityResult.Failed("Role not found ", role);

            return await Database.UserRepository.RemoveFromRoleAsync(userId, role);
        }

        #endregion

        public void Dispose()
        {
            Database?.Dispose();
        }
    }
}