// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Linq.Expressions;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {

        public override Expression VisitMultiplicativeExpression([NotNull] SimpleflowParser.MultiplicativeExpressionContext context)
        {
            var symbolNode = (ITerminalNode)context.GetChild(1); // exp(0) operator(1) exp(2)
            var symbolType = symbolNode.Symbol.Type;

            var left = Visit(context.expression()[0]);
            var right = Visit(context.expression()[1]);
            (left, right) = ConvertToBiggerNumberType(left, right);

            switch (symbolType)
            {
                case SimpleflowLexer.TimesOp:
                    return Expression.Multiply(left, right);


                case SimpleflowLexer.DivOp:
                    return Expression.Divide(left, right);

                case SimpleflowLexer.ModuloOp:
                    return Expression.Modulo(left, right);
            }

            return null;
        }

        public override Expression VisitAdditiveExpression([NotNull] SimpleflowParser.AdditiveExpressionContext context)
        {
            var symbolNode = (ITerminalNode)context.GetChild(1); // exp(0) operator(1) exp(2)
            var symbolType = symbolNode.Symbol.Type;

            var left = Visit(context.expression()[0]);
            var right = Visit(context.expression()[1]);
            (left, right) = ConvertToBiggerNumberType(left, right);

            switch (symbolType)
            {
                case SimpleflowLexer.PlusOp:
                    return Expression.Add(left, right);


                case SimpleflowLexer.MinusOp:
                    return Expression.Subtract(left, right);
            }

            return null;
        }

        public override Expression VisitParenthesizedExpression([NotNull] SimpleflowParser.ParenthesizedExpressionContext context)
        {
            return Visit(context.GetChild(1)); // letft_paren(0) expression(1) right_paren(2)
        }

        private (Expression left, Expression right) ConvertToBiggerNumberType(Expression left, Expression right)
        {
            if (left.Type == typeof(int) && right.Type == typeof(decimal))
            {
                return (Expression.Convert(left, right.Type), right);
            }
            if (left.Type == typeof(decimal) && right.Type == typeof(int))
            {
                return (left, Expression.Convert(right, left.Type));
            }
            return (left, right);
        }
    }
}
