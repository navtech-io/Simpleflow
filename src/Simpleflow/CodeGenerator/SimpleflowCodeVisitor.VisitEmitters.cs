// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Simpleflow.Exceptions;
using Simpleflow.Parser;

namespace Simpleflow.CodeGenerator
{
    partial class SimpleflowCodeVisitor<TArg>
    {
        public override Expression VisitMessageStmt(SimpleflowParser.MessageStmtContext context)
        {

            if (context.exception != null)
                throw context.exception;

            var messageToken = context.messageText();
            return HandleMessageText(messageToken, nameof(FlowOutput.Messages));
        }

        public override Expression VisitExitStmt(SimpleflowParser.ExitStmtContext context)
        {
            return Expression.Return(TargetLabelToExitFunction);
        }

        public override Expression VisitErrorStmt(SimpleflowParser.ErrorStmtContext context)
        {
            if (context.exception != null)
                throw context.exception;

            var messageToken = context.messageText();
            return HandleMessageText(messageToken, nameof(FlowOutput.Errors));
        }

        public override Expression VisitOutputStmt(SimpleflowParser.OutputStmtContext context)
        {
            if (context.exception != null)
                throw context.exception;

            var name = context.objectIdentifier().GetText();

            Expression callExpr = Expression.Call(
                instance: Expression.Property(OutputParam, nameof(FlowOutput.Output)),
                
                // ReSharper disable once AssignNullToNotNullAttribute
                method: typeof(Dictionary<string, object>).GetMethod(
                                nameof(Dictionary<string, object>.Add),
                                new Type[] { typeof(string), typeof(object) }),

                arg0: Expression.Constant(name),
                arg1: Expression.Convert(Visit(context.objectIdentifier()), typeof(object))
            );

            // Call function to add message 
            return callExpr;
        }

        private Expression HandleMessageText(SimpleflowParser.MessageTextContext messageToken, string outputProperty)
        {
            var identifier = Visit(messageToken.GetChild(0));

            identifier = identifier.NodeType != ExpressionType.Call && identifier.Type == typeof(string) ?
                         identifier : ToStringExpression(identifier);

            return CallListAddMethod(identifier, outputProperty);
        }

        private Expression CallListAddMethod(Expression message, string outputProperty)
        {
            Expression callExpr = Expression.Call(
                Expression.Property(OutputParam, outputProperty),
                // ReSharper disable once AssignNullToNotNullAttribute
                typeof(List<string>).GetMethod(nameof(List<string>.Add), new Type[] { typeof(string) }),
                message
            );

            // Call function to add message 
            return callExpr;
        }
    }
}
