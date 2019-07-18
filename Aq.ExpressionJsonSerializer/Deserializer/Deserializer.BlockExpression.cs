using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private BlockExpression BlockExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var expressions = this.Prop(obj, _properties.Expressions, this.Enumerable(this.Expression));
            var variables = this.Prop(obj, _properties.Variables, this.Enumerable(this.ParameterExpression));

            switch (nodeType) {
                case ExpressionType.Block:
                    return Expr.Block(type, variables, expressions);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
