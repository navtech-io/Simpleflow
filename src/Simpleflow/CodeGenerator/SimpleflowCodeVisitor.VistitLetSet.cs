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


            var letIdentifierList = context.Identifier();
            var letIdentifier = context.IgnoreIdentifier() != null ? null : letIdentifierList[0].GetText();

            // Validate variable name with reserved words
            if (SimpleflowKeywords.Keywords.Any(keyword => string.Equals(keyword, letIdentifier, StringComparison.Ordinal)))
            {
                throw new VariableNameViolationException(letIdentifier);
            }

            return GetLetVariableExpression(context.expression(),
                                            letIdentifier,
                                            errorVariableName: context.IgnoreIdentifier() != null &&
                                                               letIdentifierList.Length == 1
                                                               ? letIdentifierList[0].GetText()
                                                               : letIdentifierList.Length == 2
                                                                    ? letIdentifierList[1].GetText()
                                                                    : null);
        }

        // Mutate statement
        public override Expression VisitSetStmt(SimpleflowParser.SetStmtContext context)
        {
            // Find variable and assign it 
            var variableName = context.IgnoreIdentifier() != null
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

            var rightSideSetExpression = GetRightSideExpressionOfSetStmt(context, variableName, ref variableExpression);

            // return expression
            return GetSetStmtExpression(context, variableName, variableExpression, rightSideSetExpression);

        }

        private Expression GetRightSideExpressionOfSetStmt(SimpleflowParser.SetStmtContext context, string variableName, ref Expression variableExpression)
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
                rightSideSetExpression = CreateNewInstanceWithProps(variableExpression.Type, expression.jsonObj().pair());
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

        private TryExpression AddTryCatchToExpression(Expression rightsideExpression)
        {
            if (rightsideExpression.Type == typeof(void))
            {
                return AddTryCatchToExpressionForVoidValue(rightsideExpression);
            }

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
                        Expression.New(varTupleConstructor,
                                       Expression.Default(rightsideExpression.Type), 
                                       ex)
                    )
                );

            return tryCatchExpr;
        }

        private TryExpression AddTryCatchToExpressionForVoidValue(Expression rightsideExpression)
        {
            var varTupleConstructor = typeof(VarTuple).GetConstructor(new Type[] { typeof(Exception) });

            ParameterExpression ex = ParameterExpression.Parameter(typeof(Exception));

            TryExpression tryCatchExpr =
               Expression.TryCatch(
                    Expression.Block(
                        rightsideExpression, // it may throw
                        Expression.New(varTupleConstructor,
                                       Expression.Constant(null, typeof(Exception))
                        )
                    ),
                   Expression.Catch(
                       ex,
                       Expression.New(varTupleConstructor,ex)
                   )
               );

            return tryCatchExpr;
        }


        private Expression GetLetVariableExpression(SimpleflowParser.ExpressionContext context, string variableName, string errorVariableName)
        {
            if (context.jsonObj() != null)
            {
                if (variableName == null)
                {
                    throw new SimpleflowException(Resources.Message.CannotIgnoreIdentifierForJsonObj);
                }

                return new SmartJsonObjectParameterExpression(context, variableName);
            }
            var expression = Visit(context.GetChild(0));

            // if there's a error variable not declared then return value, else catch it and return
            if (string.IsNullOrWhiteSpace(errorVariableName))
            {
                if (variableName == null) //variableName is null means Ignore variable
                {
                    return expression;
                }
                else
                {
                    return Expression.Assign(Expression.Variable(expression.Type, variableName), expression);
                }
            }
            else
            {
                // add try catch if there's error variable defined
                var tryExpression = AddTryCatchToExpression(expression);
                var varFortryExpression = Expression.Variable(tryExpression.Type);

                // Add variables
                if (variableName != null)
                {
                    return Expression.Block(
                            Expression.Assign(varFortryExpression, tryExpression), // run expression with try catch and capture value
                            Expression.Assign(Expression.Variable(expression.Type, variableName), Expression.Field(varFortryExpression, "Value")),
                            Expression.Assign(Expression.Variable(typeof(Exception), errorVariableName), Expression.Field(varFortryExpression, "Error"))
                      );
                }

                //  assign error only, not regular variable
                return Expression.Block(
                           Expression.Assign(varFortryExpression, tryExpression), // run expression with try catch and capture value
                           Expression.Assign(Expression.Variable(typeof(Exception), errorVariableName), Expression.Field(varFortryExpression, "Error"))
                     );
            }
        }
    }
}
