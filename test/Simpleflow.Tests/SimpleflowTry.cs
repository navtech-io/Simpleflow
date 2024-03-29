﻿using Xunit;
using Simpleflow.Tests.Helpers;

namespace Simpleflow.Tests
{
    public class SimpleflowTry
    {
        [Fact]
        public void Try()
        {
            // Arrange -- Don't change the indentation of variable, its written for a type of testing of parser
            var flowScript =
            @"
let text  = 'Hello, विश्वम्‌'
let today = $GetCurrentDateTime ( timezone: '" + TestsHelper.Timezone + @"' )

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
        public void Try1()
        {
            // Arrange
            var flowScript =
            @" 

                let today = $GetCurrentDateTime ( timezone: """ + TestsHelper.Timezone + @""" )

                # Write rules
                rule when arg.UniversalId == 2 and (arg.New or arg.Verified)  then
                     message `Hello, World! 🌄, Universal Id {arg.UniversalId}`
                end rule

                # Output
                output today
            ";

            // Act
            FlowOutput output = SimpleflowEngine.Run(flowScript, new { UniversalId = 2, New = true, Verified = false });

            // Assert
            Assert.Equal("Hello, World! 🌄, Universal Id 2", output.Messages[0]);
        }

        [Fact]
        public void Try2()
        {
            // Arrange
            var flowScript =
            @" 
                let text  = ""Hello, विश्वम्‌""
                let today = $GetCurrentDateTime ( timezone: """ + TestsHelper.Timezone + @""" )

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

        [Fact]
        public void Try3()
        {
            // Arrange
            var flowScript =
            @" 
                /* Declare and initialize variables */
                let userId      = none
                

                /* Define Rules */
                rule when  arg.Name == """" 
                           or arg.Name == none then
                    error ""Name cannot be empty""


                rule when not $match(input: arg.Name , 
                                     pattern: ""^[a-zA-z]+$"") then
                    error ""Invalid name. Name should contain only alphabets.""


                rule when arg.Age < 18 and arg.Country == ""US"" then
                    error ""You cannot register""
                end rule

                /* Statements outside of the rules */
                message ""validations-completed""

                rule when context.HasErrors then
                    exit
                end rule

                /* Set current date time */
                partial set arg = { 
                                     RegistrationDate: $GetCurrentDateTime ( 
                                        timezone: """ + TestsHelper.Timezone + @""" ), 
                                     IsActive: true 
                                  }

                set _, err = $SendEmail(body: 'test', 
                                          to: arg.email)  


            ";

            FunctionRegister.Default
                 .Add("SendEmail", (System.Action<string, string>)SendEmail);


            // Act & Assert
            var user = new User { Name = "John", Age = 22, Country = "US" };
            FlowOutput output = SimpleflowEngine.Run(flowScript, user);
            Assert.True(user.IsActive);

            var user2 = new User { Name = "John", Age = 14, Country = "US" };
            output = SimpleflowEngine.Run(flowScript, user2);
            Assert.False(user2.IsActive);

            var user3 = new User { Age = 14, Country = "US" };
            output = SimpleflowEngine.Run(flowScript, user3);
            Assert.Equal("Name cannot be empty", output.Errors[0]);

        }

        static void SendEmail(string body, string to)
        {
            // Send email logic here
        }

        class User
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public int Age { get; set; }
            public string Country { get; set; }
            public bool IsActive { get; set; }
            public System.DateTime RegistrationDate { get; set; }
        }
    }
}
