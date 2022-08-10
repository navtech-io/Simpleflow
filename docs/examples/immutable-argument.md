---
layout: default
title: Immutable Argument
nav_order: 99
---

# Examples

**Immutable argument example**

```csharp
# Declare and initialize variables 
let address = arg.address

/* try to modify, but it does not allow and throws exception 
   since we run this script under immutable context */

partial set address, err = {
    city: 'ny'
}

/* this condition will be passed since we set the 
   AllowArgumentToMutate to false for the  current 
   execution context */

rule when err then 
    output err

```
**Execute**

```csharp

var customer =  new Customer 
                { 
                    Name = "John",  
                    Address= new Address
                    {
                        State = "CA"
                    }
                },

// Execute 
FlowOutput result = SimpleflowEngine.Run(rules,     // Above script
                                         customer,  // argument
                                         new FlowContextOptions
                                         {
                                            // Don't allow to modify customer object
                                            AllowArgumentToMutate = false 
                                         });
// Capture error
var err =  result.Output["err"];
```
Note: Simpleflow does not send a copy of argument to a function, hence it cannot prevent to modify if a function changes value of script argument. You can disallow those functions if you need strict immutability.