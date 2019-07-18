using System;
using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool InvocationExpression(Expression expr)
        {
            var expression = expr as InvocationExpression;
            if (expression == null) { return false; }

            this.Prop(_properties.TypeName, "invocation");
            this.Prop(_properties.Expression, this.Expression(expression.Expression));
            this.Prop(_properties.Arguments, this.Enumerable(expression.Arguments, this.Expression));

            return true;
        }
    }
}
