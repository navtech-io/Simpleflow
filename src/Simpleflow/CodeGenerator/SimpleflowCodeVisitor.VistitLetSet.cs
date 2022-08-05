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
            var letIdentifiers = context.Identifier();
            var letIdentifier = letIdentifiers[0].GetText();

            // Validate variable name
            if (SimpleflowKeywords.Keywords.Any(keyword => string.Equals(keyword, letIdentifier, StringComparison.Ordinal)))
            {
                throw new VariableNameViolationException(letIdentifier);

            }

            return GetLetVariableExpression(expression,
                                            letIdentifier,
                                            errorVariableName:
                                                    letIdentifiers.Length == 2 ? letIdentifiers[1].GetText() : null);

        }

        // Mutate statement
        public override Expression VisitSetStmt(SimpleflowParser.SetStmtContext context)
        {
            // Find variable and assign it 
            var variableName = context.Identifier()[0].GetText();

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

            return
                context.Identifier().Length == 2 
                 ? AssignWithHandlingError(variableExpression, rightSideSetExpression, context.Identifier()[1].GetText())
                 : Expression.Assign(variableExpression, rightSideSetExpression) ;
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
            return Expression.Block(
                        Expression.Assign(varFortryExpression, tryExpression), // run expression with try catch and capture value
                        Expression.Assign(variable, Expression.Field(varFortryExpression, "Value")),
                        Expression.Assign(errorVar, Expression.Field(varFortryExpression, "Error"))
                  );
        }

        private TryExpression AddTryCatchToExpression(Expression rightsideExpression)
        {
            var varTupleConstructor = typeof(VarTuple<>)
                                          .MakeGenericType(rightsideExpression.Type)
                                          .GetConstructor(new Type[] { rightsideExpression.Type, typeof(Exception) });

            ParameterExpression ex = ParameterExpression.Parameter(typeof(Exception));
            TryExpression tryCatchExpr =
                Expression.TryCatch(
                        Expression.New(varTupleConstructor,
                                       rightsideExpression, // it may throw
                                       Expression.Constant(null, typeof(Exception))
                    ),
                    Expression.Catch(
                        ex,
                        Expression.Block(
                            Expression.New(varTupleConstructor,
                                          Expression.Default(rightsideExpression.Type),
                                          ex))
                    )
                );

            return tryCatchExpr;
        }

        private Expression GetLetVariableExpression(SimpleflowParser.ExpressionContext context, string variableName, string errorVariableName)
        {
            if (context.jsonObj() != null)
            {
                return new SmartJsonObjectParameterExpression(context, variableName);
            }
            var expression = Visit(context.GetChild(0));

            // if there's a error variable not declared then return value, else catch it and return
            if (string.IsNullOrWhiteSpace(errorVariableName))
            {
                return Expression.Assign(Expression.Variable(expression.Type, variableName), expression);
            }
            else
            {
                var tryExpression = AddTryCatchToExpression(expression);
                var varFortryExpression = Expression.Variable(tryExpression.Type);

                // Add variables
                return Expression.Block(
                            Expression.Assign(varFortryExpression, tryExpression), // run expression with try catch and capture value
                            Expression.Assign(Expression.Variable(expression.Type, variableName), Expression.Field(varFortryExpression, "Value")),
                            Expression.Assign(Expression.Variable(typeof(Exception), errorVariableName), Expression.Field(varFortryExpression, "Error"))
                      );
            }
        }
    }
}
