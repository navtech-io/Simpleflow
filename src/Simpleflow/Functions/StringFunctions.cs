// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow.Functions
{
    internal static class StringFunctions
    {
        public static bool Contains(string input, string value ) => input?.Contains(value) ?? false;
        public static bool StartsWith(string input, string value) => input?.StartsWith(value) ?? false;
        public static bool EndsWith(string input, string value) => input?.EndsWith(value) ?? false;
        public static string Trim(string input, string value) => input?.Trim(value.ToCharArray()) ?? input;
        public static string Substring(string input, int startIndex, int length ) => length == 0 ? input?.Substring(startIndex) : input?.Substring(startIndex, length);
        public static int Length(string input) => input?.Length ?? 0;
        public static int IndexOf(string input, string value, int startIndex) => input?.IndexOf(value, startIndex) ?? -1;
        public static bool Match(string input, string pattern) => input != null ? System.Text.RegularExpressions.Regex.Match(input, pattern).Success: false;
        public static string Concat(string value1, string value2, string value3, string value4, string value5) => string.Concat(value1, value2, value3, value4, value5);
    }
}
