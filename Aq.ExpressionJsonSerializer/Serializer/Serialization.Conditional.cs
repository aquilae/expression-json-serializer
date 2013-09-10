using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool ConditionalExpression(Expression expr)
        {
            var expression = expr as ConditionalExpression;
            if (expression == null) { return false; }

            this.Prop("typeName", "conditional");
            this.Prop("test", this.Expression(expression.Test));
            this.Prop("ifTrue", this.Expression(expression.IfTrue));
            this.Prop("ifFalse", this.Expression(expression.IfFalse));

            return true;
        }
    }
}
