using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool MethodCallExpression(Expression expr)
        {
            var expression = expr as MethodCallExpression;
            if (expression == null) { return false; }

            this.Prop(_properties.TypeName, "methodCall");
            this.Prop(_properties.Object, this.Expression(expression.Object));
            this.Prop(_properties.Method, this.Method(expression.Method));
            this.Prop(_properties.Arguments, this.Enumerable(expression.Arguments, this.Expression));

            return true;
        }
    }
}
