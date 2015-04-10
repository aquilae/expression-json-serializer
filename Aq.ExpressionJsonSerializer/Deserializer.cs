using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Aq.ExpressionJsonSerializer
{
    internal sealed partial class Deserializer
    {
        public static Expression Deserialize(Assembly assembly, JToken token)
        {
            var d = new Deserializer(assembly);
            return d.Expression(token);
        }

        private readonly Dictionary<string, LabelTarget> _labelTargets = new Dictionary<string, LabelTarget>();
        private readonly Assembly _assembly;

        private Deserializer(Assembly assembly)
        {
            this._assembly = assembly;
        }

        private object Deserialize(JToken token, System.Type type)
        {
            return token.ToObject(type);
        }

        private T Prop<T>(JObject obj, string name, Func<JToken, T> result = null)
        {
            var prop = obj.Property(name);

            if (result == null)
                result = token => token != null ? token.Value<T>() : default(T);

            return result(prop == null ? null : prop.Value);
        }

        private JToken Prop(JObject obj, string name)
        {
            return obj.Property(name).Value;
        }

        private T Enum<T>(JToken token)
        {
            return (T) System.Enum.Parse(typeof(T), token.Value<string>());
        }

        private Func<JToken, IEnumerable<T>> Enumerable<T>(Func<JToken, T> result)
        {
            return token => {
                if (token == null || token.Type != JTokenType.Array) {
                    return null;
                }
                var array = (JArray) token;
                return array.Select(result);
            };
        }

        private Expression Expression(JToken token)
        {
            if (token == null || token.Type != JTokenType.Object) {
                return null;
            }

            var obj = (JObject) token;
            var nodeType = this.Prop(obj, "nodeType", this.Enum<ExpressionType>);
            var type = this.Prop(obj, "type", this.Type);
            var typeName = this.Prop(obj, "typeName", t => t.Value<string>());

            switch (typeName) {
                case "binary":              return this.BinaryExpression(nodeType, type, obj);
                case "block":               return this.BlockExpression(nodeType, type, obj);
                case "conditional":         return this.ConditionalExpression(nodeType, type, obj);
                case "constant":            return this.ConstantExpression(nodeType, type, obj);
                case "debugInfo":           return this.DebugInfoExpression(nodeType, type, obj);
                case "default":             return this.DefaultExpression(nodeType, type, obj);
                case "dynamic":             return this.DynamicExpression(nodeType, type, obj);
                case "goto":                return this.GotoExpression(nodeType, type, obj);
                case "index":               return this.IndexExpression(nodeType, type, obj);
                case "invocation":          return this.InvocationExpression(nodeType, type, obj);
                case "label":               return this.LabelExpression(nodeType, type, obj);
                case "lambda":              return this.LambdaExpression(nodeType, type, obj);
                case "listInit":            return this.ListInitExpression(nodeType, type, obj);
                case "loop":                return this.LoopExpression(nodeType, type, obj);
                case "member":              return this.MemberExpression(nodeType, type, obj);
                case "memberInit":          return this.MemberInitExpression(nodeType, type, obj);
                case "methodCall":          return this.MethodCallExpression(nodeType, type, obj);
                case "newArray":            return this.NewArrayExpression(nodeType, type, obj);
                case "new":                 return this.NewExpression(nodeType, type, obj);
                case "parameter":           return this.ParameterExpression(nodeType, type, obj);
                case "runtimeVariables":    return this.RuntimeVariablesExpression(nodeType, type, obj);
                case "switch":              return this.SwitchExpression(nodeType, type, obj);
                case "try":                 return this.TryExpression(nodeType, type, obj);
                case "typeBinary":          return this.TypeBinaryExpression(nodeType, type, obj);
                case "unary":               return this.UnaryExpression(nodeType, type, obj);
            }
            throw new NotSupportedException();
        }

        private LabelTarget CreateLabelTarget(string name, Type type) {
            if (_labelTargets.ContainsKey(name))
                return _labelTargets[name];

            _labelTargets[name] = System.Linq.Expressions.Expression.Label(type, name);

            return _labelTargets[name];
        }
    }
}
