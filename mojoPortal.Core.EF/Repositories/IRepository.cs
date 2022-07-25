using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace mojoPortal.Core.EF.Repositories
{
	public interface IRepository<TEntity> where TEntity : class
	{
		Task<TEntity> GetAsync(int id);
		Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
		Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

		Task<IEnumerable<TEntity>> GetAllAsync();
		Task<(IEnumerable<TEntity>, int)> GetPageAsync(int pageIndex, int pageSize);
		Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

		void Add(TEntity entity);
		void AddRange(IEnumerable<TEntity> entities);

		void Remove(TEntity entity);
		Task Remove(int entityId);
		void RemoveRange(IEnumerable<TEntity> entities);
	}
}
