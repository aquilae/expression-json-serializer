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

            this.Prop("typeName", "invocation");
            this.Prop("expression", this.Expression(expression.Expression));
            this.Prop("arguments", this.Enumerable(expression.Arguments, this.Expression));

            return true;
        }
    }
}
