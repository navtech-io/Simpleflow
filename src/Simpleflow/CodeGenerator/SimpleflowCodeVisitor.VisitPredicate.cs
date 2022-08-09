// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Linq;
using System.Linq.Expressions;

using Antlr4.Runtime.Tree;

using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {

        public override Expression VisitPredicate(SimpleflowParser.PredicateContext context)
        {
            if (context.exception != null)
                throw context.exception;

            // OpenParen predicate CloseParen
            if (context.OpenParen() != null)
            {
                return Visit(context.predicate().First());
            }

            if (context.Not() != null)
            {
                return Expression.Not(Visit(context.predicate().First()));
            }

            // predicate booleanOperator predicate
            if (context.logicalOperator() != null)
            {
                return VisitLogicalOperatorInternal(context.logicalOperator(), context.predicate()[0], context.predicate()[1]);
            }

            // operand operator operand
            if (context.testExpression() != null)
            {
                return Visit(context.testExpression());
            }

            if (context.unaryOperand() != null)
            {
                return Visit(context.unaryOperand());
            }

            throw new Exception("Unhandled Predicate");

        }

        public override Expression VisitUnaryOperand(SimpleflowParser.UnaryOperandContext context)
        {
            var operandExpression = Visit(context.GetChild(0));
            if (operandExpression.Type == typeof(bool))
            {
                return Expression.Equal(operandExpression, Expression.Constant(true));
            }
            return Expression.NotEqual(operandExpression, Expression.Default(operandExpression.Type));
        }

        public override Expression VisitTestExpression(SimpleflowParser.TestExpressionContext context)
        {
            var left = context.operand()[0];
            var right = context.operand()[1];

            var symbolNode = (ITerminalNode)context.relationalOperator().GetChild(0);
            var symbolType = symbolNode.Symbol.Type;

            switch (symbolType)
            {
                case SimpleflowLexer.Equal:
                    return Expression.Equal(Visit(left), Visit(right));

                case SimpleflowLexer.NotEqual:
                    return Expression.NotEqual(Visit(left), Visit(right));

                case SimpleflowLexer.GreaterThan:
                    return Expression.GreaterThan(Visit(left), Visit(right));

                case SimpleflowLexer.GreaterThanEqual:
                    return Expression.GreaterThanOrEqual(Visit(left), Visit(right));

                case SimpleflowLexer.LessThan:
                    return Expression.LessThan(Visit(left), Visit(right));

                case SimpleflowLexer.LessThanEqual:
                    return Expression.LessThanOrEqual(Visit(left), Visit(right));
            }
            return base.VisitTestExpression(context);
        }

        public override Expression VisitOperand(SimpleflowParser.OperandContext context)
        {
            return Visit(context.GetChild(0));
        }

        public Expression VisitLogicalOperatorInternal(SimpleflowParser.LogicalOperatorContext context,
            SimpleflowParser.PredicateContext leftContext,
            SimpleflowParser.PredicateContext rightContext)
        {
            var symbolNode = (ITerminalNode)context.GetChild(0);
            var symbolType = symbolNode.Symbol.Type;

            switch (symbolType)
            {
                case SimpleflowLexer.And:
                    return Expression.And(Visit(leftContext), Visit(rightContext));
                case SimpleflowLexer.Or:
                    return Expression.Or(Visit(leftContext), Visit(rightContext));
            }

            return base.VisitLogicalOperator(context);
        }
    }
}
