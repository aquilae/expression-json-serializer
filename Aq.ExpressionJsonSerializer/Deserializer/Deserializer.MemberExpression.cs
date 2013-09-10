using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private MemberExpression MemberExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var expression = this.Prop(obj, "expression", this.Expression);
            var member = this.Prop(obj, "member", this.Member);

            switch (nodeType) {
                case ExpressionType.MemberAccess:
                    return Expr.MakeMemberAccess(expression, member);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
