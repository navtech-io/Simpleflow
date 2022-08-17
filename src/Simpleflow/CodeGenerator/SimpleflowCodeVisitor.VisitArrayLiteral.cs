// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Antlr4.Runtime.Misc;

using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {
        public override Expression VisitArrayLiteral([NotNull] SimpleflowParser.ArrayLiteralContext context)
        {
            SimpleflowParser.ExpressionContext[] arrayValues = context.expression();
            var ilArrayValue = new List<Expression>(arrayValues.Length);
            Type ilArrayType = null;

            // process each value in array
            foreach (var value in arrayValues)
            {
                var ilexp = value.Accept(this);
                if (ilexp != null)
                {
                    ilArrayValue.Add(ilexp);

                    // Check for type, if type is not matched with previous one then consider it as array of objects
                    if (ilArrayType != null && ilArrayType != ilexp.Type)
                    {
                        ilArrayType = typeof(object);
                    }
                    else
                    {
                        ilArrayType = ilexp.Type;
                    }
                }
            }

            ilArrayType = ilArrayType ?? typeof(object); // This assignment if no values in array

            // Convert all of the expressions into object expressions if all the types are not same
            if (ilArrayType == typeof(object))
            {
                ilArrayValue = ilArrayValue.Select(item => Expression.Convert(item, typeof(object))).ToList<Expression>();
            }

            // Create list object
            var constructorInfo = typeof(List<>).MakeGenericType(ilArrayType)
                                                .GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(ilArrayType) });

            return Expression.New(constructorInfo, Expression.NewArrayInit(ilArrayType, ilArrayValue));
        }
    }
}
