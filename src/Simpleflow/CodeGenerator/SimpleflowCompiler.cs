// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FastExpressionCompiler;
using Antlr4.Runtime;

using Simpleflow.Exceptions;
using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    internal class SimpleflowCompiler
    {
        internal static Action<TArg, FlowOutput, RuntimeContext> Compile<TArg>(string code, 
            IFunctionRegister activityRegister, 
            ParserEventPublisher eventPublisher)
        {
            var (programContext, errors) = ParseAndGetProgramContext(code);

            // Find error if any detected
            if (errors.Count > 0)
            {
                throw new SyntaxException(GetAggregateMessages(errors), errors);
            }

            // Generate code
            var visitor = new SimpleflowCodeVisitor<TArg>(activityRegister, eventPublisher);
            var program = visitor.Visit(programContext);

            // Compile
            var programExpression = (Expression<Action<TArg, FlowOutput, RuntimeContext>>)program;
            return programExpression.CompileFast();
        }

        internal static (SimpleflowParser.ProgramContext, List<SyntaxError>) ParseAndGetProgramContext(string code)
        {
            var inputStream = new AntlrInputStream(code);

            // Tokenize
            var simpleflowLexer = new SimpleflowLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(simpleflowLexer);

            // Create Parser
            var simpleflowParser = new SimpleflowParser(commonTokenStream)
            {
                BuildParseTree = true
            };

            // Add error listener
            simpleflowParser.RemoveErrorListeners();
            var errorListener = new SimpleflowErrorListener();
            simpleflowParser.AddErrorListener(errorListener);

            // Parse
            SimpleflowParser.ProgramContext programContext = simpleflowParser.program();

            return (programContext, errorListener.Errors);
        }

        internal static string GetAggregateMessages(List<SyntaxError> syntaxErrors)
        {
            return string.Join(";\r\n", syntaxErrors);
        }
    }
}
