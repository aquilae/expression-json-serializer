using System;
using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool NewExpression(Expression expr)
        {
            var expression = expr as NewExpression;
            if (expression == null) { return false; }

            this.Prop("typeName", "new");
            this.Prop("constructor", this.Constructor(expression.Constructor));
            this.Prop("arguments", this.Enumerable(expression.Arguments, this.Expression));
            this.Prop("members", this.Enumerable(expression.Members, this.Member));

            return true;
        }
    }
}
