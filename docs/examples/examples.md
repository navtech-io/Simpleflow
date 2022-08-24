---
layout: default
title: Examples
nav_order: 99
has_children: true
permalink: docs/examples
---

# Examples

### Sample Simpleflow Script

```csharp
# Declare and initialize variables 
let userId       = 0
let currentDate  = $GetCurrentDateTime ( timezone: "Eastern Standard Time" )
let emailMessage = ""

# Define Rules 
rule when  arg.Name == "" 
           or arg.Name == none then
    error "Name cannot be empty"
    
rule when not $match(input: arg.Name, pattern: "^[a-zA-z]+$") then
    error "Invalid name. Name should contain only alphabets."
    
rule when arg.Age < 18 and arg.Country == "US" then
    error `Your age must be greater than 18 years 
           in order to register in the united states.`
end rule

# debug message
message "validations-completed"

rule when context.HasErrors then
    exit
end rule

# Update RegistrationDate and IsActive flag
partial set arg = { 
                    RegistrationDate: currentDate, 
                    IsActive: true 
                  }

# Save user's data
set userId, err = $CustomerService.RegisterUser(user: arg) 

# *if the above function has thrown an exception then stop executing
rule when err then
    error `Registration Failed. {err.Message}`
    output err
    exit
end rule

# Send an email to user, once the record is saved successfully.
set emailMessage  = ` Hello {arg.Name},
                      We would like to confirm that your account 
                      was created successfully.

                      Thank you for joining.
                      Date: {arg.RegistrationDate}
                    `
set _, err = $SendEmail(to      : arg.email, 
                        subject : "Thanks for signing up"
                        body    : emailMessage)

output userId 
```
### Execute

```csharp
// Register custom function
var register = new FunctionRegister()
    .Add("CustomerService.RegisterUser", (Func<User, int>)RegisterUser);
    .Add("SendEmail", (Action<string, string, string>)SendEmail);

// Execute Script
FlowOutput result = SimpleflowEngine.Run(script /*above script*/, 
                                         new User {Name = "John", Age=22, Country="US" },
                                         register);

// Log messages
Logger.Debug(result.Messages);

// Check errors
if (result.Errors.Count > 0 )
{
    // Show errors
    return;
}

// Capture registered user
var userId =  result.Output["userId"];

```

```csharp

class User { 
    public string Name {get;set;}
    public string Email {get;set;}
    public int Age {get;set;}
    public string Country {get;set;}
    public bool IsActive {get;set;}
    public DateTime RegistrationDate {get;set;} 
}

static int RegisterUser(User user)
{
    return 1;
}

static void SendEmail(string body, string subject, string to)
{
    // Send email logic here
}

```