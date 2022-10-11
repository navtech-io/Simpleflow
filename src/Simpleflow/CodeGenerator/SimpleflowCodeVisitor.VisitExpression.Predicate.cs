// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Collections.Generic;
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

                case SimpleflowLexer.In:
                    return InOperatorExpression(left, right);

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
                    return Expression.AndAlso(left, right);

                case SimpleflowLexer.Or:
                    return Expression.OrElse(left, right);
            }

            return null;

        }

        public override Expression VisitNotExpression([NotNull] SimpleflowParser.NotExpressionContext context)
        {
            return Expression.Not( HandleNonBooleanExpression( Visit(context.expression()) ));
        }

        private Expression InOperatorExpression(Expression left, Expression right)
        {
            if (right.Type.GenericTypeArguments.Length == 0 
               || right.Type.IsAssignableFrom( typeof(IList<>).MakeGenericType(right.Type.GenericTypeArguments[0])))
            {
                throw new Exceptions.SimpleflowException(Resources.Message.InOperatorOnList);
            }

            // Try perform automatic conversion TODO Handle nulls
            if (right.Type.GenericTypeArguments[0] == typeof(string))
            {
                left = ToStringExpression(left);
            }
            else if (right.Type.GenericTypeArguments[0] != left.Type)
            {
                left = Expression.Convert(left, right.Type.GenericTypeArguments[0]);
            }

            return Expression.Call(
                right,
                right.Type.GetMethod("Contains", right.Type.GenericTypeArguments),
                left
            );
        }
    }
}
