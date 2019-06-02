using System;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionForEF
{
    public class QueryOptions<T> where T : class, new()
    {
        public Expression<Func<T, bool>> ConditionExpression { get; set; } = _ => true;
        public Func<IQueryable<T>, IQueryable<T>> OrderExpression { get; set; } = _ => _;
        public Func<IQueryable<T>, IQueryable<T>> TopExpression { get; set; } = _ => _;
    }
}