// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Linq.Expressions;
using System.Collections.Generic;

using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {
        // Rule Statement 
        public override Expression VisitRuleStmt(SimpleflowParser.RuleStmtContext context)
        {
            if (context.exception != null)
                throw context.exception;

            var testExpression = Visit(context.expression());
            var statements = new List<Expression>();

            // Process predicate and statements
            foreach (var statement in context.generalStatement())
            {
                var c = statement.GetChild(0);
                var childResult = c.Accept(this);

                if (childResult != null)
                {
                    statements.Add(SetRuntimeState(c));
                    statements.Add(childResult);
                }
            }

            testExpression = HandleNonBooleanExpression(testExpression);

            return Expression.IfThen(testExpression, Expression.Block(statements));
        }

        
    }
}
