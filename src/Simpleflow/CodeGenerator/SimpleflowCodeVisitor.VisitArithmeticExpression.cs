// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {
     
        public override Expression VisitArithmeticExpression(SimpleflowParser.ArithmeticExpressionContext context)
        {
            // call visit (number) as a term
            if (context.atom()?.Number() != null)
            {
                return GetNumberExpression(context.atom().Number().GetText());
            }

            if (context.atom()?.objectIdentifier() != null)
            {
                return Visit(context.atom()?.objectIdentifier());
            }

            if (context.OpenParen() != null)
            {
                return Visit(context.arithmeticExpression().First());
            }

            if (context.PlusOp() != null)
            {
                var left = Visit(context.arithmeticExpression()[0]);
                var right = Visit(context.arithmeticExpression()[1]);

                (left, right) = ConvertToBiggerNumberType(left, right);

                return Expression.Add(left, right);
            }

            if (context.MinusOp() != null)
            {
                var left = Visit(context.arithmeticExpression()[0]);
                var right = Visit(context.arithmeticExpression()[1]);

                (left, right) = ConvertToBiggerNumberType(left, right);

                return Expression.Subtract(left, right);
            }

            if (context.TimesOp() != null)
            {
                var left = Visit(context.arithmeticExpression()[0]);
                var right = Visit(context.arithmeticExpression()[1]);

                (left, right) = ConvertToBiggerNumberType(left, right);

                return Expression.Multiply(left, right);
            }

            if (context.DivOp() != null)
            {
                var left = Visit(context.arithmeticExpression()[0]);
                var right = Visit(context.arithmeticExpression()[1]);

                (left, right) = ConvertToBiggerNumberType(left, right);

                return Expression.Divide(left, right);
            }

            // TODO InvalidArithmeticExpression
            throw new Exception("Invalid operator or expression");
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
