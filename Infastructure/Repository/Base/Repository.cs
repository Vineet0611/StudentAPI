using Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DataContext _datacontext;
        public Repository(DataContext dataContext)
        {
            _datacontext = dataContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _datacontext.Set<T>().AddAsync(entity);
            return entity;
        }
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _datacontext.Set<T>().AddRangeAsync(entities);
            return entities;
        }
        public async Task DeleteAsync(T entity)
        {
            _datacontext.Set<T>().Remove(entity);

        }

        public IEnumerable<T> ExecuteQuery(string query, DbParameter[] dbParams)
        {
            return _datacontext.Set<T>().FromSqlRaw(query, dbParams).AsEnumerable();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _datacontext.Set<T>().ToListAsync();
        }

        public IQueryable<T> GetAllQuery()
        {
            return _datacontext.Set<T>().AsQueryable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _datacontext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdAsync(params object[] keyvalue)
        {
            return await _datacontext.Set<T>().FindAsync(keyvalue);
        }

        public async Task<int> UpdateAsync(T entity)
        {
            _datacontext.Set<T>().Attach(entity);
            return 0;
        }
    }
}
