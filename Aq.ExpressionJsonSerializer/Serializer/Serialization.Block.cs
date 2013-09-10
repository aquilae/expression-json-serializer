using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool BlockExpression(Expression expr)
        {
            var expression = expr as BlockExpression;
            if (expression == null) { return false; }

            this.Prop("typeName", "block");
            this.Prop("expressions", this.Enumerable(expression.Expressions, this.Expression));
            this.Prop("variables", this.Enumerable(expression.Variables, this.Expression));

            return true;
        }
    }
}
