using System.Linq.Expressions;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool ConstantExpression(Expression expr)
        {
            var expression = expr as ConstantExpression;
            if (expression == null) { return false; }

            this.Prop("typeName", "constant");
            if (expression.Value == null) {
                this.Prop("value", () => this._writer.WriteNull());
            }
            else {
                var value = expression.Value;
                var type = value.GetType();
                this.Prop("value", () => {
                    this._writer.WriteStartObject();
                    this.Prop("type", this.Type(type));
                    this.Prop("value", this.Serialize(value, type));
                    this._writer.WriteEndObject();
                });
            }

            return true;
        }
    }
}
