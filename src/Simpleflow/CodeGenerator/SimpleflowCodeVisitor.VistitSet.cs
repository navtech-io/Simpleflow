// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Simpleflow.Exceptions;
using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {
        // Mutate statement
        public override Expression VisitSetStmt(SimpleflowParser.SetStmtContext context)
        {
            // Find variable and assign it 
            string variableName = context.IgnoreIdentifier() != null
                                  ? null
                                  : context.Identifier()[0].GetText();

            // Assume that SmartVariable has already created as part of function invocation if available
            Expression variableExpression = variableName != null
                                            ? GetVariable(variableName) ?? GetSmartVariable(variableName)?.VariableExpression?.Left
                                            : null;

            if (variableName != null && variableExpression == null)
            {
                throw new UndeclaredVariableException(variableName);
            }

            // Get indexed object if defined as left expression
            variableExpression = GetIndexObjectExpIfDefined(variableExpression, context.index());

            // Get value expression as right one
            Expression valueExpression = GetValueExpressionOfSetStmt(context, variableName, ref variableExpression);

            // return expression
            return GetSetStmtExpression(context, variableName, variableExpression, valueExpression);

        }

        private Expression GetValueExpressionOfSetStmt(SimpleflowParser.SetStmtContext context, 
                                                       string variableName, /* Pass variable name for discardable cheking */
                                                       ref Expression variableExpression)
        {
            var expression = context.expression();
            Expression rightSideSetExpression;

            if (context.Partial() != null) // visit complex type partially 
            {
                if (expression.jsonObj() == null)
                {
                    throw new SimpleflowException(Resources.Message.InvalidPartialKeywordUsage);
                }

                if (variableName == null)
                {
                    throw new SimpleflowException(Resources.Message.CannotIgnoreIdentifierForJsonObj);
                }

                rightSideSetExpression = VisitPartialSet(context, variableExpression);

                /* Set variableExpression  is null, because it does not require to assign for partial variable, 
                * since it changes partially already declared object by setting properties of it instead of replacing entire object.
                * 
                * GetSetExpression / AssignWithHandlingError will not assign if variableExpression is null
                */
                variableExpression = null;
            }

            else if (expression.jsonObj() != null) // visit complex type fully - complete replace of reference -- 
            {
                if (variableName == null)
                {
                    throw new SimpleflowException(Resources.Message.CannotIgnoreIdentifierForJsonObj);
                }
                rightSideSetExpression = CreateNewEntityInstance(variableExpression.Type, expression.jsonObj().pair());
            }

            else // visit simple type
            {
                rightSideSetExpression = Visit(expression.GetChild(0));
            }

            return rightSideSetExpression;
        }

        private Expression GetSetStmtExpression(SimpleflowParser.SetStmtContext context, string variableName, Expression variableExpression, Expression rightSideSetExpression)
        {
            if (variableName == null && context.Identifier().Length == 1) // Ignore variable with error handler variable
            {
                return AssignWithHandlingError(variableExpression, rightSideSetExpression, context.Identifier()[0].GetText());
            }
            else if (variableName != null && context.Identifier().Length == 2) // variable with error handler variable
            {
                return AssignWithHandlingError(variableExpression, rightSideSetExpression, context.Identifier()[1].GetText());
            }
            else if (variableName == null || variableExpression == null) // Ignore variable and with no error handler variable
            {
                return rightSideSetExpression;
            }
            else // variable and with no error handler variable
            {
                return Expression.Assign(variableExpression, rightSideSetExpression);
            }
        }

        private Expression AssignWithHandlingError(Expression variable, Expression rightSideSetExpression, string errorVariable)
        {

            var tryExpression = AddTryCatchToExpression(rightSideSetExpression);

            // Add error variable without declare using let, error is exceptional case
            var errorVar = GetExistingOrAddVariableToGlobalScope(Expression.Variable(typeof(Exception), errorVariable));
            var varFortryExpression = Expression.Variable(tryExpression.Type);

            // Declare before use
            DeclareVariable(varFortryExpression);

            // Add variables
            if (variable == null)
            {
                return Expression.Block(
                     Expression.Assign(varFortryExpression, tryExpression), // run expression with try catch and capture value
                     Expression.Assign(errorVar, Expression.Field(varFortryExpression, "Error"))
               );
            }

            return Expression.Block(
                        Expression.Assign(varFortryExpression, tryExpression), // run expression with try catch and capture value
                        Expression.Assign(variable, Expression.Field(varFortryExpression, "Value")),
                        Expression.Assign(errorVar, Expression.Field(varFortryExpression, "Error"))
                  );
        }

        private Expression VisitPartialSet(SimpleflowParser.SetStmtContext context, Expression variable)
        {
            var pairs = context.expression().jsonObj().pair();
            List<Expression> propExpressions = new List<Expression>();

            // set values to each declared property
            BindProperties(variable.Type,
                                 pairs,
                                 (propInfo, valueExp) =>
                                        propExpressions.Add(Expression.Assign(Expression.Property(variable, propInfo), valueExp)));

            return Expression.Block(propExpressions);
                   
        }
    }
}
