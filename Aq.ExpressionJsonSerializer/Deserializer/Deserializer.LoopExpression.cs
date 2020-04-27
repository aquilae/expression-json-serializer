using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private LoopExpression LoopExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var body = this.Prop(obj, "body", this.Expression);

            var breakName = this.Prop<string>(obj, "breakLabelName");
            var breakType = this.Prop(obj, "breakLabeType", this.Type);

            var continueName = this.Prop<string>(obj, "continueLabelName");
            var contiunueType = this.Prop(obj, "continueLabelType", this.Type);

            if (contiunueType != null) {
                return Expr.Loop(body, CreateLabelTarget(breakName, breakType), CreateLabelTarget(continueName, contiunueType));    
            }

            return Expr.Loop(body, CreateLabelTarget(breakName, breakType));
        }
    }
}
