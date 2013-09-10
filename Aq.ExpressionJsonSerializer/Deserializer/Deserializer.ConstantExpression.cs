using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private ConstantExpression ConstantExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            object value;

            var valueTok = this.Prop(obj, "value");
            if (valueTok == null || valueTok.Type == JTokenType.Null) {
                value = null;
            }
            else {
                var valueObj = (JObject) valueTok;
                var valueType = this.Prop(valueObj, "type", this.Type);
                value = this.Deserialize(this.Prop(valueObj, "value"), valueType);
            }

            switch (nodeType) {
                case ExpressionType.Constant:
                    return Expr.Constant(value, type);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
