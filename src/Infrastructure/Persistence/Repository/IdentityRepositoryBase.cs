using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class IdentityRepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected IdentityContext IdentityContext { get; set; }


        public IdentityRepositoryBase(IdentityContext identityContext)
        {
            IdentityContext = identityContext;
        }

        public IQueryable<T> BaseFindAll()
        {
            return IdentityContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> BaseFindByCondition(Expression<Func<T, bool>> expression)
        {
            return IdentityContext.Set<T>().Where(expression).AsNoTracking();
        }

        public async Task BaseCreateAsync(T entity)
        {
            await IdentityContext.Set<T>().AddAsync(entity);
        }

        public async Task BaseCreateAsync(IEnumerable<T> entities)
        {
            await IdentityContext.Set<T>().AddRangeAsync(entities);
        }

        public async Task BaseUpdateAsync(T entity)
        {
            await Task.Run(() => IdentityContext.Set<T>().Update(entity));
        }

        public async Task BaseUpdateAsync(IEnumerable<T> entities)
        {
            await Task.Run(() => IdentityContext.Set<T>().UpdateRange(entities));
        }

        public async Task BaseDeleteAsync(T entity)
        {
            await Task.Run(() => IdentityContext.Set<T>().Remove(entity));
        }

        public async Task BaseSaveAsync()
        {
            await IdentityContext.SaveChangesAsync();
        }
    }
}
