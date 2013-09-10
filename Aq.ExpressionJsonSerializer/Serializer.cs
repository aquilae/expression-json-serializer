using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;

namespace Aq.ExpressionJsonSerializer
{
    internal sealed partial class Serializer
    {
        public static void Serialize(
            JsonWriter writer,
            JsonSerializer serializer,
            Expression expression)
        {
            var s = new Serializer(writer, serializer);
            s.ExpressionInternal(expression);
        }

        private readonly JsonWriter _writer;
        private readonly JsonSerializer _serializer;

        private Serializer(JsonWriter writer, JsonSerializer serializer)
        {
            this._writer = writer;
            this._serializer = serializer;
        }

        private Action Serialize(object value, System.Type type)
        {
            return () => this._serializer.Serialize(this._writer, value, type);
        }

        private void Prop(string name, bool value)
        {
            this._writer.WritePropertyName(name);
            this._writer.WriteValue(value);
        }

        private void Prop(string name, int value)
        {
            this._writer.WritePropertyName(name);
            this._writer.WriteValue(value);
        }

        private void Prop(string name, string value)
        {
            this._writer.WritePropertyName(name);
            this._writer.WriteValue(value);
        }

        private void Prop(string name, Action valueWriter)
        {
            this._writer.WritePropertyName(name);
            valueWriter();
        }

        private Action Enum<TEnum>(TEnum value)
        {
            return () => this.EnumInternal(value);
        }

        private void EnumInternal<TEnum>(TEnum value)
        {
            this._writer.WriteValue(System.Enum.GetName(typeof(TEnum), value));
        }

        private Action Enumerable<T>(IEnumerable<T> items, Func<T, Action> func)
        {
            return () => this.EnumerableInternal(items, func);
        }

        private void EnumerableInternal<T>(IEnumerable<T> items, Func<T, Action> func)
        {
            if (items == null) {
                this._writer.WriteNull();
            }
            else {
                this._writer.WriteStartArray();
                foreach (var item in items) {
                    func(item)();
                }
                this._writer.WriteEndArray();
            }
        }

        private Action Expression(Expression expression)
        {
            return () => this.ExpressionInternal(expression);
        }

        private void ExpressionInternal(Expression expression)
        {
            if (expression == null) {
                this._writer.WriteNull();
                return;
            }

            while (expression.CanReduce) {
                expression = expression.Reduce();
            }

            this._writer.WriteStartObject();

            this.Prop("nodeType", this.Enum(expression.NodeType));
            this.Prop("type", this.Type(expression.Type));

            if (this.BinaryExpression(expression)) { goto end; }
            if (this.BlockExpression(expression)) { goto end; }
            if (this.ConditionalExpression(expression)) { goto end; }
            if (this.ConstantExpression(expression)) { goto end; }
            if (this.DebugInfoExpression(expression)) { goto end; }
            if (this.DefaultExpression(expression)) { goto end; }
            if (this.DynamicExpression(expression)) { goto end; }
            if (this.GotoExpression(expression)) { goto end; }
            if (this.IndexExpression(expression)) { goto end; }
            if (this.InvocationExpression(expression)) { goto end; }
            if (this.LabelExpression(expression)) { goto end; }
            if (this.LambdaExpression(expression)) { goto end; }
            if (this.ListInitExpression(expression)) { goto end; }
            if (this.LoopExpression(expression)) { goto end; }
            if (this.MemberExpression(expression)) { goto end; }
            if (this.MemberInitExpression(expression)) { goto end; }
            if (this.MethodCallExpression(expression)) { goto end; }
            if (this.NewArrayExpression(expression)) { goto end; }
            if (this.NewExpression(expression)) { goto end; }
            if (this.ParameterExpression(expression)) { goto end; }
            if (this.RuntimeVariablesExpression(expression)) { goto end; }
            if (this.SwitchExpression(expression)) { goto end; }
            if (this.TryExpression(expression)) { goto end; }
            if (this.TypeBinaryExpression(expression)) { goto end; }
            if (this.UnaryExpression(expression)) { goto end; }

            throw new NotSupportedException();

        end:
            this._writer.WriteEndObject();
        }
    }
}
