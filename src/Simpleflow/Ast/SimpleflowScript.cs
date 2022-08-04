// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using Simpleflow.CodeGenerator;

namespace Simpleflow.Ast
{

    public static class SimpleflowScript
    {
        public static SyntaxTree GetAbstractSyntaxTree(string code)
        {
            var (programContext, errors) = SimpleflowCompiler.ParseAndGetProgramContext(code);

            // Generate code
            var visitor = new SimpleflowAstVisitor();
            var program = visitor.Visit(programContext);

            return new SyntaxTree { Children = program.Children, Type = "program", SyntaxErrors = errors };
        }
    }
}
