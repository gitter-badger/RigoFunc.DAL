// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace RigoFunc.DAL.Internal {
    /// <summary>
    /// Represents a default generic repository implements the <see cref="IRepository{TEntity}"/> interface.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    internal class Repository<TEntity> : IRepository<TEntity> where TEntity: class {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public Repository(DbContext dbContext) {
            if(dbContext == null) {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate. This method is no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that contains elements that satisfy the condition specified by predicate.</returns>
        /// <remarks>This method is no-tracking query.</remarks>
        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate) => _dbSet.AsNoTracking().Where(predicate);

        /// <summary>
        /// Filters a sequence of values based on a predicate. This method will change tracking by context.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that contains elements that satisfy the condition specified by predicate.</returns>
        /// <remarks>This method will change tracking by context.</remarks>
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate) => _dbSet.Where(predicate);

        /// <summary>
        /// Inserts a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous insert operation. The task result contains the inserted entity.</returns>
        public Task<TEntity> InsertAsync(TEntity entity) {
            _dbSet.Add(entity);

            return Task.FromResult(entity);
        }

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous insert operation. The task result contatins the inserted entities.</returns>
        public Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities) {
            _dbSet.AddRange(entities);

            return Task.FromResult(entities);
        }

        /// <summary>
        /// Updates the specified entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous update operation. The task result contains the updated entity.</returns>
        public Task UpdateAsync(TEntity entity) {
            _dbSet.Update(entity);

            return Task.FromResult(entity);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Delete(TEntity entity) {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Deletes the entity by the specified primary key.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Delete(object id) {
            // using a stub entity to mark for deletion
            var typeInfo = typeof(TEntity).GetTypeInfo();
            var property = typeInfo.GetProperty("Id");
            if (property != null) {
                var entity = Activator.CreateInstance<TEntity>();
                property.SetValue(entity, id);
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
            else {
                //var entity = _dbSet.Find(id);
                //if (entity != null) {
                //    Delete(entity);
                //}
            }
        }
    }
}
