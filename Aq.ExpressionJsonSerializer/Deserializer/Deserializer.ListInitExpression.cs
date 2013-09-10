using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private ListInitExpression ListInitExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
