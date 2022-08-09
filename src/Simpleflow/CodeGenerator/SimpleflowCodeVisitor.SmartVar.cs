// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Linq.Expressions;
using Antlr4.Runtime.Misc;

using Simpleflow.Exceptions;
using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {
        public override Expression VisitJsonObj([NotNull] SimpleflowParser.JsonObjContext context)
        {
            var type = TargetTypeParserContextAnnotation.Get(context);
            return CreateNewEntityInstance(type, context.pair());
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

            var instanceExpressionWithMembers = CreateNewEntityInstance(targetType, smartVar.Context.jsonObj().pair());

            // Store created smart variable to further reuse and replace.
            return smartVar.VariableExpression = Expression.Assign(Expression.Variable(targetType), instanceExpressionWithMembers);
        }

        private Expression CreateNewEntityInstance(Type targetType, SimpleflowParser.PairContext[] pairs)
        {
            // if targetType is ExpandoObject
            return ModelBinder(targetType, pairs);
        }
    }
}
