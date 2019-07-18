using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool BlockExpression(Expression expr)
        {
            var expression = expr as BlockExpression;
            if (expression == null) { return false; }

            this.Prop(_properties.TypeName, "block");
            this.Prop(_properties.Expressions, this.Enumerable(expression.Expressions, this.Expression));
            this.Prop(_properties.Variables, this.Enumerable(expression.Variables, this.Expression));

            return true;
        }
    }
}
