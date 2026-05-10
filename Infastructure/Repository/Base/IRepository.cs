using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repository.Base
{
    public interface IRepository<T> where T : class
    {

        Task<IReadOnlyList<T>> GetAllAsync();

        IQueryable<T> GetAllQuery();

        Task<T> GetByIdAsync(int id);

        Task<T> GetByIdAsync(params object[] keyvalue);

        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        Task<int> UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        IEnumerable<T> ExecuteQuery(string query, DbParameter[] dbParams);

    }
}
