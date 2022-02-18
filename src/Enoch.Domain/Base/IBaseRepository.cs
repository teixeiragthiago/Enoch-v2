using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Enoch.Domain.Base
{
    public interface IBaseRepository<T> where T : class
    {
        T GetById(Expression<Func<T, bool>> filter);
        T GetById(Expression<Func<T, bool>> filter, params string[] include);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter);

        IEnumerable<T> Get(out int total, int? page, Expression<Func<T, bool>> filter,
            string sortBy = null, string sortDirection = null);

        IEnumerable<T> Get(out int total, int? page, params string[] include);

        IEnumerable<T> Get(Expression<Func<T, bool>> filter, params string[] include);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(params string[] include);
        IEnumerable<T> GetNoTracked(Expression<Func<T, bool>> filter, params string[] include);
        IEnumerable<T> Get(out int total, int? page, string sortBy = null, string sortDirection = null, params string[] include);
        IEnumerable<T> Get(out int total, int? page, Expression<Func<T, bool>> filter,
            params string[] include);

        int Post(T item);
        void Delete(T item);
        void Put(T item);
        bool Any(Expression<Func<T, bool>> filter);
        Expression<Func<T, bool>> CreateFilter(Expression<Func<T, bool>> filter);
        T First(Expression<Func<T, bool>> filter);
        int Count(Expression<Func<T, bool>> filter);
        T First(Expression<Func<T, bool>> filter, params string[] include);
    }
}
