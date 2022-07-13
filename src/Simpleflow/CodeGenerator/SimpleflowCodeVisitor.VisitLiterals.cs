// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Linq.Expressions;

using Antlr4.Runtime.Misc;

using Simpleflow.Exceptions;
using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {

        // Use this field to add additional attributes to use in visitors methods
        Antlr4.Runtime.Tree.ParseTreeProperty<Type> TargetTypeParserContextAnnotation = new Antlr4.Runtime.Tree.ParseTreeProperty<Type>();

        public override Expression VisitNumberLiteral([NotNull] SimpleflowParser.NumberLiteralContext context)
        {
            var targetType = TargetTypeParserContextAnnotation.Get(context);

            if (targetType == null)
            {
                return GetNumberExpression(context.Number().GetText());
            }

            return GetNumberExpression(context.Number().GetText(), targetType);
        }

        public override Expression VisitBoolLeteral([NotNull] SimpleflowParser.BoolLeteralContext context)
        {
            var targetType = TargetTypeParserContextAnnotation.Get(context);

            var value = context.GetText();

            if (targetType == null || targetType == typeof(bool))
            {
                return GetBoolExpression(value);
            }

            throw new ValueTypeMismatchException(value);
        }

        public override Expression VisitStringLiteral([NotNull] SimpleflowParser.StringLiteralContext context)
        {
            var targetType = TargetTypeParserContextAnnotation.Get(context);

            var value = context.String().GetText();

            if (targetType == null || targetType == typeof(string))
            {
                return GetStringExpression(value);
            }

            throw new ValueTypeMismatchException(value);
        }

        public override Expression VisitNoneLiteral([NotNull] SimpleflowParser.NoneLiteralContext context)
        {
            var targetType = TargetTypeParserContextAnnotation.Get(context);

            if (targetType != null)
            {
                return Expression.Default(targetType);
            }
            return Expression.Default(typeof(object));
        }
    }
}
