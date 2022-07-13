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

            return VisitVariableExpression(expression, letIdentifier, variable: null, completeExpForErrorOut: context.GetText());
        }

        // Mutate statement
        public override Expression VisitSetStmt(SimpleflowParser.SetStmtContext context)
        {
            // Find variable and assign it 
            var variableName = context.Identifier().GetText();
            Expression variableExpression = GetVariable(variableName);

            if (variableExpression == null)
            {
                throw new UndeclaredVariableException(variableName);
            }

            return VisitVariableExpression(context.expression(), variableName, variable: variableExpression, completeExpForErrorOut: context.GetText());
        }

        private Expression VisitVariableExpression(SimpleflowParser.ExpressionContext context, string variableName, Expression variable, string completeExpForErrorOut)
        {
            // TODO check for reserve keywords, liberate, context, etc...

            if (context.jsonObj() != null) 
            {
                return new SmartJsonObjectParameterExpression(context, variableName);
            }

            var expression = Visit(context.GetChild(0));
            return Expression.Assign(variable ?? Expression.Variable(expression.Type, variableName), expression);

        }
        
    }
}
