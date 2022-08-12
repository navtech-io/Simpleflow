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
                       Expression.New(varTupleConstructor, ex)
                   )
               );

            return tryCatchExpr;
        }
    }
}
