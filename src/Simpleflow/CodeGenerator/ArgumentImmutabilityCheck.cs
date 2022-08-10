// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

namespace Simpleflow.CodeGenerator
{
    internal static class ArgumentImmutabilityCheck
    {
        public static bool CheckForSameReference(object scriptArgument, object variable)
        {
            if (scriptArgument == null || variable == null )
            {
                return false;
            }

            if (object.ReferenceEquals(scriptArgument, variable))
            {
                return true;
            }

            if (scriptArgument != null)
            {
                // Deep dive for checking reference
                foreach (var prop in scriptArgument.GetType().GetProperties())
                {
                    if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                    {
                        var value = prop.GetValue(scriptArgument);
                        if (value != null)
                        {
                            return CheckForSameReference(prop.GetValue(scriptArgument), variable);
                        }
                    }
                }
            }

            return false;
        }
    }
}
