// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Linq.Expressions;

using Antlr4.Runtime;
using FastExpressionCompiler;

using Simpleflow.Exceptions;
using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    internal class SimpleflowCompiler
    {

        public static Action<FlowInput<TArg>, FlowOutput, ScriptHelperContext> Compile<TArg>(string code, 
            IFunctionRegister activityRegister, 
            ParserEventPublisher eventPublisher)
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

            // Find error if any detected
            if (errorListener.Errors.Count > 0)
            {
                throw new SyntaxException(errorListener.GetAggregateMessages(), errorListener.Errors);
            }

            
            // Generate code
            var visitor = new SimpleflowCodeVisitor<TArg>(activityRegister, eventPublisher);
            var program = visitor.Visit(programContext);

            // Compile
            var programExpression = (Expression<Action<FlowInput<TArg>, FlowOutput, ScriptHelperContext>>)program;
            return programExpression.CompileFast();
        }
    }
}
