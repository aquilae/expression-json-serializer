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

            this.Prop(_properties.TypeName, "member");
            this.Prop(_properties.Expression, this.Expression(expression.Expression));
            this.Prop(_properties.Member, this.Member(expression.Member));

            return true;
        }
    }
}
