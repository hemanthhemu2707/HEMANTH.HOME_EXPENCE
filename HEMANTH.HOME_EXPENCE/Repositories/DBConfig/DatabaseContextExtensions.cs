using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.EFCore.Repositories
{
	public static class DatabaseContextExtensions
	{
		/// <summary>
		/// Performs straight insert without checking if key already exist
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dbContext"></param>
		/// <param name="databaseEntity"></param>
		public static T Insert<T>(this DbContext dbContext, T databaseEntity) where T : class
		{
			dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
			dbContext.Entry(databaseEntity).State = EntityState.Added;
			dbContext.SaveChanges();
			dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
			return databaseEntity;
		}

		/// <summary>
		/// Performs straight insert without checking if key already exist
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dbContext"></param>
		/// <param name="databaseEntity"></param>
		public static async Task<T> InsertAsync<T>(this DbContext dbContext, T databaseEntity) where T : class
		{
			dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
			dbContext.Entry(databaseEntity).State = EntityState.Added;

			await dbContext.SaveChangesAsync();

			dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
			return databaseEntity;
		}

		/// <summary>
		/// Generic method to update the EF entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dbContext"></param>
		/// <param name="databaseEntity"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static async Task<T> UpdateAsync<T>(this DbContext dbContext, T databaseEntity) where T : class
		{
			var entry = dbContext.Entry(databaseEntity);
			if (entry.State == EntityState.Detached)
			{
				throw new ArgumentException();
			}
			entry.State = EntityState.Modified;
			await dbContext.SaveChangesAsync();
			return databaseEntity;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dbContext"></param>
		/// <param name="databaseEntities"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static async Task<IEnumerable<T>> UpdateArrayAsync<T>(this DbContext dbContext, params T[] databaseEntities) where T : class
		{
			try
			{
				dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
				var updatedEntities = new List<T>();
				foreach (var entity in databaseEntities)
				{
					var entry = dbContext.Entry(entity);
					if (entry.State == EntityState.Detached)
					{
						throw new ArgumentException();
					}
					entry.State = EntityState.Modified;
					updatedEntities.Add(entity);
				}
				await dbContext.SaveChangesAsync();
				dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
				return updatedEntities;
			}
			catch (Exception ex)
			{

			}
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dbContext"></param>
		/// <param name="databaseEntities"></param>
		public static async Task<IEnumerable<T>> InsertArrayAsync<T>(this DbContext dbContext, params T[] databaseEntities) where T : class
		{
			dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
			var insertedEntities = new List<T>();
			foreach (var entity in databaseEntities)
			{
				var entry = dbContext.Entry(entity);
				if (entry.State != EntityState.Detached)
				{
					throw new ArgumentException();
				}
				entry.State = EntityState.Added;
				insertedEntities.Add(entity);
			}

			await dbContext.SaveChangesAsync();
			dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
			return insertedEntities;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dbContext"></param>
		/// <param name="databaseEntity"></param>
		/// <returns></returns>
		public static async Task<T> SaveAsync<T>(this DbContext dbContext, T databaseEntity) where T : class
		{
			var entry = dbContext.Entry(databaseEntity);
			if (entry.State == EntityState.Detached)
			{
				var primaryKey = dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey();
				var keyValues = primaryKey.Properties.Select(x => entry.Property(x.Name).CurrentValue).ToArray();
				var existingEntity = await dbContext.FindAsync<T>(keyValues);
				if (existingEntity != null)
				{
					entry = dbContext.Entry(existingEntity);
					entry.CurrentValues.SetValues(databaseEntity);
				}
				else
				{
					dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
					dbContext.Entry(databaseEntity).State = EntityState.Added;
					await dbContext.SaveChangesAsync();
					dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
				}
			}
			else
				entry.State = EntityState.Modified;
			await dbContext.SaveChangesAsync();
			return databaseEntity;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dbContext"></param>
		/// <param name="databaseEntities"></param>
		/// <returns></returns>
		public static async Task<IEnumerable<T>> SaveArrayAsync<T>(this DbContext dbContext, params T[] databaseEntities) where T : class
		{
			dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
			var updatedEntities = new List<T>();
			foreach (var entity in databaseEntities)
			{
				var entry = dbContext.Entry(entity);
				if (entry.State == EntityState.Detached)
				{
					var primaryKey = dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey();
					var keyValues = primaryKey.Properties.Select(x => entry.Property(x.Name).CurrentValue).ToArray();
					var existingEntity = await dbContext.FindAsync<T>(keyValues);
					if (existingEntity != null)
					{
						entry = dbContext.Entry(existingEntity);
						entry.CurrentValues.SetValues(entity);
					}
					else
					{
						dbContext.Entry(entity).State = EntityState.Added;
						updatedEntities.Add(entity);
					}
				}
				else
				{
					entry.State = EntityState.Modified;
					updatedEntities.Add(entity);
				}
			}
			await dbContext.SaveChangesAsync();
			dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
			return updatedEntities;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dbContext"></param>
		/// <param name="databaseEntity"></param>
		/// <returns></returns>
		public static async Task DeleteAsync<T>(this DbContext dbContext, T databaseEntity) where T : class
		{
			if (databaseEntity != null)
			{
				dbContext.Set<T>().Remove(databaseEntity);
				await dbContext.SaveChangesAsync();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dbContext"></param>
		/// <param name="includeProperties"></param>
		/// <returns></returns>
		public static async Task<IEnumerable<T>> GetAllAsync<T>(this DbContext dbContext,
																params Expression<Func<T, object>>[] includeProperties)
																where T : class
		{
			IQueryable<T> query = dbContext.Set<T>();
			if (includeProperties != null)
			{
				foreach (var includeProperty in includeProperties)
					query = query.Include(includeProperty);
			}
			return await query.ToListAsync();
		}

		public static async Task<IEnumerable<T>> GetFilteredCollectionAsync<T>(this DbContext dbContext,
																			   Expression<Func<T, bool>> filter,
																			   params Expression<Func<T, object>>[] includeProperties)
																			   where T : class
		{
			IQueryable<T> query = dbContext.Set<T>();
			if (includeProperties != null)
				foreach (var includeProperty in includeProperties)
					query = query.Include(includeProperty);
			if (filter != null)
				query = query.Where(filter);
			return await query.ToListAsync();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dbContext"></param>
		/// <param name="filter"></param>
		/// <param name="includeProperties"></param>
		/// <returns></returns>
		public static async Task<T> GetSingleAsync<T>(this DbContext dbContext, Expression<Func<T, bool>> filter = null,
											 params Expression<Func<T, object>>[] includeProperties)
											 where T : class
		{
			IQueryable<T> query = dbContext.Set<T>();
			if (includeProperties != null)
			{
				foreach (var includeProperty in includeProperties)
					query = query.Include(includeProperty);
			}
			if (filter != null)
				query = query.Where(filter);
			return await query.SingleOrDefaultAsync();
		}

		public static async Task<IQueryable<T>> IncludeMultipleAsync<T, TProperty>(
												this Task<IQueryable<T>> queryTask,
												params Expression<Func<T, IEnumerable<TProperty>>>[] includes) where T : class
		{
			var query = await queryTask;
			if (includes == null || includes.Length == 0)
				return query;
			foreach (var include in includes)
				query = query.Include(include);
			return query;
		}
	}

}
