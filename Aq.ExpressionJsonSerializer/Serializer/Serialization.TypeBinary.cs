using System;
using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool TypeBinaryExpression(Expression expr)
        {
            var expression = expr as TypeBinaryExpression;
            if (expression == null) { return false; }

            this.Prop(_properties.TypeName, "typeBinary");
            this.Prop(_properties.Expression, this.Expression(expression.Expression));
            this.Prop(_properties.TypeOperand, this.Type(expression.TypeOperand));

            return true;
        }
    }
}
