using System;
using System.Linq.Expressions;
using System.Reflection;
using Aq.ExpressionJsonSerializer.PreProcessor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Aq.ExpressionJsonSerializer
{
    public class ExpressionJsonConverter : JsonConverter
    {
        private static readonly System.Type TypeOfExpression = typeof (Expression);

        private PropertyNames _properties;
        private Assembly[] contractAssemblies;

        /// <summary>
        /// Set to true to use legacy style nested type serialisation.
        /// </summary>
        public bool NestedTypeSerialization { get; set; }

        /// <summary>
        /// The NamingStrategy can be inferred from the Serializer as long as its <see cref="JsonSerializer.ContractResolver"/>,
        /// descends from <see cref="DefaultContractResolver"/>, but you can set this to enforce 
        /// the property naming conventions if it doesn't, or if the naming convention 
        /// should be different for expressions.
        /// </summary>
        public NamingStrategy NamingStrategy { get; set; }

        public ExpressionJsonConverter()
        {
        }

        public ExpressionJsonConverter(Assembly[] contractAssemblies)
        {
            this.contractAssemblies = contractAssemblies;
        }

        public override bool CanConvert(System.Type objectType)
        {
            return objectType == TypeOfExpression
                || objectType.IsSubclassOf(TypeOfExpression);
        }

        public override void WriteJson(
            JsonWriter writer, object value, JsonSerializer serializer)
        {
            InitProperties(serializer);
            Expression expr = (Expression)value;
            if(contractAssemblies != null && expr != null)
            {
                expr = RemoveLocalReferences(expr);
            }

            Serializer.Serialize(writer, serializer, _properties, expr, NestedTypeSerialization);
        }

        private Expression RemoveLocalReferences(Expression expr)
        {
            ProcessingVisitor visitor = new ProcessingVisitor(contractAssemblies);
            return visitor.Visit(expr);
        }

        public override object ReadJson(
            JsonReader reader, System.Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            InitProperties(serializer);
            return Deserializer.Deserialize(
                JToken.ReadFrom(reader), serializer,
                _properties, NestedTypeSerialization
            );
        }

        private void InitProperties(JsonSerializer serializer)
        {
            if (_properties != null)
                return;

            _properties = new PropertyNames();
            NamingStrategy namingStrategy = NamingStrategy ?? (serializer.ContractResolver as DefaultContractResolver)?.NamingStrategy;
            if (namingStrategy != null)
            {
                // Use the defined naming strategy
                foreach (var prop in _properties.GetType().GetProperties())
                {
                    prop.SetValue(_properties, namingStrategy.GetPropertyName(prop.Name, false));
                }
            }

        }

    }
}
