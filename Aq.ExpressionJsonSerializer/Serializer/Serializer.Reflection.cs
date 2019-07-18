using System;
using System.Collections.Generic;
using System.Reflection;

namespace Aq.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private static readonly Dictionary<Type, Tuple<string, string, Type[]>>
            TypeCache = new Dictionary<Type, Tuple<string, string, Type[]>>();

        private Action Type(Type type)
        {
            if (_nestedTypes)
                return () => this.TypeNestedInternal(type);
            else
                return () => this.TypeFlatInternal(type);

        }

        private void TypeNestedInternal(Type type)
        {
            if (type == null) {
                this._writer.WriteNull();
            }
            else {
                Tuple<string, string, Type[]> tuple;
                if (!TypeCache.TryGetValue(type, out tuple)) {
                    if (type.IsGenericType) {
                        var def = type.GetGenericTypeDefinition();
                        _serializer.SerializationBinder.BindToName(def, out string assemblyName, out string typeName);
                        tuple = new Tuple<string, string, Type[]>(
                            assemblyName, typeName,
                            type.GetGenericArguments()
                        );
                    }
                    else {
                        _serializer.SerializationBinder.BindToName(type, out string assemblyName, out string typeName);
                        tuple = new Tuple<string, string, Type[]>(
                            assemblyName, typeName, null);
                    }
                    TypeCache[type] = tuple;
                }

                this._writer.WriteStartObject();
                this.Prop(_properties.AssemblyName, tuple.Item1);
                this.Prop(_properties.TypeName, tuple.Item2);
                this.Prop(_properties.GenericArguments, this.Enumerable(tuple.Item3, this.Type));
                this._writer.WriteEndObject();
            }
        }

        private void TypeFlatInternal(Type type)
        {
            if (type == null)
            {
                _writer.WriteNull();
            }
            else
            {
                _serializer.SerializationBinder.BindToName(type, out string assemblyName, out string typeName);
                _writer.WriteValue(typeName + "," + assemblyName);
            }
        }


        private Action Constructor(ConstructorInfo constructor)
        {
            return () => this.ConstructorInternal(constructor);
        }

        private void ConstructorInternal(ConstructorInfo constructor)
        {
            if (constructor == null) {
                this._writer.WriteNull();
            }
            else {
                this._writer.WriteStartObject();
                this.Prop(_properties.Type, this.Type(constructor.DeclaringType));
                this.Prop(_properties.Name, constructor.Name);
                this.Prop(_properties.Signature, constructor.ToString());
                this._writer.WriteEndObject();
            }
        }

        private Action Method(MethodInfo method)
        {
            return () => this.MethodInternal(method);
        }

        private void MethodInternal(MethodInfo method)
        {
            if (method == null) {
                this._writer.WriteNull();
            }
            else {
                this._writer.WriteStartObject();
                if (method.IsGenericMethod) {
                    var meth = method.GetGenericMethodDefinition();
                    var generic = method.GetGenericArguments();

                    this.Prop(_properties.Type, this.Type(meth.DeclaringType));
                    this.Prop(_properties.Name, meth.Name);
                    this.Prop(_properties.Signature, meth.ToString());
                    this.Prop(_properties.Generic, this.Enumerable(generic, this.Type));
                }
                else {
                    this.Prop(_properties.Type, this.Type(method.DeclaringType));
                    this.Prop(_properties.Name, method.Name);
                    this.Prop(_properties.Signature, method.ToString());
                }
                this._writer.WriteEndObject();
            }
        }

        private Action Property(PropertyInfo property)
        {
            return () => this.PropertyInternal(property);
        }

        private void PropertyInternal(PropertyInfo property)
        {
            if (property == null) {
                this._writer.WriteNull();
            }
            else {
                this._writer.WriteStartObject();
                this.Prop(_properties.Type, this.Type(property.DeclaringType));
                this.Prop(_properties.Name, property.Name);
                this.Prop(_properties.Signature, property.ToString());
                this._writer.WriteEndObject();
            }
        }

        private Action Member(MemberInfo member)
        {
            return () => this.MemberInternal(member);
        }

        private void MemberInternal(MemberInfo member)
        {
            if (member == null) {
                this._writer.WriteNull();
            }
            else {
                this._writer.WriteStartObject();
                this.Prop(_properties.Type, this.Type(member.DeclaringType));
                this.Prop(_properties.MemberType, (int) member.MemberType);
                this.Prop(_properties.Name, member.Name);
                this.Prop(_properties.Signature, member.ToString());
                this._writer.WriteEndObject();
            }
        }
    }
}
