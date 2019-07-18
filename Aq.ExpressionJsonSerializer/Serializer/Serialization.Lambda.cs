using System;
using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool LambdaExpression(Expression expr)
        {
            var expression = expr as LambdaExpression;
            if (expression == null) { return false; }

            this.Prop(_properties.TypeName, "lambda");
            this.Prop(_properties.Name, expression.Name);
            this.Prop(_properties.Parameters, this.Enumerable(expression.Parameters, this.Expression));
            this.Prop(_properties.Body, this.Expression(expression.Body));
            this.Prop(_properties.TailCall, expression.TailCall);

            return true;
        }
    }
}
