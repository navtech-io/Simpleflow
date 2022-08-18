// Copyright (c) navtech.io. All rights reserved. 
// See License in the project root for license information.

parser grammar SimpleflowParser;

options {
    tokenVocab=SimpleflowLexer;
    superClass=SimpleflowParserBase;
}  

program
    : letStmt* 
     (ruleStmt | generalStatement)* EOF; 


letStmt
    : Let (Identifier | IgnoreIdentifier) (Comma Identifier)? Assign expression eos  
    ; 

ruleStmt
    : Rule When expression Then eos 
         generalStatement+
      endRuleStmt?
    ; 

generalStatement
    : messageStmt
    | errorStmt
    | outputStmt  
    | setStmt 
    | functionStmt
    | exitStmt
    ; 

endRuleStmt
    : End Rule eos
    ;

setStmt
    : (Partial)? Set (Identifier index? | IgnoreIdentifier) (Comma Identifier)? Assign expression eos
    ;
   
messageStmt
    : Message expression eos
    ;

errorStmt
    : Error expression eos
    ;

outputStmt
    : Output objectIdentifier eos
    ;

functionStmt
    : function eos
    ; 

exitStmt
    : Exit eos
    ;

expression
    : expression (TimesOp | DivOp | ModuloOp)  expression   #MultiplicativeExpression 
    | expression (PlusOp | MinusOp) expression              #AdditiveExpression 
    | expression ( GreaterThan 
                 | LessThan 
                 | GreaterThanEqual 
                 | LessThanEqual
                 | Equal 
                 | NotEqual 
                 ) expression                               #RelationalExpression
    | expression (And | Or ) expression                     #LogicalExpression
    | Not expression                                        #NotExpression
    | objectIdentifier                                      #ObjectIdentiferExpression
    | simpleLiteral                                         #SimpleLiteralExpression
    | arrayLiteral                                          #ArrayLiteralExpression
    | jsonObjLiteral                                        #JsonObjLiteralExpression
    | function                                              #FunctionExpression
    | OpenParen expression CloseParen                       #ParenthesizedExpression
    ;

simpleLiteral
    :  noneLiteral
    |  boolLeteral
    |  numberLiteral
    |  stringLiteral 
    |  templateStringLiteral
    ;  

templateStringLiteral
    : BackTick templateStringAtom* BackTick
    ;   
   
templateStringAtom   
    : TemplateStringAtom
    | TemplateStringStartExpression  expression TemplateCloseBrace
    ;
  
/** Function */
function
    : FunctionName functionArguments
    ;

functionArguments
    : OpenParen (functionArgument (Comma functionArgument)*)? CloseParen
    ;
    
functionArgument
    : Identifier Colon expression
    ; 

objectIdentifier 
    : identifierIndex (Dot identifierIndex)*
    ;  

identifierIndex
    : Identifier index?
    ;

index
    : OpenBracket expression CloseBracket
    ;

stringLiteral
    : String
    ;

numberLiteral
    : Number
    ;

boolLeteral
    : True | False
    ;
 
noneLiteral
    : None
    ;

arrayLiteral
   : '[' expression (',' expression)* ']'
   | '[' ']' 
   ; 
    
// JSON
jsonObjLiteral
   : OpenBrace pair (Comma pair)* CloseBrace
   | OpenBrace CloseBrace
   ;

pair
   : Identifier Colon expression
   ;

eos
    : EOF
    | {this.LineTerminatorAhead()}?
    ;
