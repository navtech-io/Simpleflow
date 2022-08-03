// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
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

        public override Expression VisitTemplateStringLiteral([NotNull] SimpleflowParser.TemplateStringLiteralContext context)
        {
            // Create array of string expressions
            // to pass string.concat method
            
            List<Expression> expressions = new List<Expression>();

            
            StringBuilder sb = new StringBuilder();
            int index = 1; // First and last characters are back ticks

            while(index < context.children.Count-1)
            {
                var child = context.children[index];

                if (child.GetChild(1) is SimpleflowParser.ObjectIdentifierContext)
                {
                    if (sb.Length > 0)
                    {
                        expressions.Add(Expression.Constant(sb.ToString()));
                        sb.Clear();
                    }
                    // evaluate
                    var expression = child.GetChild(1).Accept(this);
                    if (expression != null)
                    {
                        expression = expression.Type != typeof(string) ? ToStringExpression(expression) : expression;
                        expressions.Add(expression);
                    }
                }
                else
                {
                    sb.Append(child.GetText());
                }

                index++;
            }

            // Append last part if available
            if (sb.Length > 0)
            {
                expressions.Add(Expression.Constant(sb.ToString()));
                sb.Clear();
            }

            NewArrayExpression newArrayExpression =   Expression.NewArrayInit(typeof(string), expressions);
            var concatMethod = typeof(string).GetMethods().Where(m => m.Name == "Concat" && m.GetParameters()[0].ParameterType == typeof(IEnumerable<String>)).Single() ;

            return Expression.Call(concatMethod, newArrayExpression);
        }
    }
}
