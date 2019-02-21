using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Extensions;
using Core.Interfaces;

namespace Core
{
    public class EqualsBuilder : IEqualsBuilder
    {
        public Func<T, T, bool> BuildFunc<T>()
        {
            return ((Expression<Func<T, T, bool>>) BuildExpr(typeof(T))).Compile();
        }

        public Expression BuildExpr(Type type)
        {
            var arg1 = Expression.Parameter(type);
            var arg2 = Expression.Parameter(type);

            var body = type
                .GetProperties()
                .Select(x =>
                {
                    if (x.PropertyType.IsDefinedComplexType())
                    {
                        return (Expression) Expression.Invoke(BuildExpr(x.PropertyType),
                            Expression.Property(arg1, x.Name),
                            Expression.Property(arg2, x));
                    }
                    // ReSharper disable once RedundantIfElseBlock
                    else
                    {
                        return Expression.Equal(Expression.Property(arg1, x.Name), Expression.Property(arg2, x.Name));
                    }
                })
                .Aggregate(Expression.And);

            return Expression.Lambda(body, arg1, arg2);
        }
    }
}