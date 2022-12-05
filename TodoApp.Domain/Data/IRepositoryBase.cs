using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Data
{
    public interface IRepositoryBase<T> where T : BaseEntity
    {
        Task<T> Add(T T);
        Task<T> Update(T T);
        Task Remove(Guid id);
        Task<T> Get(Guid id);
        Task<List<T>> List(Expression<Func<T, bool>> predicate = null);
        IQueryable<T> AsQueryable();
    }
}
