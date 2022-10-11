// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using Antlr4.Runtime.Tree;

namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {
        private Expression GetNumberExpression(string value)
        {
            // TODO: use byte, int, single, float, decimal wisely
            if (value.Contains("."))
            {
                return Expression.Constant(Convert.ToDecimal(value), typeof(decimal));
            }
            return Expression.Constant(Convert.ToInt32(value), typeof(int));
        }

        private Expression GetNumberExpression(string value, Type targetType)
        {
            if (targetType == typeof(int) ||
               targetType == typeof(long) ||
               targetType == typeof(decimal) ||
               targetType == typeof(double) ||
               targetType == typeof(float) ||
               targetType == typeof(object) ||
               targetType == typeof(byte))
            {
                return Expression.Constant(Convert.ChangeType(value, targetType), targetType);
            }
            throw new Exceptions.ValueTypeMismatchException(value, targetType.Name);
        }

        private Expression GetBoolExpression(string value, Type targetType)
        {
            return Expression.Constant(Convert.ToBoolean(value), targetType == null || targetType == typeof(bool) 
                                                                 ? typeof(bool) : targetType );
        }

        private string GetUnquotedEscapeText(string @string)
        {
            var text = @string.Substring(1, @string.Length - 2); // Trim first and last quotes
            text = text.Replace("\\\"", "\"");
            text = text.Replace("\\'", "'");
            return text;
        }

        private ParameterExpression GetVariable(string name)
        {
           return  Variables.SingleOrDefault(@var => string.Equals(@var.Name , name, StringComparison.OrdinalIgnoreCase));
        }

        private SmartJsonObjectExpression GetSmartVariable(string name)
        {
            return SmartJsonVariables.SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        private System.Reflection.PropertyInfo GetPropertyInfo(Type type, string name)
        {
           return  type.GetProperties().FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        private System.Reflection.FieldInfo GetFieldInfo(Type type, string name)
        {
            return type.GetFields().FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
        }


        private Expression ToStringExpression(Expression obj)
        {
            return Expression.Call(obj, "ToString", typeArguments: null, arguments: null);
        }

        private Expression HandleNonBooleanExpression(Expression testExpression)
        {
            if (testExpression.Type != typeof(bool))
            {
                return Expression.NotEqual(testExpression, Expression.Default(testExpression.Type));
            }
            return testExpression;
        }

        private Expression TransferAnnotationToDescendent(IParseTree parserTree)
        {
            var child = parserTree.GetChild(0);

            // transfer current tree node to child
            var targetType = TargetTypeParserContextAnnotation.Get(parserTree);

            if (targetType != null)
            {
                TargetTypeParserContextAnnotation.Put(child, targetType);
            }

            var exp = Visit(child);

            if (targetType != null)
            {
                TargetTypeParserContextAnnotation.RemoveFrom(child);
            }

            return exp;
        }
    }
}
