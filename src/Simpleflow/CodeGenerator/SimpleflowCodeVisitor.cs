// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

using Simpleflow.Parser;
using Simpleflow.Exceptions;

namespace Simpleflow.CodeGenerator
{
    /// <summary>
    ///  Handles Program, Let, Mutate and Rule 
    /// </summary>
    /// <typeparam name="TArg"></typeparam>
    internal partial class SimpleflowCodeVisitor<TArg> : SimpleflowParserBaseVisitor<Expression>
    {
        protected readonly IFunctionRegister FunctionRegister;
        protected readonly LabelTarget TargetLabelToExitFunction;

        protected readonly List<ParameterExpression> Variables;  // program variables
        protected readonly List<SmartJsonObjectParameterExpression> SmartJsonVariables;  // program variables

        protected readonly ParameterExpression Input;  // script main function parameter 1
        protected readonly ParameterExpression Output; // script main function parameter 2
        protected readonly ParameterExpression ScriptHelperContext; //script main function  parameter 3

        protected readonly ParserEventPublisher EventPublisher;

        public SimpleflowCodeVisitor(IFunctionRegister functionRegister, ParserEventPublisher eventPublisher)
        {
            FunctionRegister = functionRegister ?? throw new ArgumentNullException(nameof(functionRegister));
            EventPublisher = eventPublisher;

            /* Initialize smart variables and smart json variables */
            Variables = new List<ParameterExpression>();
            SmartJsonVariables = new List<SmartJsonObjectParameterExpression>();

            /* Initialize Function parameters */
            Input = Expression.Parameter(typeof(TArg));
            Output = Expression.Parameter(typeof(FlowOutput));
            // use context parameter name in order to access in script
            ScriptHelperContext = Expression.Parameter(typeof(ScriptHelperContext), "context");


            /* A label expression of the void type that is the target for Expression.Return(). */
            TargetLabelToExitFunction = Expression.Label();
        }


        public override Expression VisitProgram(SimpleflowParser.ProgramContext context)
        {
            if (context.exception != null)
                throw context.exception;

            //Default var: arg, context
            var statementExpressions = CreateDefaultVariablesAndAssign();

            ProcessEachStatement(context, statementExpressions);

            InjectSmartVariables(statementExpressions);

            /* A label expression of the void type that is the target for Expression.Return(). */
            statementExpressions.Add(Expression.Label(TargetLabelToExitFunction));

            /* method body */
            Expression body = Expression.Block(Variables, statementExpressions);

            /* Create function with input and output parameters */
            Expression<Action<TArg, FlowOutput, ScriptHelperContext>> program =
                Expression.Lambda<Action<TArg /*input-context*/, FlowOutput, ScriptHelperContext>>(
                    body,
                    new ParameterExpression[] { Input, Output, ScriptHelperContext }
                );

            return program;
        }

        private void ProcessEachStatement(SimpleflowParser.ProgramContext context, List<Expression> statementExpressions)
        {
            for (int i = 0; i < context.ChildCount; i++)
            {
                var c = context.GetChild(i);
                var childResult = c.Accept(this);

                if (childResult != null)
                {
                    /* if current rule is variable statement then store the left expression
                       as variable identifier in variable collection */
                    if (c.GetType() == typeof(SimpleflowParser.LetStmtContext))
                    {
                        UnwrapVariableAssignment(statementExpressions, childResult);
                    }
                    else if (childResult is BlockExpression blockExpression)
                    {
                        UnwrapBlockExpression(statementExpressions, blockExpression);
                    }
                    else
                    {
                        statementExpressions.Add(childResult);
                    }
                }
            }
        }

        private void UnwrapBlockExpression(List<Expression> statementExpressions, BlockExpression blockExpression)
        {
            foreach (var item in blockExpression.Expressions)
            {
                statementExpressions.Add(item);
            }
        }

        private void UnwrapVariableAssignment(List<Expression> statementExpressions, Expression childResult)
        {
            if (childResult is BinaryExpression binaryExpression) // x = expression
            {
                DeclareAndInitializeVar(statementExpressions, binaryExpression);
            }
            else if (childResult is BlockExpression blockExpression) // to handle tuple like let a, err = expression
            {
                foreach (var item in blockExpression.Expressions)
                {
                    if (item is BinaryExpression binaryExpression1)
                    {
                        DeclareAndInitializeVar(statementExpressions, binaryExpression1);
                    }
                }
            }
            else if (childResult is SmartJsonObjectParameterExpression smartJsonParamExpression) // x = {}
            {
                CheckForDuplicateVariable(smartJsonParamExpression.Name);

                // To insert in exact location once smart variable is created
                smartJsonParamExpression.PlaceholderIndexInVariables = Variables.Count;

                // Adding this: to create a actual expression while compiling function that has used this variable.
                // This will be created while visiting the function (CreateSmartVariableIfObjectIdentiferNotDefined)
                SmartJsonVariables.Add(smartJsonParamExpression);

                // Adding this: to replace later with actual expression
                statementExpressions.Add(smartJsonParamExpression);
            }
        }

        private void DeclareAndInitializeVar(List<Expression> statementExpressions, BinaryExpression binaryExpression)
        {
            var @var = binaryExpression.Left as ParameterExpression;
            DeclareVariable(@var);
            statementExpressions.Add(binaryExpression);
        }

        private void DeclareVariable(ParameterExpression @var)
        {
            if (@var.Name != null)
            {
                CheckForDuplicateVariable(@var.Name);
            }

            Variables.Add(@var);
        }

        private ParameterExpression GetExistingOrAddVariableToGlobalScope(ParameterExpression @var)
        {
            var variable = GetVariable(@var.Name);
            if (variable != null && variable.Type != @var.Type)
            {
                throw new SimpleflowException(Resources.Message.TypeMismatchWithExistingVar);
            }

            if (variable == null)
            {
                DeclareVariable(@var);
                variable = @var;
            }
            return variable;
        }

        private void InjectSmartVariables(List<Expression> statementExpressions)
        {
            int index = 0;
            while (index < SmartJsonVariables.Count)
            {
                var sindex = statementExpressions.FindIndex(e => e is SmartJsonObjectParameterExpression);
                var item = statementExpressions[sindex] as SmartJsonObjectParameterExpression;

                // Insert JSON Object variables into main variables collection and replace assignment statement.
                if (item.VariableExpression != null)
                {
                    Variables.Insert(item.PlaceholderIndexInVariables, item.VariableExpression.Left as ParameterExpression);
                    statementExpressions[sindex] = item.VariableExpression;
                }
                else
                {
                    // Remove it has not been created because its not been used by any function. 
                    statementExpressions.RemoveAt(sindex);
                }

                index++;
            }
        }

        private void CheckForDuplicateVariable(string name)
        {
            bool anyWithGivenName = Variables.Any(v => string.Equals(v.Name, name, StringComparison.OrdinalIgnoreCase)) ||
                                    SmartJsonVariables.Any(v => string.Equals(v.Name, name, StringComparison.OrdinalIgnoreCase));

            if (anyWithGivenName)
            {
                throw new DuplicateVariableDeclarationException(name);
            }
        }


        /* Create basic set of variables to access in script */
        private List<Expression> CreateDefaultVariablesAndAssign()
        {
            var argVar = Expression.Variable(typeof(TArg), "arg");

            Variables.Add(argVar);               // arg
            Variables.Add(ScriptHelperContext);  // script

            return new List<Expression>()
            {
                Expression.Assign(argVar, Input)
            };
        }

    }
}
