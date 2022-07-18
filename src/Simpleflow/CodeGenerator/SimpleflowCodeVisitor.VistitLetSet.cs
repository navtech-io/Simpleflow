// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Linq;
using System.Linq.Expressions;

using Simpleflow.Exceptions;
using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {
        public override Expression VisitLetStmt(SimpleflowParser.LetStmtContext context)
        {
            if (context.exception != null)
                throw context.exception;

            var expression = context.expression();
            var letIdentifier = context.Identifier().GetText();

            // Validate variable name
            if (SimpleflowKeywords.Keywords.Any(keyword => string.Equals(keyword, letIdentifier, StringComparison.Ordinal)))
            {
                throw new VariableNameViolationException(letIdentifier);
            }

            return GetLetVariableExpression(expression, letIdentifier);

        }

        // Mutate statement
        public override Expression VisitSetStmt(SimpleflowParser.SetStmtContext context)
        {
            // Find variable and assign it 
            var variableName = context.Identifier().GetText();

            // Assume that SmartVariable has already created as part of function invocation if available
            Expression variableExpression = GetVariable(variableName) ?? GetSmartVariable(variableName)?.VariableExpression?.Left;

            if (variableExpression == null)
            {
                throw new UndeclaredVariableException(variableName);
            }
            
            var expression = context.expression();
            Expression rightSideSetExpression;

            if (context.Partial() != null) // visit complex type partially
            {
                if (expression.jsonObj() == null)
                {
                    throw new SimpleflowException(Resources.Message.InvalidPartialKeywordUsage);
                }
                return VisitPartialSet(context, variableExpression);
            }

            else if (expression.jsonObj() != null) // visit complex type fully - complete replace of reference
            {
                rightSideSetExpression = CreateNewInstanceWithProps(variableExpression.Type, expression.jsonObj().pair());
            }
            else // visit simple type
            {
                rightSideSetExpression = Visit(expression.GetChild(0));
            }
            return Expression.Assign(variableExpression, rightSideSetExpression);
        }


        private Expression GetLetVariableExpression(SimpleflowParser.ExpressionContext context, string variableName)
        {
            if (context.jsonObj() != null)
            {
                return new SmartJsonObjectParameterExpression(context, variableName);
            }

            var expression = Visit(context.GetChild(0));
            return Expression.Assign(Expression.Variable(expression.Type, variableName), expression);
        }

    }
}
