using System;
using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool MemberExpression(Expression expr)
        {
            var expression = expr as MemberExpression;
            if (expression == null) { return false; }

            this.Prop("typeName", "member");
            this.Prop("expression", this.Expression(expression.Expression));
            this.Prop("member", this.Member(expression.Member));

            return true;
        }
    }
}
