using System;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IEqualsBuilder
    {
        Func<T, T, bool> BuildFunc<T>();

        Expression BuildExpr(Type type);
    }
}