## Simpleflow

Simpleflow is a lightweight dynamic rule engine to build workflow with intuitive script concepts. Simpleflow allows access to the process objects or methods in script securely. Methods can be registered as activities with Simpleflow engine, which is extensible to enrich or monitor the execution flow. Simpleflow is secure and efficient to run dynamic rules and workflow. 

**Simpleflow Script**

```csharp

// Simpleflow Script
var flowScript = 
@" 
    let text  = ""Hello, विश्वम्‌""
    let today = $GetCurrentDateTime ( timezone: ""Eastern Standard Time"" )

    /* Comment: Message when UID is 2 and New is true */
    rule when arg.UID == 2 and arg.New then
         message text
    	 message today
";

// Execute Script
FlowOutput output = SimpleflowEngine.Run(flowScript, new {UID = 2, New=true} );

// Access Output
Console.WriteLine(output.Messages[0]); //output.Errors output.Output
```
**Output**

```
Hello, विश्वम्‌
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
* All keywords should be small (case sensitive) (`let, set, message, error, output, rule, when, then, exit, end rule `)
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
1. [Script Parameters](#argument)
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
set <variablename> = expression
```

#### Data Types
**Simple Types:**
* Number
    ```csharp
    let x = 1
    let y = 2.3
    let z = -442.33
    ```
* String
    ```csharp
    let name = "test"
    ```
* Boolean
    ```csharp
    let hasValue = true
    let allow = false
    ```
* Date 
<small>Simpleflow does not recognize date type directly, but this can be declared by using built-in date function</small>
    ```csharp
    let birthday = $date(y:1980, m: 1, d: 1 )
    ```
**Complex Types:**
Object type can be defined using JSON format. It does not support nested object, but in order to set nested object, you can set to a variable and use it.
```csharp
let address = {city: 'ny'}
let member =  {name: 'alex', address: address }
```

#### Operators
Arithmetic Operators: `+,-,*,/`
Logical Operators: `and, or, not`
Relational Operators: `<, <=, >, >=, == , !=`

#### Expressions
```csharp
   let v = 2 + 3; 
```

#### Script Parameters
`arg` and `context`

`arg` represents the input to the script.

```
context Properties:
    context.HasErrors
    context.HasMessages
    context.HasOutput
```

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
let/set varname = $<function_name>(param_name1: value1, param_name2: value2, ...)
```
  Function parameters can be written in any order. and if you omit a parameter it takes a default value of that type.
  Function cannot be an argument to another function. Store output of a function in a variable and use it.

* Date Functions	
    * $\color{green}{\$Date(y: int, m: int, d: int, [h:int, mn: int, s: int])}$
        ```csharp        
        // Examples
        let d1 = $Date(y: 2022, m: 7, d:11)
        let d2 = $Date(m: 10, d:25, y: 2022 )
        let t1 = $Date(m: 10, d:25, y: 2022, h:13, mn:30 )
        ```        
    * $\color{green}{\$GetCurrentDate()}$
    * $\color{green}{\$GetCurrentTime()}$
    * $\color{green}{\$GetCurrentDateTime(timeZone: "")}$
    Check supported time zones here:
    https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/default-time-zones?view=windows-11#time-zones
        ```csharp
        let today    = $GetCurrentDateTime()
        let todayEst = $GetCurrentDateTime ( timezone: "Eastern Standard Time" )
        ```

* String Functions
    * $\color{green}{\$Substring(input: string,  startIndex:int, length: int)}$ 
    * $\color{green}{\$IndexOf(input: string,  value:string, startIndex: int) }$
    * $\color{green}{\$Length(input: string) }$
    * $\color{green}{\$Contains(input: string,  value:string) }$ 
    * $\color{green}{\$StartsWith(input: string,  value:string) }$
    * $\color{green}{\$EndsWith(input: string,  value:string) }$
    * $\color{green}{\$Trim(input: string,  value:string) }$
    * $\color{green}{\$Match(input: string,  pattern:string) }$
    * $\color{green}{\$Concat(value1: string,  value2:string,  value3:string,  value4:string,  value5:string)}$ 
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

#### Simpleflow Execution
<a name="simpleflow-pipeline"></a>

<!-- ![Simpleflow Pipeline](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://github.com/navtech-io/Simpleflow/blob/main/SimpleflowDiagram.puml) -->

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
    .Add("DerivativeOfXPowN", (Func<int, int, int>)CalcDerivativeOfXPowN) //Don't add prefix $

static int CalcDerivativeOfXPowN(int x, int n)
{
    return n *  Math.Pow(x, n-1); //
}
```



#### Extensibility

###### Create Middleware

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

## Examples
<a name="examples"></a>

```csharp
/* Declare and initialize variables */
let user     = none

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

/* Set current time */
set arg.RegistrationDate = $GetCurrentDateTime ( timezone: "Eastern Standard Time" )
/* Save */
set user = $RegisterUser(user: arg) /* User defined function*/

output user  /*access this output using result.Output["user"]*/

```

```csharp
FlowOutput result = SimpleflowEngine.Run(rules /*above script*/, 
                                         new {Name = "John", Age=22, Country='US'} );

// Log messages
Logger.Info(result.Messages.ToCsv());

// Check errors
if (result.Errors.Count > 0 )
{
    // Show errors
    return;
}

// Capture registered user
var user =  result.Output["user"];

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






