// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

using System;

namespace Simpleflow.CodeGenerator
{
    internal enum EventType
    {
        None,
        VisitFunctionOnAvail
    }
    internal class ParserEventPublisher
    {
        public Action<EventType, object> OnVisit;

        public void Publish(EventType eventType, object data)
        {
            OnVisit(eventType, data);
        }
    }
}
