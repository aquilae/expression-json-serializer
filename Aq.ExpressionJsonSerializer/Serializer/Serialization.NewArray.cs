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

            this.Prop(_properties.TypeName, "newArray");
            this.Prop(_properties.ElementType, this.Type(expression.Type.GetElementType()));
            this.Prop(_properties.Expressions, this.Enumerable(expression.Expressions, this.Expression));

            return true;
        }
    }
}
