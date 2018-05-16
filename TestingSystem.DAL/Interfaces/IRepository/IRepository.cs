using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestingSystem.DAL.Interfaces.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetById(object id);
        Task<T> GetByIdAsync(object id);
        T GetSingle(Expression<Func<T, bool>> whereCondition);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> whereCondition);
        List<T> GetAll();
        Task<List<T>> GetAllAsync();
        List<T> GetAll(Expression<Func<T, bool>> whereCondition);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> whereCondition);
        IQueryable<T> GetQueryable();
        void Create(T item);
        void Update(T item);
        void Delete(T item);
    }
}