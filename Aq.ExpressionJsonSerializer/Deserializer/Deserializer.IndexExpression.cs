﻿using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Expr = System.Linq.Expressions.Expression;

namespace Aq.ExpressionJsonSerializer
{
    partial class Deserializer
    {
        private IndexExpression IndexExpression(
            ExpressionType nodeType, System.Type type, JObject obj)
        {
            var expression = this.Prop(obj, _properties.Object, this.Expression);
            var indexer = this.Prop(obj, _properties.Indexer, this.Property);
            var arguments = this.Prop(obj, _properties.Arguments, this.Enumerable(this.Expression));

            switch (nodeType) {
                case ExpressionType.Index:
                    return Expr.MakeIndex(expression, indexer, arguments);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
