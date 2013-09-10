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
            return () => this.TypeInternal(type);
        }

        private void TypeInternal(Type type)
        {
            if (type == null) {
                this._writer.WriteNull();
            }
            else {
                Tuple<string, string, Type[]> tuple;
                if (!TypeCache.TryGetValue(type, out tuple)) {
                    var assemblyName = type.Assembly.FullName;
                    if (type.IsGenericType) {
                        var def = type.GetGenericTypeDefinition();
                        tuple = new Tuple<string, string, Type[]>(
                            def.Assembly.FullName, def.FullName,
                            type.GetGenericArguments()
                        );
                    }
                    else {
                        tuple = new Tuple<string, string, Type[]>(
                            assemblyName, type.FullName, null);
                    }
                    TypeCache[type] = tuple;
                }

                this._writer.WriteStartObject();
                this.Prop("assemblyName", tuple.Item1);
                this.Prop("typeName", tuple.Item2);
                this.Prop("genericArguments", this.Enumerable(tuple.Item3, this.Type));
                this._writer.WriteEndObject();
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
                this.Prop("type", this.Type(constructor.DeclaringType));
                this.Prop("name", constructor.Name);
                this.Prop("signature", constructor.ToString());
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

                    this.Prop("type", this.Type(meth.DeclaringType));
                    this.Prop("name", meth.Name);
                    this.Prop("signature", meth.ToString());
                    this.Prop("generic", this.Enumerable(generic, this.Type));
                }
                else {
                    this.Prop("type", this.Type(method.DeclaringType));
                    this.Prop("name", method.Name);
                    this.Prop("signature", method.ToString());
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
                this.Prop("type", this.Type(property.DeclaringType));
                this.Prop("name", property.Name);
                this.Prop("signature", property.ToString());
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
                this.Prop("type", this.Type(member.DeclaringType));
                this.Prop("memberType", (int) member.MemberType);
                this.Prop("name", member.Name);
                this.Prop("signature", member.ToString());
                this._writer.WriteEndObject();
            }
        }
    }
}
