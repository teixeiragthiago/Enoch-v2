using Enoch.CrossCutting;
using Enoch.Domain.Base;
using Enoch.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Enoch.Infra.Base
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly DataContext Context;

        protected BaseRepository(DataContext context)
        {
            Context = context;
        }

        public int Post(TEntity item)
        {
            Context.Set<TEntity>().Add(item);
            Context.SaveChanges();

            var idProperty = item.GetType().GetProperty("Id")?.GetValue(item, null);

            if (!(Convert.ChangeType(idProperty, typeof(int)) is int intTried))
                return 0;

            return (int)intTried;
        }

        public void Put(TEntity item)
        {
            Context.Set<TEntity>().Attach(item);
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public TEntity GetById(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>().FirstOrDefault(filter);
        }

        public TEntity First(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>().FirstOrDefault(filter);
        }

        public TEntity First(Expression<Func<TEntity, bool>> filter, params string[] include)
        {
            var query = Context.Set<TEntity>().AsQueryable();

            query = include.Aggregate(query, (current, item) => current.Include(item).AsQueryable());

            return query.FirstOrDefault(filter);
        }

        public TEntity GetById(Expression<Func<TEntity, bool>> filter, params string[] include)
        {
            var query = Context.Set<TEntity>().AsQueryable();

            query = include.Aggregate(query, (current, item) => current.Include(item).AsQueryable());

            return query.FirstOrDefault(filter);
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>().Where(filter).ToList();
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, params string[] include)
        {
            var query = Context.Set<TEntity>().Where(filter);

            query = include.Aggregate(query, (current, item) => current.Include(item).AsQueryable());

            return query.ToList();
        }

        public IEnumerable<TEntity> GetNoTracked(Expression<Func<TEntity, bool>> filter, params string[] include)
        {
            var query = Context.Set<TEntity>().Where(filter);

            query = include.Aggregate(query, (current, item) => current.Include(item).AsNoTracking().AsQueryable());

            return query.ToList();
        }


        public IEnumerable<TEntity> Get(out int total, int? page, Expression<Func<TEntity, bool>> filter,
            string sortBy = null, string sortDirection = null)
        {
            page = page == 0 ? 1 : page;

            return Context.Set<TEntity>().Where(filter).Order(sortBy, sortDirection).Paginate(page, out total).ToList();
        }

        public IEnumerable<TEntity> Get(out int total, int? page, string sortBy = null, string sortDirection = null, params string[] include)
        {
            page = page == 0 ? 1 : page;

            var query = Context.Set<TEntity>().AsQueryable();

            query = include.Aggregate(query, (current, item) => current.Include(item).AsNoTracking().AsQueryable());

            return query.Paginate(page, out total).ToList();
        }

        public IEnumerable<TEntity> Get(out int total, int? page, params string[] include)
        {
            page = page == 0 ? 1 : page;

            var query = Context.Set<TEntity>().AsQueryable();

            query = include.Aggregate(query, (current, item) => current.Include(item).AsNoTracking().AsQueryable());

            return query.Paginate(page, out total).ToList();
        }

        public IEnumerable<TEntity> Get(out int total, int? page, Expression<Func<TEntity, bool>> filter, params string[] include)
        {
            page = page == 0 ? 1 : page;
            var query = Context.Set<TEntity>().AsQueryable();

            query = include.Aggregate(query, (current, item) => current.Include(item).AsNoTracking().AsQueryable());

            return query.Where(filter).Paginate(page, out total).ToList();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> GetAll(params string[] include)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            query = include.Aggregate(query, (current, item) => current.Include(item).AsNoTracking().AsQueryable());
            return query.ToList();
        }

        public void Delete(TEntity item)
        {
            Context.Set<TEntity>().Remove(item);
            Context.SaveChanges();
        }

        public bool Any(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>().Any(filter);
        }

        public int Count(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>().Count(filter);
        }

        public Expression<Func<TEntity, bool>> CreateFilter(Expression<Func<TEntity, bool>> filter) => filter;
    }


}
