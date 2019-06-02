using System;
using System.Linq;

namespace ExpressionForEF
{
    public class PostActionQueryOptions<T, TOutput> : QueryOptions<T>
        where T : class, new()
    {
        internal PostActionQueryOptions(QueryOptions<T> options)
        {
            this.ConditionExpression = options.ConditionExpression;
            this.OrderExpression = options.OrderExpression;
            this.TopExpression = options.TopExpression;
        }
        public Func<IQueryable<T>, IQueryable<TOutput>> PostActionExpression { get; set; }
    }
}