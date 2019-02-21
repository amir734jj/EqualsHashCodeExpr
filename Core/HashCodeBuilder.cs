using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Core.Abstracts;
using Core.Extensions;
using Core.Interfaces;

namespace Core
{
    public class HashCodeBuilder : AbstractExpressionBuilder, IGetHashCodeBuilder
    {
        public Func<T, int> BuildFunc<T>()
        {
            return ((Expression<Func<T, int>>) BuildExpr(typeof(T))).Compile();
        }

        public Expression BuildExpr(Type type)
        {
            return BuildExprHelper(type, new List<Type> {type});
        }

        private Expression BuildExprHelper(Type type, ICollection<Type> types)
        {
            var arg = Expression.Parameter(type);

            var body = type
                .GetProperties()
                .Select<PropertyInfo, Expression>(x =>
                {
                    var propertyValueExpr = Expression.Property(arg, x);

                    // Elimintaes StackOverFlowException
                    if (types.Contains(x.PropertyType))
                    {
                        var testExpr = IsNullExpr(propertyValueExpr);
                        var ifExpr = Expression.Constant(0);
                        var elseExpr = Expression.Call(propertyValueExpr,
                            x.PropertyType.GetMethod(Constants.Constants.GetHashCodeMethodName, new Type[] { }));

                        var condition = Expression.Condition(
                            testExpr, ifExpr, elseExpr
                        );

                        return condition;
                    }
                    else if (x.PropertyType.IsDefinedComplexType())
                    {
                        var testExpr = IsNullExpr(propertyValueExpr);
                        var ifExpr = Expression.Constant(0);
                        var elseExpr = Expression.Invoke(
                            BuildExprHelper(x.PropertyType, types.Concat(new[] {x.PropertyType}).ToList()),
                            propertyValueExpr);

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