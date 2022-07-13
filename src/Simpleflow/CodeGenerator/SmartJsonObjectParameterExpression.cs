// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Linq.Expressions;

using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    internal class SmartJsonObjectParameterExpression : Expression 
    {
        public readonly SimpleflowParser.ExpressionContext Context;

        public BinaryExpression VariableExpression;
        public int PlaceholderIndexInVariables;

        public readonly string Name;

        public SmartJsonObjectParameterExpression(SimpleflowParser.ExpressionContext context, string variableName)
        {
            Name = variableName;
            Context = context;
        }

    
    }
}
