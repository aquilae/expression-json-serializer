using System;
using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool RuntimeVariablesExpression(Expression expr)
        {
            var expression = expr as RuntimeVariablesExpression;
            if (expression == null) { return false; }

            this.Prop("typeName", "runtimeVariables");
            this.Prop("variables", this.Enumerable(expression.Variables, this.Expression));

            return true;
        }
    }
}
