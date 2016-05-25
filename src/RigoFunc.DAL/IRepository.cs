// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RigoFunc.DAL {
    /// <summary>
    /// Defines the interface(s) for generic repository.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class {
        /// <summary>
        /// Filters a sequence of values based on a predicate. This method is no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="IQueryable{T}"/> that contains elements that satisfy the condition specified by predicate.</returns>
        /// <remarks>This method is no-tracking query.</remarks>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Filters a sequence of values based on a predicate. This method will change tracking by context.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="IQueryable{T}"/> that contains elements that satisfy the condition specified by predicate.</returns>
        /// <remarks>This method will change tracking by context.</remarks>
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Inserts a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous insert operation. The task result contains the inserted entity.</returns>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous insert operation. The task result contatins the inserted entities.</returns>
        Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates the specified entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous update operation. The task result contains the updated entity.</returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Deletes the entity by the specified primary key.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        void Delete(object id);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(TEntity entity);
    }
}
