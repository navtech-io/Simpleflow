---
layout: default
title: Simpleflow Script Reference
nav_order: 2
---

# Simpleflow Script Reference
{: .fs-9 }

1. [Script Outline and Guidelines](#script-outline)
1. [Variables](#variables)
1. [Data Types](#data-types)
1. [Operators](#operators)
1. [Expressions](#expressions)
1. [Template Strings](#template-strings)
1. [Script Parameters](#script-parameters)
1. [Rule Control Flow](#rule-control-flow)
1. [Emitters](#emitters)
1. [Functions](#functions)
1. [Comments](#comments)
1. [Error Handling](#error-handling)


## Script Outline

```
<let statements>* 
( <rule statements> 
 or <emitters> 
 or <functions> 
 or <set statements> )* 
```
### Guidelines
* All `let` statements (declare and initialize variables) must be declared in the beginning of the script. you must declare variables if you wish to modify further in the script using set statement. Set statement does not work on undeclared variable. 
* Each statement must end with a new line and a statement can be written in multiple lines.
* All keywords must be written in lower case (`let, set, message, error, output, rule, when, then, exit, end rule, partial, true, false `). Variable names, property names and function names are not case sensitive.


## Variables <a name="variables"></a>
```fsharp
let <variable_name> [, <error_handler_variable_name>] = expression
```

**Modify value of a variable** <br>
```csharp
[partial] set <variable_name> [, <error_handler_variable_name>] = expression
```
`set` statement can be used to modify the value of variable that has been declared using let statement. `partial` keyword can be used to modify certain properties of an object.

Update properties of an object:
```csharp
partial set arg = { RegistrationDate: currentDate, IsActive: true }
```

## Data Types

<table>
    <tr>
        <th> Data Type </th>
        <th> Description/Examples</th>
    </tr>
    <tr>
        <td>Number</td>
        <td>
                <code>let x = 1 </code><br>
                <code>let y = 2.3 </code><br>
                <code>let z = -442.33 </code><br>
        </td>
    </tr>
    <tr>
        <td>String</td>
        <td>
             In Simpleflow, single quotes (' ') or double quotes (" ") can be used to create string literals.
            <code>let name = 'test'</code>
        </td>
    </tr>
    <tr>
        <td>Boolean</td>
        <td>
            <code>let hasValue = true </code><br>
            <code>let allow = false </code><br>
        </td>
    </tr>
    <tr>
        <td>Date</td>
        <td>
            Use date function to declare a variable as date type. <br>
            <code>let birthday = $date(y:1980, m: 1, d: 1 )</code>
        </td>
    </tr>
    <tr>
        <td>Object</td>
        <td>
            Object type can be defined using JSON format. <br>
            <pre><code>let member = {
                name: "alex", 
                type: "Director", # type is Enum
                address: {
                           city: "ny"
                         } 
              }</code></pre> Object will be created only when it will be supplied as parameter to a function. Until it gets created or activated by a function, you cannot use partial set on this object. 
        </td>
    </tr>
    <tr>
        <td>
            List
        </td>
        <td>
            Simpleflow supports only list type<br>
<pre><code>let values = [100, 200, 300, 400]  # creates list of integers
output values[ 0 ] #output first value
</code></pre>
        </td>
    </tr>
</table>


```csharp
# Example
let user = none

set user = 2
```
## Operators

| Operator Type | Operators             |
|---------------|-----------------------|
| Arithmetic    | + (Addition), - (Subtraction), * (Multiplication), / (Division), % (Remainder)|
| Logical       | and, or, not          |
| Relational    | <, <=, >, >=, == , != |

## Expressions
```csharp
let v = 2 + 3 * (3 * arg.value); 
```
Use a space between each operand and operator.

## Template Strings
Template strings are strings delimited with backtick (`) characters, allowing for multi-line strings for string interpolation with embedded object identifiers.

```csharp
let to   = "John"; 
let from = "Chris"; 

let v    = ` Hi {to},
             .....
             .....
             Thanks,
             {from}
           `
```


## Script Parameters
`arg` and `context` parameters available in Simpleflow script. 
`arg` represents the input to the script.

**Context Properties:**

 | Property                                          | Description                                                      |
|---------------------------------------------------|------------------------------------------------------------------|
| context.HasErrors                                 | Returns true if there are any errors emitted                     |
| context.HasMessages                               | Returns true if there are any messages emitted                    |
| context.CancellationToken                         | Returns cancellation token that has been supplied to a script    |
| context.CancellationToken.IsCancellationRequested | Returns true if cancellation has been requested for that context |

CancellationToken can be supplied through context options.
Example:
```csharp
SimpleflowEngine.Run(script, <argument>, 
                     new FlowContextOptions { 
                        CancellationToken = <your cancellation token here>
                     });
```

## Rule Control Flow
```csharp
rule when expression then
	<statement..1>	
	<statement..2>
	<statement..N>
[end rule]
```
 `end rule` is optional and it can be used to terminate the rule scope.
Simpleflow does not support nested rules. if you need to write nested rules, declare a variable and write conditional expression and use it in rules.

Example:
```csharp
let highPriority = arg.priority == 'high'

# run rules when highPriority is true
rule when highPriority and arg.type == 'Gold' then
    $ProcessGoldCustomer(customer: arg)

rule when highPriority and arg.type == 'Silver' then
    $ProcessSilverCustomer(customer: arg)
```


## Emitters
Emitters are a kind of special functions to collect the stream of data and outputs, which are helpful to read the data from script execution.

| Emitter Type | Syntax                      	|
|--------------|--------------------------------|
| message      | `message <expression>`         |
| error        | `error <expression>`           |
| output       | `output <variable/property>`   |
| exit         | `exit`                         |

Note: `exit` can be used to terminate the script execution, and `output` does not support expression.

## Functions
Functions can be invoked from script that have been registered with this engine in a host language.

```csharp
$<function_name>(param_name1: value1, param_name2: value2, ...)
```
Function arguments can be written in any order. and if you omit an argument it takes a default value of that type. 
You can use your custom functions in Simpleflow script. 

<table>
    <tr>
        <th> Function </th>
        <th> Syntax/Examples</th>
    </tr>
    <tr>
        <td>Date</td>
        <td>
            <div>
		<pre>$Date(y: int, m: int, d: int, [h:int, mn: int, s: int])</pre>  
                <code>let d1 = $Date(y: 2022, m: 7, d:11) </code><br>
                <code>let d2 = $Date(m: 10, d:25, y: 2022 ) </code><br>
                <code>let t1 = $Date(m: 10, d:25, y: 2022, h:13, mn:30 ) </code>
            </div>
        </td>
    </tr>
    <tr>
        <td>GetCurrentDate</td>
        <td>
		<pre>$GetCurrentDate()</pre>
        </td>
    </tr>
    <tr>
        <td>GetCurrentTime</td>
        <td>
		<pre>$GetCurrentTime()</pre>
        </td>
    </tr>
    <tr>
        <td>GetCurrentDateTime</td>
        <td>
		<pre>GetCurrentDateTime(timeZone: string)</pre> 
	    <a href="https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/default-time-zones?view=windows-11#time-zones">Windows Timezones List</a><br>
	    <a href="https://manpages.ubuntu.com/manpages/bionic/man3/DateTime::TimeZone::Catalog.3pm.html">Ubuntu Timezones List</a><br>
            <code>let today = $GetCurrentDateTime() </code> <br>
            <code>let todayEst = $GetCurrentDateTime ( timezone: "Eastern Standard Time" )</code>
        </td>
    </tr>
    <tr>
        <td>Substring</td>
        <td>
		<pre>$Substring(input: string, startIndex:int, length: int)</pre>
        </td>
    </tr>
    <tr>
        <td>IndexOf</td>
        <td>
            <pre>$IndexOf(input: string, value:string, startIndex: int) </pre>
        </td>
    </tr>
    <tr>
        <td>Length</td>
        <td>
            <pre>$Length(input: string) </pre>
        </td>
    </tr>
    <tr>
        <td>Contains</td>
        <td>
            <pre>$Contains(input: string, value:string) </pre>
        </td>
    </tr>
    <tr>
        <td>StartsWith</td>
        <td>
            <pre>$StartsWith(input: string, value:string) </pre>
        </td>
    </tr>
    <tr>
        <td>EndsWith</td>
        <td>
            <pre>$EndsWith(input: string, value:string) </pre>
        </td>
    </tr>
    <tr>
        <td>Trim</td>
        <td>
            <pre>$Trim(input: string, value:string)</pre> 
        </td>
    </tr>
    <tr>
        <td>Match</td>
        <td>
            <pre>$Match(input: string, pattern:string) </pre>
        </td>
    </tr>
    <tr>
        <td>Concat</td>
        <td>
<pre>Concat(value1: string, value2:string, value3:string,
            value4:string, value5:string) 
</pre>
            <code>let value = $Concat ( value1: "I ", value2: "got it" )</code>
        </td>
    </tr>
</table>
    
## Comments
It supports single line and multi line comments.
```csharp
# Single line comment using hash symbol

/*  
    Multi-line comment
*/
```
## Error Handling
v1.0.4
{: .fs-1 }

By specifying a second variable along with the first one in a `let` or `set` statement, you can capture error if occurred.

```csharp
let x, err = 2 / 0

rule when err then
    message `Error has occurred:  {err.Message}`
end rule

set x, err2 = 5 + 3

rule when not err2 then
    message "No error"

```
when you use `set` to update a variable and you want to catch the error as well, if error is occurred then you don't need declare err2 using `let` as you declare to capture regular value.
