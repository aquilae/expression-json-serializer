using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private NewExpression NewExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var constructor = this.Prop(obj, "constructor", this.Constructor);
            var arguments = this.Prop(obj, "arguments", this.Enumerable(this.Expression));
            var members = this.Prop(obj, "members", this.Enumerable(this.Member));

            switch (nodeType) {
                case ExpressionType.New:
                    if (arguments == null) {
                        if (members == null) {
                            return Expr.New(constructor);
                        }
                        return Expr.New(constructor, new Expression[0], members);
                    }
                    if (members == null) {
                        return Expr.New(constructor, arguments);
                    }
                    return Expr.New(constructor, arguments, members);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
