using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private TypeBinaryExpression TypeBinaryExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var expression = this.Prop(obj, "expression", this.Expression);
            var typeOperand = this.Prop(obj, "typeOperand", this.Type);
            
            switch (nodeType) {
                case ExpressionType.TypeIs:
                    return Expr.TypeIs(expression, typeOperand);
                case ExpressionType.TypeEqual:
                    return Expr.TypeEqual(expression, typeOperand);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
