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

            this.Prop(_properties.TypeName, "index");
            this.Prop(_properties.Object, this.Expression(expression.Object));
            this.Prop(_properties.Indexer, this.Property(expression.Indexer));
            this.Prop(_properties.Arguments, this.Enumerable(expression.Arguments, this.Expression));

            return true;
        }
    }
}
