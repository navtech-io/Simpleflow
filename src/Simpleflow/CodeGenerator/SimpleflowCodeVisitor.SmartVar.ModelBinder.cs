// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Antlr4.Runtime.Tree;

using Simpleflow.Exceptions;
using Simpleflow.Parser;


namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {

        private Expression ModelBinder(Type targetType, SimpleflowParser.PairContext[] pairs)
        {
            var memberBindings = new List<MemberBinding>();

            // set values to each declared property
            BindProperties(targetType, pairs, (propInfo, valueExp) => memberBindings.Add(Expression.Bind(propInfo, valueExp)));

            // Create new instance and assign member bindings
            Expression membersInitialization = Expression.MemberInit(Expression.New(targetType), memberBindings);

            return membersInitialization;
        }

        private void BindProperties(Type targetType, SimpleflowParser.PairContext[] pairs, Action<PropertyInfo, Expression> pairCallback)
        {
            foreach (var pair in pairs)
            {
                // Get Property name
                var prop = pair.Identifier().GetText();

                // Find .NET Property Type
                var member = GetPropertyInfo(targetType, prop);

                if (member == null)
                {
                    throw new InvalidPropertyException(prop);
                }

                // Get Property Text Value
                var value = pair.expression().GetChild(0);

                // Create Property Expression
                Expression valueExpression;
                if (value is SimpleflowParser.ObjectIdentifierContext oic)  
                {
                    // Handle Child Object
                    valueExpression = VisitObjectIdentiferAsPerTargetType(oic, member.PropertyType);
                }
                else
                {
                    // Handle Primitive Types
                    valueExpression = VisitWithType(value, member.PropertyType); 
                }

                // Bind property to instance
                pairCallback(member, valueExpression);
            }
        }

        private Expression VisitWithType(IParseTree tree, Type type)
        {
            TargetTypeParserContextAnnotation.Put(tree, type);
            var expression = Visit(tree);
            TargetTypeParserContextAnnotation.RemoveFrom(tree);

            return expression;
        }
    }
}
