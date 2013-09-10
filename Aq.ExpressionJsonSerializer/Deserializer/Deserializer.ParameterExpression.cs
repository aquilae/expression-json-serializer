using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private readonly Dictionary<string, ParameterExpression>
            _parameterExpressions = new Dictionary<string, ParameterExpression>();

        private ParameterExpression ParameterExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var name = this.Prop(obj, "name", t => t.Value<string>());

            ParameterExpression result;
            if (_parameterExpressions.TryGetValue(name, out result)) {
                return result;
            }

            switch (nodeType) {
                case ExpressionType.Parameter:
                    result = Expr.Parameter(type, name);
                    break;
                default:
                    throw new NotSupportedException();
            }

            _parameterExpressions[name] = result;
            return result;
        }

        private ParameterExpression ParameterExpression(JToken token)
        {
            if (token == null || token.Type != JTokenType.Object) {
                return null;
            }

            var obj = (JObject) token;
            var nodeType = this.Prop(obj, "nodeType", this.Enum<ExpressionType>);
            var type = this.Prop(obj, "type", this.Type);
            var typeName = this.Prop(obj, "typeName", t => t.Value<string>());

            if (typeName != "parameter") { return null; }

            return this.ParameterExpression(nodeType, type, obj);
        }
    }
}
