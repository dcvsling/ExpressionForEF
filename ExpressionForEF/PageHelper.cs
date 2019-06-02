using System;
using System.Linq;

namespace ExpressionForEF
{

    internal static class PageHelper
    {
        public static QueryOptions<T> ConfigurePaging<T>(this QueryOptions<T> queryOptions, PagingOptions pageOptions)
            where T : class, new()
        {
            queryOptions.TopExpression = query => new[] {
                SKip<T>((pageOptions.PageNum - 1) * pageOptions.CountPerPage),
                Take<T>(pageOptions.CountPerPage)
                }.Aggregate(query, (seed, next) => next(seed));

            queryOptions.OrderExpression = pageOptions.Orders.GenerateOrderByExpression<T>();
            return queryOptions;
        }

        private static Func<IQueryable<T>, IQueryable<T>> Take<T>(int count)
            => query => query.Take(count < 0 ? 0 : count);

        private static Func<IQueryable<T>, IQueryable<T>> SKip<T>(int count)
            => query => query.Skip(count < 0 ? 0 : count);
    }
}