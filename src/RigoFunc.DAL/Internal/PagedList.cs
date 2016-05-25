// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace RigoFunc.DAL.Internal {
    internal class PagedList<T> : IPagedList<T> {
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }

        public IList<T> Items { get; }

        public bool HasPreviousPage => (PageIndex > 0);

        public bool HasNextPage => (PageIndex + 1 < TotalPages);

        public PagedList(IOrderedQueryable<T> source, int pageIndex, int pageSize) {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            Items = source.Skip(PageIndex * PageSize).Take(PageSize).ToList();
        }
    }

    internal class PagedList<TInput, TOutput> : IPagedList<TOutput> {
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }

        public IList<TOutput> Items { get; }

        public bool HasPreviousPage => (PageIndex > 0);

        public bool HasNextPage => (PageIndex + 1 < TotalPages);

        public PagedList(IOrderedQueryable<TInput> source, Func<IEnumerable<TInput>, IEnumerable<TOutput>> converter, int pageIndex, int pageSize) {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            var items = source.Skip(PageIndex * PageSize).Take(PageSize).ToArray();

            Items = new List<TOutput>(converter(items));
        }
    }
}
