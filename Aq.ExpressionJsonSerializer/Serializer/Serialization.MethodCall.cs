using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool MethodCallExpression(Expression expr)
        {
            var expression = expr as MethodCallExpression;
            if (expression == null) { return false; }

            this.Prop("typeName", "methodCall");
            this.Prop("object", this.Expression(expression.Object));
            this.Prop("method", this.Method(expression.Method));
            this.Prop("arguments", this.Enumerable(expression.Arguments, this.Expression));

            return true;
        }
    }
}
