﻿
namespace Simpleflow.Tests.Helpers
{
    internal static class TestsHelper
    {
        public static bool IsWindows => System.Runtime.InteropServices.RuntimeInformation.OSDescription.Contains("Windows");

        public static string Timezone => IsWindows ? "Eastern Standard Time" : "America/New_York";
    }
}
