using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private BinaryExpression BinaryExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var left = this.Prop(obj, "left", this.Expression);
            var right = this.Prop(obj, "right", this.Expression);
            var method = this.Prop(obj, "method", this.Method);
            var conversion = this.Prop(obj, "conversion", this.LambdaExpression);
            var liftToNull = this.Prop(obj, "liftToNull").Value<bool>();

            switch (nodeType) {
                case ExpressionType.Add: return Expr.Add(left, right, method);
                case ExpressionType.AddAssign: return Expr.AddAssign(left, right, method, conversion);
                case ExpressionType.AddAssignChecked: return Expr.AddAssignChecked(left, right, method, conversion);
                case ExpressionType.AddChecked: return Expr.AddChecked(left, right, method);
                case ExpressionType.And: return Expr.And(left, right, method);
                case ExpressionType.AndAlso: return Expr.AndAlso(left, right, method);
                case ExpressionType.AndAssign: return Expr.AndAssign(left, right, method, conversion);
                case ExpressionType.ArrayIndex: return Expr.ArrayIndex(left, right);
                case ExpressionType.Assign: return Expr.Assign(left, right);
                case ExpressionType.Coalesce: return Expr.Coalesce(left, right, conversion);
                case ExpressionType.Divide: return Expr.Divide(left, right, method);
                case ExpressionType.DivideAssign: return Expr.DivideAssign(left, right, method, conversion);
                case ExpressionType.Equal: return Expr.Equal(left, right, liftToNull, method);
                case ExpressionType.ExclusiveOr: return Expr.ExclusiveOr(left, right, method);
                case ExpressionType.ExclusiveOrAssign: return Expr.ExclusiveOrAssign(left, right, method, conversion);
                case ExpressionType.GreaterThan: return Expr.GreaterThan(left, right, liftToNull, method);
                case ExpressionType.GreaterThanOrEqual: return Expr.GreaterThanOrEqual(left, right, liftToNull, method);
                case ExpressionType.LeftShift: return Expr.LeftShift(left, right, method);
                case ExpressionType.LeftShiftAssign: return Expr.LeftShiftAssign(left, right, method, conversion);
                case ExpressionType.LessThan: return Expr.LessThan(left, right, liftToNull, method);
                case ExpressionType.LessThanOrEqual: return Expr.LessThanOrEqual(left, right, liftToNull, method);
                case ExpressionType.Modulo: return Expr.Modulo(left, right, method);
                case ExpressionType.ModuloAssign: return Expr.ModuloAssign(left, right, method, conversion);
                case ExpressionType.Multiply: return Expr.Multiply(left, right, method);
                case ExpressionType.MultiplyAssign: return Expr.MultiplyAssign(left, right, method, conversion);
                case ExpressionType.MultiplyAssignChecked: return Expr.MultiplyAssignChecked(left, right, method, conversion);
                case ExpressionType.MultiplyChecked: return Expr.MultiplyChecked(left, right, method);
                case ExpressionType.NotEqual: return Expr.NotEqual(left, right, liftToNull, method);
                case ExpressionType.Or: return Expr.Or(left, right, method);
                case ExpressionType.OrAssign: return Expr.OrAssign(left, right, method, conversion);
                case ExpressionType.OrElse: return Expr.OrElse(left, right, method);
                case ExpressionType.Power: return Expr.Power(left, right, method);
                case ExpressionType.PowerAssign: return Expr.PowerAssign(left, right, method, conversion);
                case ExpressionType.RightShift: return Expr.RightShift(left, right, method);
                case ExpressionType.RightShiftAssign: return Expr.RightShiftAssign(left, right, method, conversion);
                case ExpressionType.Subtract: return Expr.Subtract(left, right, method);
                case ExpressionType.SubtractAssign: return Expr.SubtractAssign(left, right, method, conversion);
                case ExpressionType.SubtractAssignChecked: return Expr.SubtractAssignChecked(left, right, method, conversion);
                case ExpressionType.SubtractChecked: return Expr.SubtractChecked(left, right, method);
                default: throw new NotSupportedException();
            }
        }
    }
}
