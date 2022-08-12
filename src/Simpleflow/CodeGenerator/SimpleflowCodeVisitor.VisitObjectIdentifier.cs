// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Linq;
using System.Linq.Expressions;

using Simpleflow.Exceptions;
using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {
        public override Expression VisitObjectIdentifier(SimpleflowParser.ObjectIdentifierContext context)
        {
            // Get initial object
            var variableName = context.identifierIndex()[0].Identifier().GetText();
            Expression objectExp = GetVariable(variableName) ?? GetSmartVariable(variableName)?.VariableExpression?.Left;

            if (objectExp == null)
            {
                throw new UndeclaredVariableException(variableName);
            }

            // Get index object if specified
            var indexObjectExp = GetIndexObjectExpIfDefined(objectExp, context.identifierIndex()[0].index());

            // Traverse through and get final object
            return GetFinalPropertyValue(indexObjectExp, context.identifierIndex());
        }

        private Expression GetIndexObjectExpIfDefined(Expression objectExp, SimpleflowParser.IndexContext context)
        {
            if (context != null)
            {
                var indexExpression = Visit(context.indexExpression().GetChild(0)); // represents index 

                var indexProperty
                    = objectExp
                        .Type
                        .GetProperties()
                        .SingleOrDefault(p => p.GetIndexParameters().Length == 1 &&
                                              p.GetIndexParameters()[0].ParameterType == indexExpression.Type);
                
                return Expression.MakeIndex(objectExp, indexProperty, new[] { indexExpression });
            }

            return objectExp;
        }

        private Expression GetFinalPropertyValue(Expression propExp, SimpleflowParser.IdentifierIndexContext[] propertiesHierarchy)
        {
            for (int i = 1; i < propertiesHierarchy.Length; i++)
            {
                var property = propertiesHierarchy[i];

                // Get next property name
                var propName = property.Identifier().GetText();
                var prop = GetPropertyInfo(propExp.Type, propName);

                if (prop == null)
                {
                    throw new InvalidPropertyException($"Invalid property '{propName}'");
                }

                // Get indexed object
                propExp = GetIndexObjectExpIfDefined(propExp, property.index());

                // Get property of indexed object
                propExp = Expression.Property(propExp, prop);
            }
            return propExp;
        }
    }
}
