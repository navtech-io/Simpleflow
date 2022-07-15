// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Linq;
using System.Linq.Expressions;

using Antlr4.Runtime.Tree;

using Simpleflow.Exceptions;
using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {
        public override Expression VisitObjectIdentifier(SimpleflowParser.ObjectIdentifierContext context)
        {
            var objectPathProperties = context.GetText().Split('.');
            
            var propName = objectPathProperties[0];
            Expression identifier = GetVariable(propName) ?? GetSmartVariable(propName)?.VariableExpression?.Left;

            if (identifier == null)
            {
                throw new UndeclaredVariableException(objectPathProperties[0]);
            }

            return GetEndProperty(identifier, objectPathProperties, startIndex: 1);
        }
    }
}
