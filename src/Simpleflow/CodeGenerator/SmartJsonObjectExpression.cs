// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Linq.Expressions;

using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    internal class SmartJsonObjectExpression : Expression 
    {
        public readonly SimpleflowParser.JsonObjLiteralExpressionContext Context;

        public BinaryExpression VariableExpression;
        public int PlaceholderIndexInVariables;

        public readonly string Name;

        public SmartJsonObjectExpression(SimpleflowParser.JsonObjLiteralExpressionContext context, string variableName)
        {
            Name = variableName;
            Context = context;
        }
    
    }
}
