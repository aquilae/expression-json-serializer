using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private UnaryExpression UnaryExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var operand = this.Prop(obj, "operand", this.Expression);
            var method = this.Prop(obj, "method", this.Method);
            
            switch (nodeType) {
                case ExpressionType.ArrayLength: return Expr.ArrayLength(operand);
                case ExpressionType.Convert: return Expr.Convert(operand, type, method);
                case ExpressionType.ConvertChecked: return Expr.ConvertChecked(operand, type, method);
                case ExpressionType.Decrement: return Expr.Decrement(operand, method);
                case ExpressionType.Increment: return Expr.Increment(operand, method);
                case ExpressionType.IsFalse: return Expr.IsFalse(operand, method);
                case ExpressionType.IsTrue: return Expr.IsTrue(operand, method);
                case ExpressionType.Negate: return Expr.Negate(operand, method);
                case ExpressionType.NegateChecked: return Expr.NegateChecked(operand, method);
                case ExpressionType.Not: return Expr.Not(operand, method);
                case ExpressionType.OnesComplement: return Expr.OnesComplement(operand, method);
                case ExpressionType.PostDecrementAssign: return Expr.PostDecrementAssign(operand, method);
                case ExpressionType.PostIncrementAssign: return Expr.PostIncrementAssign(operand, method);
                case ExpressionType.PreDecrementAssign: return Expr.PreDecrementAssign(operand, method);
                case ExpressionType.PreIncrementAssign: return Expr.PreIncrementAssign(operand, method);
                case ExpressionType.Quote: return Expr.Quote(operand);
                case ExpressionType.Throw: return Expr.Throw(operand, type);
                case ExpressionType.TypeAs: return Expr.TypeAs(operand, type);
                case ExpressionType.UnaryPlus: return Expr.UnaryPlus(operand, method);
                case ExpressionType.Unbox: return Expr.Unbox(operand, type);
                default: throw new NotSupportedException();
            }
        }
    }
}
