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
            var body = this.Prop(obj, _properties.Body, this.Expression);
            var tailCall = this.Prop(obj, _properties.TailCall).Value<bool>();
            var parameters = this.Prop(obj, _properties.Parameters, this.Enumerable(this.ParameterExpression));

            switch (nodeType) {
                case ExpressionType.Lambda:
                    return Expr.Lambda(type, body, tailCall, parameters);
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
            var nodeType = this.Prop(obj, _properties.NodeType, this.Enum<ExpressionType>);
            var type = this.Prop(obj, _properties.Type, this.Type);
            var typeName = this.Prop(obj, _properties.TypeName, t => t.Value<string>());

            if (typeName != "lambda") { return null; }

            return this.LambdaExpression(nodeType, type, obj);
        }
    }
}
