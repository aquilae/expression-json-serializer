using System;
using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool GotoExpression(Expression expr)
        {
            var expression = expr as GotoExpression;
            if (expression == null) { return false; }

            this.Prop("typeName", "goto");
            this.Prop("value", this.Expression(expression.Value));
            this.Prop("kind", this.Enum(expression.Kind));
            this.Prop("targetName", expression.Target.Name ?? "#" + expression.Target.GetHashCode());
            this.Prop("targetType", this.Type(expression.Target.Type));

            return true;
        }
    }
}
