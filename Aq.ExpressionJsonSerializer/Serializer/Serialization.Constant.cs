using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool ConstantExpression(Expression expr)
        {
            var expression = expr as ConstantExpression;
            if (expression == null) { return false; }

            this.Prop(_properties.TypeName, "constant");
            if (expression.Value == null) {
                this.Prop(_properties.Value, () => this._writer.WriteNull());
            }
            else {
                var value = expression.Value;
                var type = value.GetType();
                this.Prop(_properties.Value, () => {
                    this._writer.WriteStartObject();
                    this.Prop(_properties.Type, this.Type(type));
                    this.Prop(_properties.Value, this.Serialize(value, type));
                    this._writer.WriteEndObject();
                });
            }

            return true;
        }
    }
}
