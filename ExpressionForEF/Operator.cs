using System;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionForEF
{
    public static class Operator
    {
        public static Func<Expression, Expression> Property(string name)
           => exp => Expression.Property(exp, name);

        public static Func<Expression, Expression> ConvertTo(Type type)
           => exp => exp.Type.Equals(type)
            ? exp
            : Expression.Convert(exp, type);

        public static Func<Expression, Expression> MoreThen(object value)
            => exp => Expression.GreaterThan(exp, Expression.Constant(Convert.ChangeType(value, exp.Type)));

        public static Func<Expression, Expression> MoreOrEqual(object value)
            => exp => Expression.GreaterThanOrEqual(exp, Expression.Constant(Convert.ChangeType(value, exp.Type)));

        public static Func<Expression, Expression> LessThen(object value)
            => exp => Expression.LessThan(exp, Expression.Constant(Convert.ChangeType(value, exp.Type)));

        public static Func<Expression, Expression> LessOrEqual(object value)
            => exp => Expression.LessThanOrEqual(exp, Expression.Constant(Convert.ChangeType(value, exp.Type)));

        public static Func<Expression, Expression> Equal(object value)
            => exp => Expression.Equal(exp, Expression.Constant(Convert.ChangeType(value, exp.Type)));

        public static Func<Expression, Expression> NotEqual(object value)
            => exp => Expression.NotEqual(exp, Expression.Constant(Convert.ChangeType(value, exp.Type)));

        public static Func<Expression, Expression> Between(Range range)
            => And(
                range.HasFrom ? MoreOrEqual(range.From) : MoreThen(range.From),
                range.HasTo ? LessOrEqual(range.To) : LessThen(range.To)
            );

        public static Func<Func<Expression, Expression>, Func<Expression, Expression>, Func<Expression, Expression>> And
            => (from, to) => exp => Expression.AndAlso(from(exp), to(exp));
        public static Func<Func<Expression, Expression>, Func<Expression, Expression>, Func<Expression, Expression>> Or
            => (from, to) => exp => Expression.OrElse(from(exp), to(exp));

        public static Expression<Func<T, TResult>> Lambda<T, TResult>(params Func<Expression, Expression>[] exps)
        {
            var parameter = Expression.Parameter(typeof(T));

            return Expression.Lambda<Func<T, TResult>>(
                exps.Aggregate((Expression)parameter, (seed, next) => next(seed))
                    .Validate(
                       exp => typeof(TResult).IsAssignableFrom(exp.Type),
                       ConvertTo(typeof(TResult)),
                       new ArgumentException($"expression return type of result expression must be of {typeof(TResult).Name}")),
                parameter);
        }

        internal static Expression Validate(
            this Expression exp,
            Func<Expression, bool> condition,
            Func<Expression, Expression> pass,
            Exception ex)
            => condition(exp) ? pass(exp) : throw ex;
        public static Func<Expression, Expression> Then(this Func<Expression, Expression> left, Func<Expression, Expression> right)
            => exp => right(left(exp));
    }
}