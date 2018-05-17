using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestingSystem.DAL.EF;
using TestingSystem.DAL.Interfaces.IRepository;

namespace TestingSystem.DAL.Repositories.Base
{
    public abstract class BaseRepository<T> : IRepository<T>, IDisposable where T : class
    {
        protected DbSet<T> Set => Db.Set<T>();

        protected AppIdentityDbContext Db { get; }

        protected BaseRepository(AppIdentityDbContext context)
        {
            Db = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region IRepository Members
        public T GetById(object id)
        {
            return Set.Find(id);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await Set.FindAsync(id);
        }

        public T GetSingle(Expression<Func<T, bool>> whereCondition)
        {
            return Set.FirstOrDefault(whereCondition);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await Set.FirstOrDefaultAsync(whereCondition);
        }

        public List<T> GetAll()
        {
            return Set.ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await Set.ToListAsync();
        }

        public List<T> GetAll(Expression<Func<T, bool>> whereCondition)
        {
            return Set.Where(whereCondition).ToList();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await Set.Where(whereCondition).ToListAsync();
        }

        public IQueryable<T> GetQueryable()
        {
            return Set.AsQueryable();
        }

        public T Create(T item)
        {
            return Set.Add(item);
        }

        public void Update(T item)
        {
            Set.AddOrUpdate(item);
        }

        public T Delete(T item)
        {
            return Set.Remove(item);
        }
        #endregion

        #region Dispose pattern
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}