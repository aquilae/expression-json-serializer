using System;
using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool GotoExpression(Expression expr)
        {
            var expression = expr as GotoExpression;
            if (expression == null) { return false; }

            throw new NotImplementedException();
        }
    }
}
