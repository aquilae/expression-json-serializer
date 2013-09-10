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

            this.Prop("typeName", "typeBinary");
            this.Prop("expression", this.Expression(expression.Expression));
            this.Prop("typeOperand", this.Type(expression.TypeOperand));

            return true;
        }
    }
}
