using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool ConditionalExpression(Expression expr)
        {
            var expression = expr as ConditionalExpression;
            if (expression == null) { return false; }

            this.Prop(_properties.TypeName, "conditional");
            this.Prop(_properties.Test, this.Expression(expression.Test));
            this.Prop(_properties.IfTrue, this.Expression(expression.IfTrue));
            this.Prop(_properties.IfFalse, this.Expression(expression.IfFalse));

            return true;
        }
    }
}
