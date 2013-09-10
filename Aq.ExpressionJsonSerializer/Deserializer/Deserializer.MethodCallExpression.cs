using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private MethodCallExpression MethodCallExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var instance = this.Prop(obj, "object", this.Expression);
            var method = this.Prop(obj, "method", this.Method);
            var arguments = this.Prop(obj, "arguments", this.Enumerable(this.Expression));

            switch (nodeType) {
                case ExpressionType.ArrayIndex:
                    return Expr.ArrayIndex(instance, arguments);
                case ExpressionType.Call:
                    return Expr.Call(instance, method, arguments);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
