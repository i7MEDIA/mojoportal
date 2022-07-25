using mojoPortal.Core.EF.Domain;
using mojoPortal.Core.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace mojoPortal.Data.EF.Repositories
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
	{
		protected readonly DbContext Context;
		private readonly DbSet<TEntity> entities;


		public Repository(DbContext context)
		{
			Context = context;
			entities = Context.Set<TEntity>();
		}


		/// <summary>
		/// Get entity by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<TEntity> GetAsync(int id)
		{
			// Here we are working with a DbContext, not mojoportalDbContext, and so we don't have DbSets 
			// such as BannedIPAddresses, and we need to use the generic Set() method to access them.
			return await entities.FindAsync(id);
		}


		/// <summary>
		/// Returns a single, specific entity, if one or more exist, or a default value if no such entity is found.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns>TEntity</returns>
		public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await entities.FirstOrDefaultAsync(predicate);
		}


		/// <summary>
		/// Returns a single, specific entity, or a default value if no such entity is found. Will throw error if more than one Entry exists.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns>TEntity</returns>
		public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await entities.SingleOrDefaultAsync(predicate);
		}


		/// <summary>
		/// Get all entities in table
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			return await entities.ToListAsync();
		}


		public async Task<(IEnumerable<TEntity>, int)> GetPageAsync(int pageIndex, int pageSize)
		{
			var results = await entities.OrderBy(b => b.ID)
				.Select(e => e)
				.Skip((pageIndex - 1) * pageSize)
				.Take(pageSize)
				.GroupBy(e => new 
				{
					Total = entities.Count()
				}).ToListAsync();


			var totalCount = results[0].Key.Total;
			var entityList = results[0].Select(e => e).ToList();

			pageSize = Math.Max(pageSize, 1);

			var totalPages = totalCount / pageSize;

			if (totalCount % pageSize > 0)
			{
				totalPages++;
			}

			return (entityList, totalPages);
		}


		/// <summary>
		/// Find entity by predicate
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await entities.Where(predicate).ToListAsync();
		}


		/// <summary>
		/// Add entity
		/// </summary>
		/// <param name="entity"></param>
		public void Add(TEntity entity)
		{
			_ = entities.Add(entity);
		}


		/// <summary>
		/// Add collection of entities
		/// </summary>
		/// <param name="entities"></param>
		public void AddRange(IEnumerable<TEntity> entities)
		{
			_ = this.entities.AddRange(entities);
		}


		/// <summary>
		/// Remove by entity
		/// </summary>
		/// <param name="entity"></param>
		public void Remove(TEntity entity)
		{
			_ = entities.Remove(entity);
		}


		/// <summary>
		/// Remove by entity ID
		/// </summary>
		/// <param name="entityId"></param>
		public async Task Remove(int entityId)
		{
			_ = entities.Remove(await GetAsync(entityId));
		}


		/// <summary>
		/// Remove collection of entities
		/// </summary>
		/// <param name="entities"></param>
		public void RemoveRange(IEnumerable<TEntity> entities)
		{
			_ = this.entities.RemoveRange(entities);
		}
	}
}
