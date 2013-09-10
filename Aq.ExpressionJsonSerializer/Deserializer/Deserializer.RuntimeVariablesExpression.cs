using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private RuntimeVariablesExpression RuntimeVariablesExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var variables = this.Prop(obj, "variables", this.Enumerable(this.ParameterExpression));

            switch (nodeType) {
                case ExpressionType.RuntimeVariables:
                    return Expr.RuntimeVariables(variables);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
