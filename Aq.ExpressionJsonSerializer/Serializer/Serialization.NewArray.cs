using System;
using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool NewArrayExpression(Expression expr)
        {
            var expression = expr as NewArrayExpression;
            if (expression == null) { return false; }

            this.Prop("typeName", "newArray");
            this.Prop("elementType", this.Type(expression.Type.GetElementType()));
            this.Prop("expressions", this.Enumerable(expression.Expressions, this.Expression));

            return true;
        }
    }
}
