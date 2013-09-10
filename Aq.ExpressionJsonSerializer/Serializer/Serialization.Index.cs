using System;
using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool IndexExpression(Expression expr)
        {
            var expression = expr as IndexExpression;
            if (expression == null) { return false; }

            this.Prop("typeName", "index");
            this.Prop("object", this.Expression(expression.Object));
            this.Prop("indexer", this.Property(expression.Indexer));
            this.Prop("arguments", this.Enumerable(expression.Arguments, this.Expression));

            return true;
        }
    }
}
