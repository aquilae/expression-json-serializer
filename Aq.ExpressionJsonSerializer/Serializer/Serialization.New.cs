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

            this.Prop(_properties.TypeName, "new");
            this.Prop(_properties.Constructor, this.Constructor(expression.Constructor));
            this.Prop(_properties.Arguments, this.Enumerable(expression.Arguments, this.Expression));
            this.Prop(_properties.Members, this.Enumerable(expression.Members, this.Member));

            return true;
        }
    }
}
