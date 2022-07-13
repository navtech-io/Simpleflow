
grammar Json;
import Common;

jsonObj
   : '{' pair (',' pair)* '}'
   | '{' '}'
   ;

pair
   : Identifier ':' value
   ;

// arr
//    : '[' value (',' value)* ']'
//    | '[' ']'
//    ;

value
   : boolLeteral
   | noneLiteral
   | numberLiteral
   | objectIdentifier
   | stringLiteral
   //| obj
   //| arr
   ;



