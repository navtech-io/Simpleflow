---
layout: default
title: Functions
parent: Simpleflow Script Reference
---

# Functions
{: .fs-9 }

Functions can be invoked from script that have been registered with this engine in a host language.

```csharp
$<function_name>(param_name1: value1, param_name2: value2, ...)
```
Function arguments can be written in any order. and if you omit an argument it takes a default value of that type. 
You can use your custom functions in Simpleflow script. Please refer to these sections of documentation to register and use custom functions.
[Register Custom Function](../../api/#register-custom-functions) | [Custom Function - Example](../../examples)


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
