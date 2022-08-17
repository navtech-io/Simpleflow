// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {
        public override Expression VisitRelationalExpression([NotNull] SimpleflowParser.RelationalExpressionContext context)
        {
            var symbolNode = (ITerminalNode)context.GetChild(1); // left-exp-0 operator-1 right-exp-2
            var symbolType = symbolNode.Symbol.Type;

            var left = Visit(context.expression()[0]);
            var right = Visit(context.expression()[1]);

            switch (symbolType)
            {
                case SimpleflowLexer.Equal:
                    return Expression.Equal(left, right);

                case SimpleflowLexer.NotEqual:
                    return Expression.NotEqual(left, right);

                case SimpleflowLexer.GreaterThan:
                    return Expression.GreaterThan(left, right);

                case SimpleflowLexer.GreaterThanEqual:
                    return Expression.GreaterThanOrEqual(left, right);

                case SimpleflowLexer.LessThan:
                    return Expression.LessThan(left, right);

                case SimpleflowLexer.LessThanEqual:
                    return Expression.LessThanOrEqual(left, right);
            }
            return null;
        }

        public override Expression VisitLogicalExpression([NotNull] SimpleflowParser.LogicalExpressionContext context)
        {
            var symbolNode = (ITerminalNode)context.GetChild(1); // left-exp-0 operator-1 right-exp-2
            var symbolType = symbolNode.Symbol.Type;

            var left = Visit(context.expression()[0]);
            var right = Visit(context.expression()[1]);


            switch (symbolType)
            {
                case SimpleflowLexer.And:
                    return Expression.And(left, right);

                case SimpleflowLexer.Or:
                    return Expression.Or(left, right);
            }

            return null;

        }

        public override Expression VisitNotExpression([NotNull] SimpleflowParser.NotExpressionContext context)
        {
            return Expression.Not( HandleNonBooleanExpression( Visit(context.expression()) ));
        }
    }
}
