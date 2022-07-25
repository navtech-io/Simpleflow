---
layout: default
title: Examples
nav_order: 99
---

# Examples

**Sample Simpleflow Script**

```csharp
/* Declare and initialize variables */
let userId      = none
let currentDate = $GetCurrentDateTime ( timezone: "Eastern Standard Time" )

/* Define Rules */
rule when  arg.Name == "" or arg.Name == none then
    error "Name cannot be empty"
    
rule when not $match(input: arg.Name, pattern: "^[a-zA-z]+$") then
    error "Invalid name. Name should contain only alphabets."
    
rule when arg.Age < 18 and arg.Country == "US" then
    error "You cannot register"
end rule

/* Statements outside of the rules */
message "validations-completed"

rule when context.HasErrors then
    exit
end rule

/* Set current date time */
partial set arg = { RegistrationDate: currentDate, IsActive: true }

/* Save */
set userId = $CustomerService.RegisterUser(user: arg) /* User defined function*/

output userId  /*access this output using result.Output["userId"]*/

```
**Sample simpleflow script execution from code**

```csharp
class User { 
    public string Name {get;set;}
    public int Age {get;set;}
    public string Country {get;set;}
    public bool IsActive {get;set;}
    public DateTime RegistrationDate {get;set;} 
}

// Register custom function
var register = 
    FunctionRegister.Default
        .Add("CustomerService.RegisterUser", (Func<User, int>)RegisterUser);

// Execute Dynamic Script
FlowOutput result = SimpleflowEngine.Run(rules /*above script*/, 
                                         new User {Name = "John", Age=22, Country="US" } );

// Log messages
Logger.Info(result.Messages.ToCsv());

// Check errors
if (result.Errors.Count > 0 )
{
    // Show errors
    return;
}

// Capture registered user
var userId =  result.Output["userId"];

```

