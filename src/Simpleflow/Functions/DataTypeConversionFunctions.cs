// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow.Functions
{
    internal static class DataTypeConversionFunctions
    {
        public static string Str(object value) => value?.ToString() ?? string.Empty;
    }
}
