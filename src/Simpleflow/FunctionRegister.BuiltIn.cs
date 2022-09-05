// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using Simpleflow.Functions;

namespace Simpleflow
{
    public partial class FunctionRegister 
    {
        static FunctionRegister _register;
        static readonly object SyncDefault = new object();

        /// <summary>
        /// 
        /// </summary>
        public static FunctionRegister Default
        {
            get
            {
                lock (SyncDefault)
                {
                    if (_register == null)
                    {
                        _register = new FunctionRegister();

                        _register
                            .Add("Date", (Func<int, int, int, int, int, int, DateTime>)DateTimeFunctions.Date)
                            .Add("GetCurrentDate", (Func<DateTime>)DateTimeFunctions.GetCurrentDate)
                            .Add("GetCurrentTime", (Func<TimeSpan>)DateTimeFunctions.GetCurrentTime)
                            .Add("GetCurrentDateTime", (Func<string, DateTime>)DateTimeFunctions.GetNow)

                            .Add("Contains", (Func<string, string, bool>)StringFunctions.Contains)
                            .Add("StartsWith", (Func<string, string, bool>)StringFunctions.StartsWith)
                            .Add("EndsWith", (Func<string, string, bool>)StringFunctions.EndsWith)
                            .Add("Trim", (Func<string, string, string>)StringFunctions.Trim)
                            .Add("Substring", (Func<string, int, int, string>)StringFunctions.Substring)
                            .Add("IndexOf", (Func<string, string, int, int>)StringFunctions.IndexOf)
                            .Add("Length", (Func<string, int>)StringFunctions.Length)
                            .Add("Match", (Func<string, string, bool>)StringFunctions.Match)
                            .Add("Concat", (Func<string, string, string, string, string, string>)StringFunctions.Concat)
                            ;
                    }
                }

                return _register;
            }
        }
    }
}
