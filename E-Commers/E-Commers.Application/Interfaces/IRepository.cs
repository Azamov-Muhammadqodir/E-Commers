using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commers.Application.Interfaces
{
    public interface IRepository<T>
    {
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T,bool>>? expression = null);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid Id);
        Task<T> CreateAsync(T entity);
    }
}
