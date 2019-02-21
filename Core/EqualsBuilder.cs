using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Abstracts;
using Core.Extensions;
using Core.Interfaces;

namespace Core
{
    public class EqualsBuilder : AbstractExpressionBuilder, IEqualsBuilder
    {
        public Func<T, T, bool> BuildFunc<T>()
        {
            return ((Expression<Func<T, T, bool>>) BuildExpr(typeof(T))).Compile();
        }

        public Expression BuildExpr(Type type)
        {
            return BuildExprhelper(type, new List<Type> {type});
        }

        private Expression BuildExprhelper(Type type, ICollection<Type> types)
        {
            var arg1 = Expression.Parameter(type);
            var arg2 = Expression.Parameter(type);

            var body = type
                .GetProperties()
                .Select(x =>
                {
                    var propertyValue1Expr = Expression.Property(arg1, x);
                    var propertyValue2Expr = Expression.Property(arg1, x);

                    // Elimintaes StackOverFlowException
                    if (types.Contains(x.PropertyType))
                    {
                        var testExpr = IsNullExpr(propertyValue1Expr);
                        var ifExpr = Expression.And(IsNullExpr(propertyValue1Expr), IsNullExpr(propertyValue2Expr));
                        var elseExpr = (Expression) Expression.Call(
                            propertyValue1Expr,
                            x.PropertyType.GetMethod(Constants.Constants.EqualsMethodName, new[] {typeof(object)}),
                            Expression.Property(arg2, x));

                        return Expression.Condition(testExpr, ifExpr, elseExpr);
                    }
                    else if (x.PropertyType.IsDefinedComplexType())
                    {
                        return (Expression) Expression.Invoke(
                            BuildExprhelper(x.PropertyType, types.Concat(new[] {x.PropertyType}).ToList()),
                            Expression.Property(arg1, x),
                            Expression.Property(arg2, x));
                    }
                    // ReSharper disable once RedundantIfElseBlock
                    else
                    {
                        return Expression.Equal(Expression.Property(arg1, x), Expression.Property(arg2, x));
                    }
                })
                .Aggregate(Expression.And);

            return Expression.Lambda(body, arg1, arg2);
        }
    }
}