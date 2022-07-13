grammar Expression;
import Common, Json; 

arithmeticExpression
   :  arithmeticExpression  (TimesOp | DivOp)  arithmeticExpression
   |  arithmeticExpression  (PlusOp | MinusOp) arithmeticExpression
   |  OpenParen arithmeticExpression CloseParen
   |  atom
   ;

atom:
   | Number
   | objectIdentifier
   ;

function
    : FunctionName OpenParen (functionParameter (',' functionParameter)*)? CloseParen
    ;

functionParameter
    : Identifier ':' functionParameterValue
    ; 
  

functionParameterValue
    : numberLiteral 
    | stringLiteral 
    | boolLeteral
    | noneLiteral
    | objectIdentifier
    ; 

// Lexer

FunctionName
    : '$' NAME ('.' NAME)*
    ;

fragment NAME
    : [_]*[a-zA-Z][_a-zA-Z0-9]* ;


 
