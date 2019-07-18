using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool BinaryExpression(Expression expr)
        {
            var expression = expr as BinaryExpression;
            if (expression == null) { return false; }

            this.Prop(_properties.TypeName, "binary");
            this.Prop(_properties.Left, this.Expression(expression.Left));
            this.Prop(_properties.Right, this.Expression(expression.Right));
            this.Prop(_properties.Method, this.Method(expression.Method));
            this.Prop(_properties.Conversion, this.Expression(expression.Conversion));
            this.Prop(_properties.LiftToNull, expression.IsLiftedToNull);

            return true;
        }
    }
}
