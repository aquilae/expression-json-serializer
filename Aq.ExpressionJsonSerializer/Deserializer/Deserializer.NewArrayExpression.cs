using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private NewArrayExpression NewArrayExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var elementType = this.Prop(obj, "elementType", this.Type);
            var expressions = this.Prop(obj, "expressions", this.Enumerable(this.Expression));

            switch (nodeType) {
                case ExpressionType.NewArrayInit:
                    return Expr.NewArrayInit(elementType, expressions);
                case ExpressionType.NewArrayBounds:
                    return Expr.NewArrayBounds(elementType, expressions);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
