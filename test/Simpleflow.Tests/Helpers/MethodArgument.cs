// Copyright (c) navtech.io. All rights reserved.
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

        public Permission Permission { get; set; }

        public override string ToString()
        {
            return $"{Id}-{Value}-{Text}-{IsValid}-{NullCheck ?? "NULL"}-{CheckIdentifer}";
        }
    }

    public enum Permission
    {
        None,
        Read,
        Write,
        ReadWrite
    }

    public class MethodSuperArgument
    {
        public string Uid { get; set; }
        public MethodArgument Child { get; set; }
        public MethodArgument Child2 { get; set; }

        public override string ToString()
        {
            return $"{Uid}-{Child}";
        }
    }
}
