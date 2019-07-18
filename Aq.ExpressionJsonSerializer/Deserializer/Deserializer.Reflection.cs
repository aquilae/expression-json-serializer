using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {

        private static readonly Dictionary<Type, Dictionary<string, Dictionary<string, ConstructorInfo>>>
            ConstructorCache = new Dictionary<Type, Dictionary<string, Dictionary<string, ConstructorInfo>>>();

        private Type Type(JToken token)
        {
            
            switch(token?.Type)
            {
                case JTokenType.Object:
                    return TypeNested(token);
                case JTokenType.String:
                    return TypeFlat(token);
                default:
                    return null;
            }

        }

        private Type TypeNested(JToken token)
        {
            var obj = (JObject)token;
            var assemblyName = this.Prop(obj, _properties.AssemblyName, t => t.Value<string>());
            var typeName = this.Prop(obj, _properties.TypeName, t => t.Value<string>());
            var generic = this.Prop(obj, _properties.GenericArguments, this.Enumerable(this.Type));

            Type type = _serializer.SerializationBinder.BindToType(assemblyName, typeName);

            if (type == null)
            {
                throw new Exception(
                    "Type could not be found: "
                    + typeName + "," + assemblyName
                );
            }
            
            if (generic != null && type.IsGenericTypeDefinition)
            {
                type = type.MakeGenericType(generic.ToArray());
            }

            return type;
        }

        private Type TypeFlat(JToken token)
        {
            var val = token.Value<string>();
            int rbracket = val.LastIndexOf(']');
            if (rbracket < 0)
                rbracket = 0;
            int comma = val.IndexOf(',', rbracket);
            string typeName = val.Substring(0, comma);
            string assemblyName = val.Substring(comma + 1);
            Type type = _serializer.SerializationBinder.BindToType(assemblyName, typeName);
            return type;
        }

        private ConstructorInfo Constructor(JToken token)
        {
            if (token == null || token.Type != JTokenType.Object)
            {
                return null;
            }

            var obj = (JObject)token;
            var type = this.Prop(obj, _properties.Type, this.Type);
            var name = this.Prop(obj, _properties.Name).Value<string>();
            var signature = this.Prop(obj, _properties.Signature).Value<string>();

            ConstructorInfo constructor;
            Dictionary<string, ConstructorInfo> cache2;
            Dictionary<string, Dictionary<string, ConstructorInfo>> cache1;

            if (!ConstructorCache.TryGetValue(type, out cache1))
            {
                constructor = this.ConstructorInternal(type, name, signature);

                cache2 = new Dictionary<
                    string, ConstructorInfo>(1) {
                        {signature, constructor}
                    };

                cache1 = new Dictionary<
                    string, Dictionary<
                        string, ConstructorInfo>>(1) {
                            {name, cache2}
                        };

                ConstructorCache[type] = cache1;
            }
            else if (!cache1.TryGetValue(name, out cache2))
            {
                constructor = this.ConstructorInternal(type, name, signature);

                cache2 = new Dictionary<
                    string, ConstructorInfo>(1) {
                        {signature, constructor}
                    };

                cache1[name] = cache2;
            }
            else if (!cache2.TryGetValue(signature, out constructor))
            {
                constructor = this.ConstructorInternal(type, name, signature);
                cache2[signature] = constructor;
            }

            return constructor;
        }

        private ConstructorInfo ConstructorInternal(
            Type type, string name, string signature)
        {
            var constructor = type
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(c => c.Name == name && c.ToString() == signature);

            if (constructor == null)
            {
                constructor = type
                    .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                    .FirstOrDefault(c => c.Name == name && c.ToString() == signature);

                if (constructor == null)
                {
                    throw new Exception(
                        "Constructor for type \""
                        + type.FullName +
                        "\" with signature \""
                        + signature +
                        "\" could not be found"
                    );
                }
            }

            return constructor;
        }

        private MethodInfo Method(JToken token)
        {
            if (token == null || token.Type != JTokenType.Object)
            {
                return null;
            }

            var obj = (JObject)token;
            var type = this.Prop(obj, _properties.Type, this.Type);
            var name = this.Prop(obj, _properties.Name).Value<string>();
            var signature = this.Prop(obj, _properties.Signature).Value<string>();
            var generic = this.Prop(obj, _properties.Generic, this.Enumerable(this.Type));

            var methods = type.GetMethods(
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static
            );
            var method = methods.First(m => m.Name == name && m.ToString() == signature);

            if (generic != null && method.IsGenericMethodDefinition)
            {
                method = method.MakeGenericMethod(generic.ToArray());
            }

            return method;
        }

        private PropertyInfo Property(JToken token)
        {
            if (token == null || token.Type != JTokenType.Object)
            {
                return null;
            }

            var obj = (JObject)token;
            var type = this.Prop(obj, _properties.Type, this.Type);
            var name = this.Prop(obj, _properties.Name).Value<string>();
            var signature = this.Prop(obj, _properties.Signature).Value<string>();

            var properties = type.GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static
            );
            return properties.First(p => p.Name == name && p.ToString() == signature);
        }

        private MemberInfo Member(JToken token)
        {
            if (token == null || token.Type != JTokenType.Object)
            {
                return null;
            }

            var obj = (JObject)token;
            var type = this.Prop(obj, _properties.Type, this.Type);
            var name = this.Prop(obj, _properties.Name).Value<string>();
            var signature = this.Prop(obj, _properties.Signature).Value<string>();
            var memberType = (MemberTypes)this.Prop(obj, _properties.MemberType).Value<int>();

            var members = type.GetMembers(
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static
            );
            return members.First(p => p.MemberType == memberType
                && p.Name == name && p.ToString() == signature);
        }
    }
}
