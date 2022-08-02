---
layout: default
title: Simpleflow Script Reference
nav_order: 2
---

# Simpleflow Script Reference
{: .fs-9 }

1. [Script Outline](#script-outline)
1. [Variables](#variables)
1. [Data Types](#data-types)
1. [Operators](#operators)
1. [Expressions](#expressions)
1. [Script Parameters](#script-parameters)
1. [Rule Control Flow](#rule-control-flow)
1. [Emitters](#emitters)
1. [Functions](#functions)
1. [Comments](#comments)
1. [Script Guidelines](#script-guidelines)
1. [Limitations](#limitations)


## Script Outline

```
<let statements>* 
(<rule statements> or <emitters> or <functions> or <set>)* 
```


## Variables <a name="variables"></a>
```fsharp
let <variablename> = expression
```
Expressions can be used with only variable. Anywhere else you need expression then declare variable,  assign expression and use it.

**Modify value of a variable** <br>
```csharp
[partial] set <variablename> = expression
```
`set` statement can be used to modify the value of variable that has been declared using let statement. `partial` keyword can be used to modify certain properties of an object.

Change values of properrties of an object:
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
            <code>let name = "test"</code>
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
        <td>Object Type</td>
        <td>
            Object type can be defined using JSON format. 
            <code>let member = {name: 'alex', address: {city: 'ny'} }</code>
        </td>
    </tr>
</table>



## Operators

| Operator Type | Operators             |
|---------------|-----------------------|
| Arithmetic    | + (Addition), - (Subtraction), * (Multiplication), / (Division), % (Modulo)|
| Logical       | and, or, not          |
| Relational    | <, <=, >, >=, == , != |

## Expressions
```csharp
let v = 2 + 3 * (3 * arg.value); 
```

## Script Parameters
`arg` and `context` parameters available in Simpleflow script. 
`arg` represents the input to the script.

**Context Properties:**

 | Property                                          | Description                                                      |
|---------------------------------------------------|------------------------------------------------------------------|
| context.HasErrors                                 | Returns true if there are any errors emitted                     |
| context.HasMessages                               | Returns true if there are any messages emitted                    |
| context.HasOutputs                                 | Returns true if there are any outputs emitted                     |
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
rule when <predicate> then
	<statement..1>	
	<statement..2>
	<statement..N>
[end rule]
```

Condition does not allow expression. If you need to write expression declare variable and write expression and use that variable in predicate. This does not support nested rules to avoid code complexity. `end rule` is optional and it can be used to terminate the rule scope.

## Emitters

| Emitter Type | Syntax                      	|
|--------------|--------------------------------|
| message      | `message <string/identifier>`  |
| error        | `error <string/identifier>`    |
| output       | `output <identifier>`    	|
| exit         | `exit`                         |

`exit` can be used to terminate the script execution.

## Functions
```csharp
$<function_name>(param_name1: value1, param_name2: value2, ...)
```
Function parameters can be written in any order. and if you omit a parameter it takes a default value of that type.
Function cannot be an argument to another function. Store output of a function in a variable and use it.

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
It supports single line and multi line comments 
Single line comment using hash symbol: # ***your comment here***
Multi line comment using /* ... */ :  
/*
     ***your comment here***   
*/

```csharp
/* Write your comment here */
```
	
## Script Guidelines
* All `let` statements (declare and initialize variables) must be declared in the beginning of the script.
* Each statement must end with a new line and a statement can be written in multiple lines.
* All keywords should be small (case sensitive) (`let, set, message, error, output, rule, when, then, exit, end rule, partial `)
* variable, property and function names are not case sensitive.


## Limitations
* Expressions, Objects ([], {}) cannot be used directly while passing parameters to a function.	But it accepts variables. There's a trick to use array in a function, if a function returns an array and that variable can be used to pass to another function.
* Arrays are not supported (planned in future releases).
```	