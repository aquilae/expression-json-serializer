using System;
using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool LambdaExpression(Expression expr)
        {
            var expression = expr as LambdaExpression;
            if (expression == null) { return false; }

            this.Prop("typeName", "lambda");
            this.Prop("name", expression.Name);
            this.Prop("parameters", this.Enumerable(expression.Parameters, this.Expression));
            this.Prop("body", this.Expression(expression.Body));
            this.Prop("tailCall", expression.TailCall);

            return true;
        }
    }
}
