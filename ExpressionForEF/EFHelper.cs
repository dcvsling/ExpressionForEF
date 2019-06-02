using System.Linq;
#if NET
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
#elif NETSTANDARD
 using Microsoft.EntityFrameworkCore;
#endif
using System;

namespace ExpressionForEF
{
    public static class EFHelper
    {
        public static IQueryable<TOutput> CreateQuery<T, TOutput>(this DbContext context, PostActionQueryOptions<T, TOutput> options = default)
           where T : class, new()
           => options.PostActionExpression(context.CreateQuery<T>(options));

        public static IQueryable<T> CreateQuery<T>(this DbContext context, QueryOptions<T> options = default)
            where T : class, new()
            => context?.Set<T>().ConfigureQuery(options);

        public static IQueryable<T> ConfigureQuery<T>(this IQueryable<T> query, QueryOptions<T> options = default)
            where T : class, new()
            => query is null
                ? throw new ArgumentNullException(nameof(query))
                : query.InternalConfigureQuery(options ?? new QueryOptions<T>());

        private static IQueryable<T> InternalConfigureQuery<T>(this IQueryable<T> query, QueryOptions<T> options = default)
            where T : class, new()
            => query.Where(options.ConditionExpression)
                .ConfigureBy(options.OrderExpression)
                .ConfigureBy(options.TopExpression);

        private static IQueryable<T> ConfigureBy<T>(this IQueryable<T> query, Func<IQueryable<T>, IQueryable<T>> config)
            => config?.Invoke(query) ?? query;


    }
}