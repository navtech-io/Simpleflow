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

            var argumentNames = context.functionArguments()
                                       .functionArgument()
                                       .Select(arg => new ArgumentInfo { ArgumentName = arg.Identifier().GetText() })
                                       .ToArray();

            // Get registered function
            var function = FunctionRegister.GetFunction(functionName, argumentNames); 

            if (function == null)
            {
                throw new InvalidFunctionException(functionName);
            }

            // Publish event once function is available to compile
            EventPublisher.Publish(EventType.VisitFunctionOnAvail, functionName);

            // Get actual method meta-data info and parameters
            var methodInfo = function.Reference.GetMethodInfo();

            // Map script arguments to method parameters
            var argumentsExpressions = GetArgumentExpressions(context, methodInfo);

            var exp = Expression.Call(null, methodInfo, argumentsExpressions);
            return exp;
        }

        private List<Expression> GetArgumentExpressions(SimpleflowParser.FunctionContext context, MethodInfo methodInfo)
        {
            var actualMethodParameters = methodInfo.GetParameters();
            var arguments = context.functionArguments().functionArgument();
            var argumentsExpressions = new List<Expression>();
            
            if (arguments == null)
            {
                return argumentsExpressions;
            }

            CheckInvalidParameters(actualMethodParameters, arguments, context.FunctionName().GetText());
            CheckRepeatedParameters(arguments);

            foreach (var methodParameter in actualMethodParameters)
            {

                //Parameter Syntax:  ParmeterName ':' (Number|String|Bool|None|objectIdentifier)
                var funcArgument = arguments.SingleOrDefault(parameter => string.Equals(parameter.Identifier().GetText(), methodParameter.Name, System.StringComparison.OrdinalIgnoreCase));

                if (funcArgument == null)
                {
                    argumentsExpressions.Add(Expression.Default(methodParameter.ParameterType));
                    // AddDefaultIfMatches(argumentsExpressions, methodParameter, parameterValueContext: null);
                    continue;
                }

                var parameterExpression = CreateFunctionArgumentExpression(methodParameter, funcArgument);

                // Add result expression
                if (parameterExpression != null)
                {
                    argumentsExpressions.Add(parameterExpression);
                }
            }

            return argumentsExpressions;
        }

        private Expression CreateFunctionArgumentExpression(ParameterInfo methodParameter, SimpleflowParser.FunctionArgumentContext funcArgument)
        {
            var argValueExp = funcArgument.expression();

            if (argValueExp is SimpleflowParser.ObjectIdentiferExpressionContext oic)
            {
                return VisitObjectIdentiferAsPerTargetType(oic, methodParameter.ParameterType);
            }

            return VisitWithType(argValueExp, methodParameter.ParameterType);
        }

        private void CheckRepeatedParameters(SimpleflowParser.FunctionArgumentContext[] parameters)
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

        private void CheckInvalidParameters(ParameterInfo[] actualMethodParameters, 
                                            SimpleflowParser.FunctionArgumentContext[] parameters, 
                                            string functionName)
        {
            foreach (var parameter in parameters)
            {
                var paramterName = parameter.Identifier().GetText();
                if (!actualMethodParameters.Any(p => string.Equals(p.Name, paramterName, System.StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidFunctionParameterNameException(paramterName, functionName);
                }
            }
        }

        private Expression VisitObjectIdentiferAsPerTargetType(SimpleflowParser.ObjectIdentiferExpressionContext objectIdentifier, Type targetType)
        {
            var objectIdentieferText = objectIdentifier.GetText();

            if (   objectIdentieferText.Contains(".") // accessing property
                || objectIdentifier.objectIdentifier()?.identifierIndex()[0]?.index() != null
                || Variables.Any(v => string.Equals(v.Name,  objectIdentieferText, StringComparison.OrdinalIgnoreCase)))
            {
                return Visit(objectIdentifier); // regular object identifier used from variables
            }

            return CreateSmartVariableIfObjectIdentiferNotDefined(targetType, objectIdentieferText);
        }

        private Expression CreateSmartVariableIfObjectIdentiferNotDefined(Type targetType, string name)
        {
            // Variable name is not case sensitive
            var smartVar = GetSmartVariable(name);

            if (smartVar == null)
            {
                // Since smart variable (JSON) can only be used with function argument, 
                // so here we need to throw function related exception
                throw new InvalidFunctionParameterNameException($"Invalid parameter or variable '{name}'");
            }

            // Return if already created
            if (smartVar.VariableExpression != null)
            {
                return smartVar.VariableExpression;
            }

            var instanceExpressionWithMembers = CreateNewEntityInstance(targetType, smartVar.Context.jsonObjLiteral().pair());

            // Store created smart variable to further reuse and replace.
            return smartVar.VariableExpression = Expression.Assign(Expression.Variable(targetType), instanceExpressionWithMembers);

        }

    }
}
