using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionForEF
{

    internal static class ConditionHelper
    {
        internal static Expression<Func<T, object>> CreatePropertyAccessor<T>(this string name)
            => Operator.Lambda<T, object>(Operator.Property(name));


        public static QueryOptions<T> ConfigureCondition<T>(this QueryOptions<T> options, QueryValues values)
            where T : class, new()
        {
            options.ConditionExpression = Operator.Lambda<T, bool>(values.Keys.Join(
                typeof(T).GetProperties(),
                key => key,
                prop => prop.Name,
                (key, prop) => GenerateCondition(prop.Name, values[key]))
                .Aggregate(Operator.And));
            return options;
        }

        private static Func<Expression, Expression> GenerateCondition(string name, Range range)
            => range.IsSingleValue
                ? CreateSingleCondition(name, range)
                : Operator.Property(name).Then(Operator.Between(range));

        private static Func<Expression, Expression> CreateSingleCondition(string name, Range range)
            => range.IsExactlyArray
                ? CreateContainsCondition(name, range)
                : CreateEqualCondition(name, range);

        private static Func<Expression, Expression> CreateEqualCondition(string name, Range range)
            => CreateMemberCondition(
                    name, range.From,
                    range.IsExcept
                        ? (Func<Expression, Expression, BinaryExpression>)Expression.NotEqual
                        : Expression.Equal);
        private static Func<Expression, Expression> CreateContainsCondition(string name, Range range)
            => (range.From is IEnumerable seq ? seq.OfType<object>() : Enumerable.Empty<object>())
                .Select(x => CreateEqualCondition(name, new Range(x)))
                .Aggregate(Operator.Or);

        //private static MethodInfo ANY = typeof(Queryable).GetMethods().First(x => x.Name == nameof(Queryable.Any) && x.GetParameters().Skip(1).Any());

        internal static Func<Expression, Expression> CreateMemberCondition(this string name, object value, Func<Expression, Expression, BinaryExpression> condition)
            => exp => condition(Expression.Property(exp, name), Expression.Constant(value));


    }
}