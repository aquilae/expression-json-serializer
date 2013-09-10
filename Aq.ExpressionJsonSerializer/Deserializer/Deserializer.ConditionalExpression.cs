using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private ConditionalExpression ConditionalExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var test = this.Prop(obj, "test", this.Expression);
            var ifTrue = this.Prop(obj, "ifTrue", this.Expression);
            var ifFalse = this.Prop(obj, "ifFalse", this.Expression);

            switch (nodeType) {
                case ExpressionType.Conditional:
                    return Expr.Condition(test, ifTrue, ifFalse, type);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
