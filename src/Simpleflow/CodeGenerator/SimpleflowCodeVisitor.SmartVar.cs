// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

using Simpleflow.Exceptions;
using Simpleflow.Parser;


namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {

        public override Expression VisitJsonObj([NotNull] SimpleflowParser.JsonObjContext context)
        {
            var type = TargetTypeParserContextAnnotation.Get(context);
            return CreateJsonObjectWithDotNetType(type, context);
        }

        private Expression CreateSmartVariableIfObjectIdentiferNotDefined(Type targetType, string name)
        {
            // Variable names are not case sensitive
            var smartVar = GetSmartVariable(name);

            if (smartVar == null)
            {
                throw new InvalidFunctionParameterNameException(name);
            }

            // Return if already created
            if (smartVar.VariableExpression != null)
            {
                return smartVar.VariableExpression;
            }

            var instanceExpressionWithMembers = CreateJsonObjectWithDotNetType(targetType, smartVar.Context.jsonObj());

            // Store created smart variable to further reuse and replace.
            return smartVar.VariableExpression = Expression.Assign(Expression.Variable(targetType), instanceExpressionWithMembers);
        }

        private Expression CreateJsonObjectWithDotNetType(Type targetType, SimpleflowParser.JsonObjContext jsonObj)
        {
            // Create
            var pairs = jsonObj.pair();
            var membersInitialization = CreateNewInstanceWithProps(targetType, pairs);


            return membersInitialization;
        }

        private Expression CreateNewInstanceWithProps(Type targetType,  SimpleflowParser.PairContext[] pairs)
        {
            var memberBindings = new List<MemberBinding>();

            // set values to each declared property
            SetValuesToProperties(targetType, pairs, (propInfo, valueExp) => memberBindings.Add(Expression.Bind(propInfo, valueExp)));

            // Create new instance and assign member bindings
            Expression membersInitialization = Expression.MemberInit(Expression.New(targetType), memberBindings);

            return membersInitialization;
        }

        private void SetValuesToProperties(Type targetType, SimpleflowParser.PairContext[] pairs, Action<PropertyInfo, Expression> pairCallback)
        {
            foreach (var pair in pairs)
            {
                // Property name
                var prop = pair.Identifier().GetText();

                // Property Type
                var member = GetPropertyInfo(targetType, prop);

                if (member == null)
                {
                    throw new InvalidPropertyException(prop);
                }

                // Property Value
                var value = pair.expression().GetChild(0);

                // Create Property Expression
                Expression valueExpression;
                if (value is SimpleflowParser.ObjectIdentifierContext oic)
                {
                    valueExpression = VisitParameterObjectIdentifer(oic, member.PropertyType);
                }
                else
                {
                    valueExpression = VisitWithType(value, member.PropertyType);
                }

                // Bind member
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


        private Expression VisitPartialSet(SimpleflowParser.SetStmtContext context, Expression variable)
        {
            // variable.Left.Type
            var pairs = context.expression().jsonObj().pair();
            List<Expression> propExpressions = new List<Expression>();

            // set values to each declared property
            SetValuesToProperties(variable.Type, 
                                 pairs, 
                                 (propInfo, valueExp) => 
                                        propExpressions.Add(Expression.Assign(Expression.Property(variable, propInfo), valueExp)));

            // context.expression
            return Expression.Block(propExpressions);
        }
    }
}
