using System;
using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool UnaryExpression(Expression expr)
        {
            var expression = expr as UnaryExpression;
            if (expression == null) { return false; }

            this.Prop("typeName", "unary");
            this.Prop("operand", this.Expression(expression.Operand));
            this.Prop("method", this.Method(expression.Method));

            return true;
        }
    }
}
