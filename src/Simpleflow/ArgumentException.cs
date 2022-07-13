// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Runtime.CompilerServices;

namespace Simpleflow
{
    internal static class ArgumentException
    {
        public static void ThrowIfNull(object argument, [CallerArgumentExpression("argument")]string paramName = default)
        {
            if (argument == null)
            {
                if (paramName == null)
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    throw new ArgumentNullException(paramName);
                }
            }
        }

        public static void ThrowIfNullOrEmpty(string argument, [CallerArgumentExpression("argument")] string paramName = default)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                if (paramName == null)
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    throw new ArgumentNullException(paramName);
                }
            }
        }
    }
}
