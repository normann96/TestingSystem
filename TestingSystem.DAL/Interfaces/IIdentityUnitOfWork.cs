using System;
using System.Threading.Tasks;
using TestingSystem.DAL.Interfaces.IRepository;

namespace TestingSystem.DAL.Interfaces
{
    public interface IIdentityUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }

        Task SaveAsync();
    }
}