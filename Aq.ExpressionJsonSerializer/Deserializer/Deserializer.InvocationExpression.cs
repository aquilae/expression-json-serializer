using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private InvocationExpression InvocationExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var expression = this.Prop(obj, "expression", this.Expression);
            var arguments = this.Prop(obj, "arguments", this.Enumerable(this.Expression));
            
            switch (nodeType) {
                case ExpressionType.Invoke:
                    if (arguments == null) {
                        return Expr.Invoke(expression);
                    }
                    return Expr.Invoke(expression, arguments);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
