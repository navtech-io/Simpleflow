// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using Simpleflow.Exceptions;

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
               targetType == typeof(byte))
            {
                return Expression.Constant(Convert.ChangeType(value, targetType), targetType);
            }
            throw new Exceptions.ValueTypeMismatchException(value, targetType.Name);
        }

        private Expression GetStringExpression(string value)
        {
            return Expression.Constant(GetUnquotedText(value));
        }

        private Expression GetBoolExpression(string value)
        {
            return Expression.Constant(Convert.ToBoolean(value), typeof(bool));
        }

        private string GetUnquotedText(string @string)
        {
            return @string.Substring(1, @string.Length - 2); // Trim first and last quotes
        }

        private Expression GetEndProperty(Expression rootObjectExp, string[] propertiesHierarchy, int startIndex = 0)
        {
            for (int index = startIndex; index < propertiesHierarchy.Length; index++)
            {
                var propertyName = propertiesHierarchy[index];
                var prop = GetProperty(rootObjectExp.Type, propertyName);

                if (prop == null)
                {
                    throw new InvalidPropertyException($"Invalid property '{propertyName}'");
                }

                rootObjectExp = Expression.Property(rootObjectExp, prop);
            }

            return rootObjectExp;
        }

        private ParameterExpression GetVariable(string name)
        {
           return  Variables.SingleOrDefault(@var => string.Equals(@var.Name , name, StringComparison.OrdinalIgnoreCase));
        }

        private SmartJsonObjectParameterExpression GetSmartVariable(string name)
        {
            return SmartJsonVariables.SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public System.Reflection.PropertyInfo GetProperty(Type type, string name)
        {
           return  type.GetProperties().FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
