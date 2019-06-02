using System;
using System.Linq;

namespace ExpressionForEF
{
    public static class OptionsHelper
    {
        public static QueryOptions<T> CreateOptions<T>(this QueryValues queryValues)
            where T : class, new()
            => new QueryOptions<T>()
                .ConfigureCondition(queryValues)
                .ConfigurePaging(queryValues.Paging);

        public static PostActionQueryOptions<T, TOutput> ConfigurePostAction<T, TOutput>(
            this QueryOptions<T> options,
            Func<IQueryable<T>, IQueryable<TOutput>> postAction)
            where T : class, new()
            => new PostActionQueryOptions<T, TOutput>(options) { PostActionExpression = postAction };

    }
}