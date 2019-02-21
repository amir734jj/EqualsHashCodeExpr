using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Core.Extensions;
using Core.Interfaces;

namespace Core
{
    public class HashCodeBuilder : IGetHashCodeBuilder
    {
        public Func<T, int> BuildFunc<T>()
        {
            return ((Expression<Func<T, int>>) BuildExpr(typeof(T))).Compile();
        }

        public Expression BuildExpr(Type type)
        {
            var arg = Expression.Parameter(type);

            var body = type
                .GetProperties()
                .Select<PropertyInfo, Expression>(x =>
                {
                    var propertyValueExpr = Expression.Property(arg, x);

                    if (x.PropertyType.IsDefinedComplexType())
                    {
                        var testExpr = Expression.NotEqual(propertyValueExpr, Expression.Constant(null));
                        var ifExpr = Expression.Invoke(BuildExpr(x.PropertyType), propertyValueExpr);
                        var elseExpr = Expression.Constant(0);

                        var condition = Expression.Condition(
                            testExpr, ifExpr, elseExpr
                        );

                        return condition;
                    }
                    // ReSharper disable once RedundantIfElseBlock
                    else
                    {
                        // Empty type array is needed to eliminate ambiguity
                        return Expression.Call(propertyValueExpr,
                            x.PropertyType.GetMethod(Constants.Constants.GetHashCodeMethodName, new Type[] { }));
                    }
                })
                .Aggregate(Expression.Add(Expression.Constant(0), Expression.Constant(0)),
                    (x, y) => Expression.ExclusiveOr(
                        Expression.Multiply(x, Expression.Constant(Constants.Constants.Prime)), y));


            return Expression.Lambda(body, arg);
        }
    }
}