// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;
using System.Globalization;
using Xunit;

using Simpleflow.Tests.Helpers;

namespace Simpleflow.Tests.Activities
{
    public class DateTimeFunctionsTest
    {

        [Fact]
        public void CheckDateTypeVariable()
        {
            // Arrange
            var context = new SampleArgument();

            var script =
                @"
                    let date = $Date(y: 2022, m: 7, d: 11)
                    output date 
                ";

            // Act

            FlowOutput output = SimpleflowEngine.Run(script, context);

            // Assert
            Assert.IsType<DateTime>(output.Output["date"]);
        }

        [Fact]
        public void CheckDate()
        {
            // Arrange
            var context = new SampleArgument();
            
            var script =
                @"
                    let date = $GetCurrentDate()
                    message date 
                ";

            // Act

            FlowOutput output = SimpleflowEngine.Run(script, context);
            
            // Assert
            Assert.Equal(actual: output.Messages.Count, expected: 1);
            Assert.Equal(actual: output.Messages[0], expected: DateTime.Now.Date.ToString(CultureInfo.CurrentCulture));

        }

        [Fact]
        public void CheckEstDate()
        {
            //System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
            //    System.Runtime.InteropServices.RuntimeInformation.OS)
            //Environment.OSVersion == OperatingSystem.
            // Arrange
            var arg = new SampleArgument();
            var script =
                string.Format(
                @"
                    let date = $GetCurrentDateTime ( timezone: ""{0}"" )
                    output date 
                ", TestsHelper.Timezone);

            // Act
            FlowOutput output = SimpleflowEngine.Run(script, arg);

            // Assert
            var date = TimeZoneInfo.ConvertTimeFromUtc(
                        DateTime.UtcNow,
                        TimeZoneInfo.FindSystemTimeZoneById(TestsHelper.Timezone));
            var actualDate = (DateTime)output.Output["date"];

            Assert.Equal(actual: actualDate.Date, expected: date.Date);
            Assert.Equal(actual: actualDate.TimeOfDay.Hours, expected: date.TimeOfDay.Hours);
            Assert.Equal(actual: actualDate.TimeOfDay.Minutes, expected: date.TimeOfDay.Minutes);
        }
       
    }
}
