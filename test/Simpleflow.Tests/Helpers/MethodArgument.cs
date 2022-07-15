﻿// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow.Tests.Helpers
{
    public class MethodArgument
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public string Text { get; set; }
        public bool IsValid { get; set; }
        public string NullCheck { get; set; }
        public int CheckIdentifer { get; set; }
        public override string ToString()
        {
            return $"{Id}-{Value}-{Text}-{IsValid}-{NullCheck ?? "NULL"}-{CheckIdentifer}";
        }
    }
}
