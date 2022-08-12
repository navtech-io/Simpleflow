// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Antlr4.Runtime.Misc;

using Simpleflow.Exceptions;
using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {

        // Use this field to add additional attributes to use in visitors methods

        public override Expression VisitFunction([NotNull] SimpleflowParser.FunctionContext context)
        {
            var functionName = context.FunctionName().GetText().Substring(1); // Remove $ symbol

            // Get registered function
            var function = FunctionRegister.GetFunction(functionName); 

            if (function == null)
            {
                throw new InvalidFunctionException(functionName);
            }

            // Publish event once function is available to compile
            EventPublisher.Publish(EventType.VisitFunctionOnAvail, functionName);

            // Get actual method meta-data info and parameters
            var methodInfo = function.GetMethodInfo();

            // Map script arguments to method parameters
            var argumentsExpressions = GetArgumentExpressions(context, methodInfo);

            var exp = Expression.Call(null, methodInfo, argumentsExpressions);
            return exp;
        }

        private List<Expression> GetArgumentExpressions(SimpleflowParser.FunctionContext context, MethodInfo methodInfo)
        {
            var actualMethodParameters = methodInfo.GetParameters();
            var parameters = context.functionParameter();
            var argumentsExpressions = new List<Expression>();

            if (parameters == null)
            {
                return argumentsExpressions;
            }

            CheckInvalidParameters(actualMethodParameters, parameters);
            CheckRepeatedParameters(parameters);

            foreach (var methodParameter in actualMethodParameters)
            {

                //Parameter Syntax:  ParmeterName ':' (Number|String|Bool|None|objectIdentifier)
                var scriptArgument = parameters.SingleOrDefault(parameter => string.Equals(parameter.Identifier().GetText(), methodParameter.Name, System.StringComparison.OrdinalIgnoreCase));

                if (scriptArgument == null)
                {
                    argumentsExpressions.Add(Expression.Default(methodParameter.ParameterType));
                    // AddDefaultIfMatches(argumentsExpressions, methodParameter, parameterValueContext: null);
                    continue;
                }

                var parameterExpression = CreateFunctionParameterExpression(methodParameter, scriptArgument);

                // Add result expression
                if (parameterExpression != null)
                {
                    argumentsExpressions.Add(parameterExpression);
                }
            }

            return argumentsExpressions;
        }

        private Expression CreateFunctionParameterExpression(ParameterInfo methodParameter, SimpleflowParser.FunctionParameterContext scriptArgument)
        {
            var parameterValueContext = scriptArgument.expression().GetChild(0);

            if (parameterValueContext is SimpleflowParser.ObjectIdentifierContext oic)
            {
                return VisitObjectIdentiferAsPerTargetType(oic, methodParameter.ParameterType);
            }

            return VisitWithType(parameterValueContext, methodParameter.ParameterType);
        }

        private void CheckRepeatedParameters(SimpleflowParser.FunctionParameterContext[] parameters)
        {
            var repeatedParameters = (from parameter in parameters
                                     group parameter by parameter.Identifier().GetText() into g
                                     where g.Count() > 1
                                     select g.Key).ToList();

            if (repeatedParameters.Count > 0)
            {
                throw new DuplicateParametersException(repeatedParameters);
            }
        }

        private void CheckInvalidParameters(ParameterInfo[] actualMethodParameters, SimpleflowParser.FunctionParameterContext[] parameters)
        {
            foreach (var parameter in parameters)
            {
                var paramterName = parameter.Identifier().GetText();
                if (!actualMethodParameters.Any(p => string.Equals(p.Name, paramterName, System.StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidFunctionParameterNameException(paramterName);
                }
            }
        }

        private Expression VisitObjectIdentiferAsPerTargetType(SimpleflowParser.ObjectIdentifierContext objectIdentifier, Type targetType)
        {
            var objectIdentieferText = objectIdentifier.GetText();
            
            if (objectIdentieferText.Contains(".") 
                || Variables.Any(v => string.Equals(v.Name,  objectIdentieferText, StringComparison.OrdinalIgnoreCase)))
            {
                return Visit(objectIdentifier); // regular object identifier used from variables
            }

            return CreateSmartVariableIfObjectIdentiferNotDefined(targetType, objectIdentieferText);
        }
    }
}
