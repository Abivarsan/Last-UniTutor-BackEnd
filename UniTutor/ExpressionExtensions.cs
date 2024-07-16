using System;
using System.Linq.Expressions;

namespace UniTutor.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, TResult>> Compose<T, TIntermediate, TResult>(
            this Expression<Func<T, TIntermediate>> first,
            Expression<Func<TIntermediate, TResult>> second)
        {
            var param = Expression.Parameter(typeof(T), "param");
            var newFirst = Expression.Invoke(first, param);
            var newSecond = Expression.Invoke(second, newFirst);

            return Expression.Lambda<Func<T, TResult>>(newSecond, param);
        }
    }
}
