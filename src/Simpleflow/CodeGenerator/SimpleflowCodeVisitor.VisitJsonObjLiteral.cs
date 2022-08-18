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
        public override Expression VisitJsonObjLiteral([NotNull] SimpleflowParser.JsonObjLiteralContext context)
        {
            var type = TargetTypeParserContextAnnotation.Get(context);
            return CreateNewEntityInstance(type, context.pair());
        }
        
        private Expression CreateNewEntityInstance(Type targetType, SimpleflowParser.PairContext[] pairs)
        {
            // if targetType is ExpandoObject
            return ModelBinder(targetType, pairs);
        }
    }
}
