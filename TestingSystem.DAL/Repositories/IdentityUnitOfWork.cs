using System;
using System.Threading.Tasks;
using TestingSystem.DAL.EF;
using TestingSystem.DAL.Interfaces;
using TestingSystem.DAL.Interfaces.IRepository;

namespace TestingSystem.DAL.Repositories
{
    public class IdentityUnitOfWork : IIdentityUnitOfWork
    {
        private readonly AppIdentityDbContext _db;

        public IdentityUnitOfWork(AppIdentityDbContext db, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            RoleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        #region IIdentityUnitOfWork Members
        public IUserRepository UserRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
        #endregion

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}