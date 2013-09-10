using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private LambdaExpression LambdaExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var body = this.Prop(obj, "body", this.Expression);
            var tailCall = this.Prop(obj, "tailCall").Value<bool>();
            var parameters = this.Prop(obj, "parameters", this.Enumerable(this.ParameterExpression));

            switch (nodeType) {
                case ExpressionType.Lambda:
                    return Expr.Lambda(body, tailCall, parameters);
                default:
                    throw new NotSupportedException();
            }
        }

        private LambdaExpression LambdaExpression(JToken token)
        {
            if (token == null || token.Type != JTokenType.Object) {
                return null;
            }

            var obj = (JObject) token;
            var nodeType = this.Prop(obj, "nodeType", this.Enum<ExpressionType>);
            var type = this.Prop(obj, "type", this.Type);
            var typeName = this.Prop(obj, "typeName", t => t.Value<string>());

            if (typeName != "lambda") { return null; }

            return this.LambdaExpression(nodeType, type, obj);
        }
    }
}
