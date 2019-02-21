using System;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IGetHashCodeBuilder
    {
        Func<T, int> BuildFunc<T>();

        Expression BuildExpr(Type type);
    }
}