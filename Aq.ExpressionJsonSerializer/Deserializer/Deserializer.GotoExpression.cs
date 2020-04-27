using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private GotoExpression GotoExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var value = this.Expression(this.Prop(obj, "value"));
            var kind = this.Enum<GotoExpressionKind>(this.Prop(obj, "kind"));
            var targetType = this.Type(this.Prop(obj, "targetType"));
            var targetName = this.Prop(obj, "targetName").Value<string>();

            switch (kind) {
                case GotoExpressionKind.Break:
                    return Expr.Break(CreateLabelTarget(targetName, targetType), value);
                case GotoExpressionKind.Continue:
                    return Expr.Continue(CreateLabelTarget(targetName, targetType));
                case GotoExpressionKind.Goto:
                    return Expr.Goto(CreateLabelTarget(targetName, targetType), value);
                case GotoExpressionKind.Return:
                    return Expr.Return(CreateLabelTarget(targetName, targetType), value);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
