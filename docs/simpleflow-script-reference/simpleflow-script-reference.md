---
layout: default
title: Simpleflow Script Reference
nav_order: 2
has_children: true
permalink: docs/simpleflow-script-reference
---

# Simpleflow Script Reference
{: .fs-9 }

1. [Script Outline and Guidelines](#simpleflow-script-outline)
1. [Variables](#variables)
1. [Data Types](#data-types)
1. [Type Casting](#type-casting)
1. [Operators](#operators)
1. [Expressions](#expressions)
1. [Template Strings](#template-strings)
1. [Script Parameters](#script-parameters)
1. [Rule Control Flow](#rule-control-flow)
1. [Emitters](#emitters)
1. [Functions](simpleflow-script-reference/functions)
1. [Comments](#comments)
1. [Error Handling](#error-handling)


## Simpleflow Script Outline

```
<let_statement>* 
<general_statement>*
<rule_statement_block>*
<general_statement>* 
[<end_rule>]
<general_statement>*

general_statement:
    <emitter>* 
    or <invoke_function>*
    or <set statement>*
```

### Guidelines
* All `let` statements (declare and initialize variables) must be declared in the beginning of the script. you must declare variables if you wish to modify further in the script using set statement.
* Each statement must end with a new line and a statement can be written in multiple lines.
* All keywords must be written in lower case (`let, set, message, error, output, rule, when, then, exit, end rule, partial, true, false, none,... `). 
Variable name, property name, function's parameter name and function name are not case sensitive.
* Keywords cannot be used for variable name, function's parameter name and property name.
  Example: `arg.message` or `$Send(message: '')` are not valid,
  it will throw exception, but you can use keywords by changing the letter case. `arg.Message` is valid but not `arg.message`.


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
                <code>let x = 1 #Inferred to be of type int</code><br>
                <code>let y = 2.3 #Inferred to be of type decimal</code><br>
                <code>let z = -442.33 #Inferred to be of type decimal</code><br>
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
              }</code></pre> Object will be created and evaluated only when it will be supplied as parameter to a function. Until it gets created or activated by a function call, you cannot use partial set on this object. 
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
let user = none #inferred to be of type object

set user = 2
```

## Type Casting
Simpleflow does not offer any built-in type casting functionality but this can
be achieved by registering custom functions to convert a type to another type.

**Example**

```csharp
var script =
    @"
        let int = 10 #inferred to be of type int
        let decimal = 20.0 #inferred to be of type decimal

        # convert decimal to int
        set int = $decimal_to_int(value: decimal)

        output int
    ";

var functionRegister = new FunctionRegister()
                      .Add("decimal_to_int", 
                           (Func<decimal, int>)DecimalToInt);

var output = SimpleflowEngine.Run(script, new object(), functionRegister);
```
```csharp
private static int DecimalToInt(decimal value)
{
    return Convert.ToInt32(value);
}
```

## Operators

| Operator Type | Operators             |
|---------------|-----------------------|
| Arithmetic    | + (Addition), - (Subtraction), * (Multiplication), / (Division), % (Remainder)|
| Logical       | and, or, not          |
| Relational    | <, <=, >, >=, == , !=, in |

**Example - in operator**
```csharp
rule when 5 in [2,3,5] then
    message 'exists'

rule when not (5 in [2,3]) then
    message "doesn't exist"
```

## Expressions
```csharp
let v = 2 + 3 * (3 * arg.value); 
```

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
