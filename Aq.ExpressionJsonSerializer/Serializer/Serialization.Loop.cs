using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool LoopExpression(Expression expr)
        {
            var expression = expr as LoopExpression;
            if (expression == null) { return false; }

            this.Prop("typeName", "loop");
            this.Prop("body", this.Expression(expression.Body));

            if (expression.BreakLabel != null) {
                this.Prop("breakLabelName", expression.BreakLabel.Name ?? "#" + expression.BreakLabel.GetHashCode());
                this.Prop("breakLabeType", this.Type(expression.BreakLabel.Type));
            }

            if (expression.ContinueLabel != null) {
                this.Prop("continueLabelName", expression.ContinueLabel.Name ?? "#" + expression.ContinueLabel.GetHashCode());
                this.Prop("continueLabelType", this.Type(expression.ContinueLabel.Type));
            }

            return true;
        }
    }
}
