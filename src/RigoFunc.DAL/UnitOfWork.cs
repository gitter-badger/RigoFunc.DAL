// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using RigoFunc.DAL.Extensions;
using RigoFunc.DAL.Internal;

namespace RigoFunc.DAL {
    /// <summary>
    /// Represents the default implementation of the <see cref="IUnitOfWork"/> interface.
    /// </summary>
    public class UnitOfWork : IUnitOfWork {
        private readonly DbContext _context;
        private bool disposed = false;
        private Dictionary<Type, object> repositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UnitOfWork(DbContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }
            _context = context;
        }

        /// <summary>
        /// Gets the specified type context.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <returns>An instance of type <typeparamref name="TContext" /> or null if the inner context isn't compatibility with <typeparamref name="TContext" />.</returns>
        public TContext Context<TContext>() where TContext : DbContext => _context as TContext;

        /// <summary>
        /// Gets the specified repository for the <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class {
            if (repositories == null) {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type)) {
                repositories[type] = new Repository<TEntity>(_context);
            }

            return (IRepository<TEntity>)repositories[type];
        }

        /// <summary>
        /// Uses raw SQL queries to fetch the specified <typeparamref name="TEntity" /> data.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>An <see cref="IQueryable{T}" /> that contains elements that satisfy the condition specified by raw SQL.</returns>
        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class => _context.Set<TEntity>().FromSql(sql, parameters);

        /// <summary>
        /// Asynchronously saves all changes made in this unit of work to the database.
        /// </summary>
        /// <param name="withHistory"><c>True</c> if save changes with auto record the change history.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
        public async Task<int> SaveChangesAsync(bool withHistory = false) {
            if (withHistory) {
                return await _context.SaveChangesWithHistoryAsync();
            }

            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    _context.Dispose();
                }
            }
            disposed = true;
        }
    }
}
