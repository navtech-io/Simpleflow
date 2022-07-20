## Simpleflow

Simpleflow is a lightweight dynamic rule engine to build workflow with intuitive script concepts. Simpleflow allows access to the process objects or methods in script securely. Methods can be registered as activities with Simpleflow engine, which is extensible to enrich or monitor the execution flow. Simpleflow is secure and efficient to run dynamic rules and workflow. 


[![NuGet version (Simpleflow)](https://img.shields.io/nuget/vpre/Simpleflow?style=for-the-badge)](https://www.nuget.org/packages/Simpleflow/) [![Github Workflow (Simpleflow)](https://img.shields.io/github/workflow/status/navtech-io/simpleflow/.NET?style=for-the-badge)](https://github.com/navtech-io/Simpleflow/actions)

**Simpleflow Script**

```csharp

// Simpleflow Script
var flowScript = 
@" 
    let text  = ""Hello, विश्वम्‌""
    let today = $GetCurrentDateTime ( timezone: ""Eastern Standard Time"" )

    /* comment: check condition  */
    rule when arg.UniversalId == 2 and (arg.New or arg.Verified)  then
         message text
    end rule
    
    output today
";

// Execute Script
FlowOutput result = SimpleflowEngine.Run(flowScript, new {UniversalId = 2, New=true, Verified=false} );

// Access result
Console.WriteLine(result.Messages[0]); 
Console.WriteLine(result.Output["today"]);

```
**Output**

```
Hello, विश्वम्‌
<current system date and time>
```
Please see [this](#examples) example with most of the simpleflow script features.

---

* [Script Outline](#script-outline)
* [Reference](#simpleflow-reference)
* [API](#api)
* [Examples](#examples)
* [Limitations](#limitations)

## Script Outline

```
<let statements>* 
(<rule statements> or <emitters> or <functions> or <set>)* 
```
##### Script Instructions
* All `let` statements (declare and initialize variables) must be declared in the beginning of the script.
* Each statement must end with a new line and each statement can be written in single line only.
* `set` statement can be used to modify the value of variable that has been declared using let statement. 
* All keywords should be small (case sensitive) (`let, set, message, error, output, rule, when, then, exit, end rule, partial `)
 * variable names and function names are not case sensitive.
 * `end rule` can be used to terminate the rule scope.
 * `exit` can be used to terminate the script execution.
 * Function parameters need not be ordered as it defined. And function must prefix with $.
 
 

## Simpleflow Reference
<a name="simpleflow-reference"></a>

1. [Variables](#variables)
1. [Data Types](#data-types)
1. [Operators](#operators)
1. [Expressions](#expressions)
1. [Script Parameters](#script-parameters)
1. [Rule Control Flow](#rule-control-flow)
1. [Emitters](#emitters)
1. [Functions](#functions)
1. [Comment](#comment)


#### Variables <a name="variables"></a>
$\color{skyblue}{Syntax}$
```fsharp
let <variablename> = expression
```
> <small> Expressions can be used with only variable. Anywhere else you need expression then declare variable,  assign expression and use it.</small>

**Modify value of a variable**
$\color{skyblue}{Syntax}$
```csharp
[partial] set <variablename> = expression
```

#### Data Types
**Simple Types:**
###### Number
```csharp
let x = 1
let y = 2.3
let z = -442.33
```
###### String
```csharp
let name = "test"
```
###### Boolean
```csharp
let hasValue = true
let allow = false
```
###### Date 
Use date function to declare a variable as date type.
```csharp
let birthday = $date(y:1980, m: 1, d: 1 )
```
**Complex Types:**

Object type can be defined using JSON format. It does not support nested object syntax, but in order to set nested object, you can set to a variable and use it.
```csharp
let address = {city: 'ny'}
let member =  {name: 'alex', address: address }
```

#### Operators

Arithmetic Operators: `+,-,*,/` <br>
Logical Operators:    `and, or, not`  <br>
Relational Operators: `<, <=, >, >=, == , !=` 

#### Expressions
```csharp
let v = 2 + 3 * (3 * arg.value); 
```

#### Script Parameters
`arg` and `context`
`arg` represents the input to the script.

Context Properties:
* context.HasErrors
* context.HasMessages
* context.HasOutput


#### Rule Control Flow
$\color{skyblue}{Syntax}$
```csharp
rule when <predicate> then
	<statement..1>	
	<statement..2>
	<statement..N>
[end rule]
```

> <small> condition does not allow expression. If you need to write expression
declare variable and write expression and use that variable in predicate. This does not support nested rules to avoid code complexity</small>


#### Emitters

$\color{skyblue}{Syntax}$
```
message <string/identifer>
error 	<string/identifer>
output 	<identifer>
exit    /*exit will terminate the execution*/
```


#### Functions
$\color{skyblue}{Syntax}$
```csharp
$<function_name>(param_name1: value1, param_name2: value2, ...)
```
  Function parameters can be written in any order. and if you omit a parameter it takes a default value of that type.
  Function cannot be an argument to another function. Store output of a function in a variable and use it.

###### Date Functions	

* $\color{#4686f2}{\$Date(y: int, m: int, d: int, [h:int, mn: int, s: int])}$
    ```csharp        
    // Examples
    let d1 = $Date(y: 2022, m: 7, d:11)
    let d2 = $Date(m: 10, d:25, y: 2022 )
    let t1 = $Date(m: 10, d:25, y: 2022, h:13, mn:30 )
    ```        
* $\color{#4686f2}{\$GetCurrentDate()}$
* $\color{#4686f2}{\$GetCurrentTime()}$
* $\color{#4686f2}{\$GetCurrentDateTime(timeZone: "")}$

    Check available list of time zones here: <br>
    Windows: https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/default-time-zones?view=windows-11#time-zones <br>
    Ubuntu: https://manpages.ubuntu.com/manpages/bionic/man3/DateTime::TimeZone::Catalog.3pm.html  <br>

    ```csharp
    let today    = $GetCurrentDateTime()
    let todayEst = $GetCurrentDateTime ( timezone: "Eastern Standard Time" )
    ```

###### String Functions
* $\color{#4686f2}{\$Substring(input: string,  startIndex:int, length: int)}$ 
* $\color{#4686f2}{\$IndexOf(input: string,  value:string, startIndex: int) }$
* $\color{#4686f2}{\$Length(input: string) }$
* $\color{#4686f2}{\$Contains(input: string,  value:string) }$ 
* $\color{#4686f2}{\$StartsWith(input: string,  value:string) }$
* $\color{#4686f2}{\$EndsWith(input: string,  value:string) }$
* $\color{#4686f2}{\$Trim(input: string,  value:string) }$
* $\color{#4686f2}{\$Match(input: string,  pattern:string) }$
* $\color{#4686f2}{\$Concat(value1: string,  value2:string,  value3:string,  value4:string,  value5:string)}$ 
    ```csharp
    let value = $Concat ( value1: "I ", value2: "got it" )
    ```
#### Comment
It supports only one style of comment can be used for single or multiline using /* .. */
```csharp
/* Write your comment here */
```

## API

1. [Simpleflow Execution](#simpleflow-pipeline)
1. [Flowoutput](#flowoutput)
1. [Register Custom Functions](#register-custom-functions)
1. [Extensibility](#extensibility)
1. [Compile Script](#compile-script)

#### Simpleflow Execution
<a name="simpleflow-pipeline"></a>

![Simpleflow Pipeline](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/navtech-io/Simpleflow/main/SimpleflowDiagram.puml)

Sample code to create, build and run pipeline
```csharp

// Build Pipeline to get full control over execution
ISimpleflowPipelineBuilder engine
    = new SimpleflowPipelineBuilder()

            // Add middleware services 
            .AddPipelineServices(
                    new MyPreprocessor(), 
                    new MyLoggingService());

            // Add core services - Cache, Compiler, Execution
            .AddCorePipelineServices(FunctionRegister.Default) // or
            /*
                 AddPipelineServices(
                    new CacheService(), //Cache uses Sliding window cache policy 
                    new CompilerService(ActivityRegister.Default),
                    new ExecutionService()); 
            */

// Build
ISimpleflow flow = engine.Build();

// Run
FlowOutput result = flow.Run(script, new Member { Id = id});

```
#### FlowOutput

Emitters (`message, error, output`) produce output from script that will be available in FlowOutput object.



#### Register Custom Functions

```csharp
FunctionRegister.Default
    .Add("DerivativeOfXPowN", (Func<int, int, int>)CalcDerivativeOfXPowN)

static int CalcDerivativeOfXPowN(int x, int n)
{
    return n *  Math.Pow(x, n-1); //
}
```



#### Extensibility

Create middleware and add it to pipeline.

```csharp
public class LoggingService : IFlowPipelineService
{
    public void Run<TArg>(FlowContext<TArg> context, NextPipelineService<TArg> next)
    {
        // log here    
        next?.Invoke(context);
    }
}
```

#### Compile Script
By adding only CompilerService to build pipeline, script can be compiled and reported if there are any errors.
```csharp
    
var engine
    = new SimpleflowPipelineBuilder()
        .AddPipelineServices(new Services.CompilerService(FunctionRegister.Default));

var simpleflow = engine.Build();
   
try 
{
    simpleflow.Run(script, context);
} 
catch(SimpleflowException exception)
{
    //Handle script errors
}

```

## Examples
<a name="examples"></a>

```csharp
/* Declare and initialize variables */
let userId      = none
let currentDate = $GetCurrentDateTime ( timezone: "Eastern Standard Time" )

/* Define Rules */
rule when  arg.Name == "" then
    error "Name cannot be empty"
    
rule when not $match(input: arg.Name, pattern: "^[a-zA-z]+$") then
    error "Invalid name. Name should contain only alphabets."
    
rule when arg.Age < 18 and arg.Country == 'US' then
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
                                         new User {Name = "John", Age=22, Country='US' } );

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



## Limitations
* Each statement can only be written in singleline.	 Currently It does not support multiline statement.
* Expressions, Objects ([], {}) cannot be used directly while passing parameters to a function.	But it accepts variables. There's a trick to use array in a function, if a function returns an array and that variable can be used to pass to another function.
* Arrays are not supported (planned in future releases).
* Nested objects cannot be defined directly, but you can use variable to use nested object
Below statement throws exception:
    ```csharp
    let o = {Id: 2, Name: "John", Address: {City: "Ny"} } 
    ```
    Alternate:
    ```csharp
    let address = {City: "Ny"}
    let o = {Id: 2, Name: "John", Address: address } 
    ```
