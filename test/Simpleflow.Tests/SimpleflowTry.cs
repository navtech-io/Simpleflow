using Xunit;
using Simpleflow.Tests.Helpers;

namespace Simpleflow.Tests
{
    public class SimpleflowTry
    {
        [Fact]
        public void Try()
        {
            // Arrange
            var flowScript =
            @$" 
                let text  = ""Hello, विश्वम्‌""
                let today = $GetCurrentDateTime ( timezone: ""{TestsHelper.Timezone}"" )

                /* Comment: Message when UniversalId is 2 and New is true */
                rule when arg.UniversalId == 2 and (arg.New or arg.Verified)  then
                     message text
    	             message today
            ";

            // Act
            FlowOutput output = SimpleflowEngine.Run(flowScript, new { UniversalId = 2, New = true, Verified = false });

            // Assert
            Assert.Equal("Hello, विश्वम्‌", output.Messages[0]);
        }

        [Fact]
        public void Try2()
        {
            // Arrange
            var flowScript =
            @$" 
                let text  = ""Hello, विश्वम्‌""
                let today = $GetCurrentDateTime ( timezone: ""{TestsHelper.Timezone}"" )

                /* Comment: Message when UniversalId is 2 and New is true */
                rule when arg.UniversalId == 2 and (arg.New or arg.Verified)  then
                     message text
                end rule
    	        
                output today
            ";

            // Act
            FlowOutput output = SimpleflowEngine.Run(flowScript, new { UniversalId = 2, New = false, Verified = false });

            // Assert
            Assert.Empty(output.Messages);
        }
    }
}
