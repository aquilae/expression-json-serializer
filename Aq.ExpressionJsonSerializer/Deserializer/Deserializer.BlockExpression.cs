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
            var expressions = this.Prop(obj, "expressions", this.Enumerable(this.Expression));
            var variables = this.Prop(obj, "variables", this.Enumerable(this.ParameterExpression));

            switch (nodeType) {
                case ExpressionType.Block:
                    return Expr.Block(type, variables, expressions);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
