using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionForEF
{
    internal static class OrderByHelper
    {
        public static Func<IQueryable<T>, IQueryable<T>> GenerateOrderByExpression<T>(this IEnumerable<OrderOptions> orders)
            where T : class, new()
            => orders.Any()
                ? query => CreateOrderBy<T>(orders.First())
                    .Invoke(query)
                    .AppendThenByExpression(
                        orders.Skip(1)
                            .GenerateThenByExpression<T>())
                : (Func<IQueryable<T>, IQueryable<T>>)(_ => _);

        private static IOrderedQueryable<T> AppendThenByExpression<T>(
            this IOrderedQueryable<T> query,
            IEnumerable<Func<IOrderedQueryable<T>, IOrderedQueryable<T>>> thenbys)
            => thenbys.Aggregate(query, (seed, next) => next(seed));

        private static IEnumerable<Func<IOrderedQueryable<T>, IOrderedQueryable<T>>> GenerateThenByExpression<T>(this IEnumerable<OrderOptions> orders)
            where T : class, new()
            => orders.Select(AppendThenBy<T>);


        private static Func<IQueryable<T>, IOrderedQueryable<T>> CreateOrderBy<T>(OrderOptions order)
            where T : class, new()
            => order.IsDescending
                ? CreateOrderByDescending<T>(order.OrderColumn)
                : CreateOrderBy<T>(order.OrderColumn);

        private static Func<IQueryable<T>, IOrderedQueryable<T>> CreateOrderBy<T>(string name)
            where T : class, new()
            => query => query.OrderBy(name.CreatePropertyAccessor<T>());

        private static Func<IQueryable<T>, IOrderedQueryable<T>> CreateOrderByDescending<T>(string name)
            where T : class, new()
            => query => query.OrderByDescending(name.CreatePropertyAccessor<T>());

        private static Func<IOrderedQueryable<T>, IOrderedQueryable<T>> AppendThenBy<T>(OrderOptions order)
            where T : class, new()
            => order.IsDescending
                ? AppendThenByDescending<T>(order.OrderColumn)
                : AppendThenBy<T>(order.OrderColumn);

        private static Func<IOrderedQueryable<T>, IOrderedQueryable<T>> AppendThenBy<T>(string name)
            where T : class, new()
            => query => query.ThenBy(name.CreatePropertyAccessor<T>());

        private static Func<IOrderedQueryable<T>, IOrderedQueryable<T>> AppendThenByDescending<T>(string name)
            where T : class, new()
            => query => query.ThenByDescending(name.CreatePropertyAccessor<T>());
    }
}