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
                    var propertyValue = Expression.Property(arg, x);

                    if (x.PropertyType.IsDefinedComplexType())
                    {
                        var testExpr = Expression.NotEqual(propertyValue, Expression.Constant(null));
                        var ifExpr = Expression.Invoke(BuildExpr(x.PropertyType), propertyValue);
                        var elseExpr = Expression.Constant(0);

                        var condition = Expression.Condition(
                            testExpr, ifExpr, elseExpr
                        );

                        return condition;
                    }
                    // ReSharper disable once RedundantIfElseBlock
                    else
                    {
                        return Expression.Call(propertyValue,
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