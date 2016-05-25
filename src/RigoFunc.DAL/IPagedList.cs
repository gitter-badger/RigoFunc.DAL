// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

using RigoFunc.DAL.Internal;

namespace RigoFunc.DAL {
    /// <summary>
    /// Provides the interface(s) for paged list of any type.
    /// </summary>
    /// <typeparam name="T">The type for paging.</typeparam>
    public interface IPagedList<T> {
        /// <summary>
        /// Gets the page index (current).
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// Gets the page size.
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// Gets the total count of the list of type <typeparamref name="T"/>
        /// </summary>
        int TotalCount { get; }
        /// <summary>
        /// Gets the total pages.
        /// </summary>
        int TotalPages { get; }
        /// <summary>
        /// Gets the current page items.
        /// </summary>
        IList<T> Items { get; }
    }

    /// <summary>
    /// Provides some extension methods for <see cref="IOrderedQueryable{T}"/> to provide paging capability.
    /// </summary>
    public static class IOrderedQueryableExtensions {
        /// <summary>
        /// Converts the specified source to <see cref="IPagedList{T}"/> by the specified <paramref name="pageIndex"/> and <paramref name="pageSize"/>.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to paging.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>An instance of the inherited from <see cref="IPagedList{T}"/> interface.</returns>
        public static IPagedList<T> ToPagedList<T>(this IOrderedQueryable<T> source, int pageIndex, int pageSize) => new PagedList<T>(source, pageIndex, pageSize);

        /// <summary>
        /// Converts the specified source to <see cref="IPagedList{T}"/> by the specified <paramref name="converter"/>, <paramref name="pageIndex"/> and <paramref name="pageSize"/>
        /// </summary>
        /// <typeparam name="TInput">The type of the source.</typeparam>
        /// <typeparam name="TOutput">The type of the output data</typeparam>
        /// <param name="source">The source to convert.</param>
        /// <param name="converter">The converter to change the <typeparamref name="TInput"/> to <typeparamref name="TOutput"/>.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>An instance of the inherited from <see cref="IPagedList{T}"/> interface.8</returns>
        public static IPagedList<TOutput> ToPagedList<TInput, TOutput>(this IOrderedQueryable<TInput> source, Func<IEnumerable<TInput>, IEnumerable<TOutput>> converter, int pageIndex, int pageSize) => new PagedList<TInput, TOutput>(source, converter, pageIndex, pageSize);
    }
}
