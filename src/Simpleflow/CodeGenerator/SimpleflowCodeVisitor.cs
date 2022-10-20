// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Antlr4.Runtime.Tree;

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
        protected readonly List<SmartJsonObjectExpression> SmartJsonVariables;  // program variables

        protected readonly ParameterExpression InputParam;  // script main function parameter 1
        protected readonly ParameterExpression OutputParam; // script main function parameter 2
        protected readonly ParameterExpression ScriptHelperContextParam; //script main function  parameter 3

        protected readonly ParserEventPublisher EventPublisher;

        public SimpleflowCodeVisitor(IFunctionRegister functionRegister, ParserEventPublisher eventPublisher)
        {
            FunctionRegister = functionRegister ?? throw new ArgumentNullException(nameof(functionRegister));
            EventPublisher = eventPublisher;

            /* Initialize smart variables and smart json variables */
            Variables = new List<ParameterExpression>();
            SmartJsonVariables = new List<SmartJsonObjectExpression>();

            /* Initialize Function parameters */
            InputParam = Expression.Parameter(typeof(TArg));
            OutputParam = Expression.Parameter(typeof(FlowOutput));
            // use context parameter name in order to access in script
            ScriptHelperContextParam = Expression.Parameter(typeof(RuntimeContext));

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

            ReplaceVirtualSmartVariablesWithReal(statementExpressions);

            /* A label expression of the void type that is the target for Expression.Return(). */
            statementExpressions.Add(Expression.Label(TargetLabelToExitFunction));

            /* method body */
            Expression body = Expression.Block(Variables, statementExpressions);

            /* Create function with input and output parameters */
            Expression<Action<TArg, FlowOutput, RuntimeContext>> program =
                Expression.Lambda<Action<TArg /*input-context*/, FlowOutput, RuntimeContext>>(
                    body,
                    new ParameterExpression[] { InputParam, OutputParam, ScriptHelperContextParam }
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
                    // Set runtime state for debugging Simpleflow code
                    statementExpressions.Add( SetRuntimeState(c) );

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
                AddVariablesFromBlockExpression(statementExpressions, blockExpression);
            }
            else if (childResult is SmartJsonObjectExpression smartJsonParamExpression) // x = {}
            {
                AddVariablesFromSmartVarExpressions(statementExpressions, smartJsonParamExpression);
            }
            else
            {
                statementExpressions.Add(childResult);
            }
        }

        private void AddVariablesFromSmartVarExpressions(List<Expression> statementExpressions, SmartJsonObjectExpression smartJsonParamExpression)
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

        private void AddVariablesFromBlockExpression(List<Expression> statementExpressions, BlockExpression blockExpression)
        {
            foreach (var item in blockExpression.Expressions)
            {
                if (item is BinaryExpression binaryExpression1)
                {
                    DeclareAndInitializeVar(statementExpressions, binaryExpression1);
                }
                else
                {
                    statementExpressions.Add(item);
                }
            }
        }

        private void DeclareAndInitializeVar(List<Expression> statementExpressions, BinaryExpression binaryExpression)
        {
            var @var = binaryExpression.Left as ParameterExpression;
            if (@var != null) // Left expression maybe null if variable is ignored _
            {
                DeclareVariable(@var);
            }
            statementExpressions.Add(binaryExpression);
        }

        private void DeclareVariable(ParameterExpression @var)
        {
            CheckForDuplicateVariable(@var.Name);
            Variables.Add(@var);
        }

        private void CheckForDuplicateVariable(string name)
        {
            bool anyWithGivenName = Variables.Any(v => name != null && string.Equals(v.Name, name, StringComparison.OrdinalIgnoreCase)) ||
                                    SmartJsonVariables.Any(v => name != null && string.Equals(v.Name, name, StringComparison.OrdinalIgnoreCase));

            if (anyWithGivenName)
            {
                throw new DuplicateVariableDeclarationException(name);
            }
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

        private void ReplaceVirtualSmartVariablesWithReal(List<Expression> statementExpressions)
        {
            int index = 0;
            while (index < SmartJsonVariables.Count)
            {
                var sindex = statementExpressions.FindIndex(e => e is SmartJsonObjectExpression);
                var item = statementExpressions[sindex] as SmartJsonObjectExpression;

                // Insert JSON Object variables into main variables collection and replace assignment statement.
                if (item.VariableExpression != null)
                {
                    if (item.VariableExpression.Left is ParameterExpression parameter)
                    {
                        Variables.Insert(item.PlaceholderIndexInVariables, parameter);
                    }
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


        /* Create basic set of variables to access in script */
        private List<Expression> CreateDefaultVariablesAndAssign()
        {
            var argVar = Expression.Variable(typeof(TArg), "arg");
            var contextVar = Expression.Variable(typeof(RuntimeContext), "context");

            Variables.Add(argVar);      
            Variables.Add(contextVar);  

            return new List<Expression>()
            {
                Expression.Assign(argVar, InputParam),
                Expression.Assign(contextVar, ScriptHelperContextParam)
            };
        }

        private Expression SetRuntimeState(IParseTree node)
        {
            var codeLineNumber = ((Antlr4.Runtime.CommonToken)((Antlr4.Runtime.ParserRuleContext)node).Start).Line;

            var property = typeof(RuntimeContext)
                           .GetProperty("LineNumber", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var propertyExpression = Expression.Property(ScriptHelperContextParam, property);
            return Expression.Assign(propertyExpression, Expression.Constant(codeLineNumber));
        }

    }
}
