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

            this.Prop(_properties.TypeName, "unary");
            this.Prop(_properties.Operand, this.Expression(expression.Operand));
            this.Prop(_properties.Method, this.Method(expression.Method));

            return true;
        }
    }
}
