using System.Linq.Expressions;

namespace Core.Abstracts
{
    public abstract class AbstractExpressionBuilder
    {
        protected static BinaryExpression IsNullExpr(Expression valueExpr)
        {
            return Expression.Equal(valueExpr, Expression.Constant(null));
        }
    }
}