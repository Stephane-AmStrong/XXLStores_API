using Application.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public IQueryable<T> BaseFindAll()
        {
            return RepositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> BaseFindByCondition(Expression<Func<T, bool>> expression)
        {
            return RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        public async Task BaseCreateAsync(T entity)
        {
            await RepositoryContext.Set<T>().AddAsync(entity);
        }

        public async Task BaseCreateAsync(IEnumerable<T> entities)
        {
            await RepositoryContext.Set<T>().AddRangeAsync(entities);
        }

        public async Task BaseUpdateAsync(T entity)
        {
            await Task.Run(() => RepositoryContext.Set<T>().Update(entity));
        }

        public async Task BaseUpdateAsync(IEnumerable<T> entities)
        {
            await Task.Run(() => RepositoryContext.Set<T>().UpdateRange(entities));
        }

        public async Task BaseDeleteAsync(T entity)
        {
            await Task.Run(() => RepositoryContext.Set<T>().Remove(entity));
        }
    }
}
