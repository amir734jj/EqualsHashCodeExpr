using System.Linq.Expressions;

namespace Core.Abstracts
{
    public abstract class AbstractExpressionBuilder
    {
        protected BinaryExpression IsNullExpr(Expression valueExpr)
        {
            return Expression.Equal(valueExpr, Expression.Constant(null));
        }
    }
}