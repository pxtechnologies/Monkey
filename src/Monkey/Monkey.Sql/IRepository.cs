using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monkey.Sql
{
    public interface IRepository
    {
        IQueryable<TEntity> Query<TEntity>() where TEntity:class;
        Task Add<TEntity>(TEntity entity) where TEntity : class;
        Task Update<TEntity>(TEntity entity) where TEntity : class;
        Task Remove<TEntity>(TEntity entity) where TEntity : class;
        Task CommitChanges();
    }
}
