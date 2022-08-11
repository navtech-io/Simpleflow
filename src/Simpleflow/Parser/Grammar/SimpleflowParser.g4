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
    : Rule When predicate Then eos 
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
    : (Partial)? Set (Identifier | IgnoreIdentifier) (Comma Identifier)? Assign expression eos  
    ;
   
messageStmt
    : Message messageText eos
    ;

errorStmt
    : Error messageText eos
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

messageText
    : (stringLiteral | templateStringLiteral | objectIdentifier)
    ; 

expression
    : boolLeteral 
    | noneLiteral 
    | function 
    | jsonObj 
    | objectIdentifier
    | arithmeticExpression 
    | stringLiteral 
    | templateStringLiteral
    | arrayLiteral
    ;
    
templateStringLiteral
    : BackTick templateStringAtom* BackTick
    ;   
   
templateStringAtom   
    : TemplateStringAtom
    | TemplateStringStartExpression  objectIdentifier TemplateCloseBrace
    ;
  
/** Arithmetic Expression */  
  
arithmeticExpression
   :  arithmeticExpression  (TimesOp | DivOp | ModuloOp)  arithmeticExpression  
   |  arithmeticExpression  (PlusOp | MinusOp) arithmeticExpression             
   |  OpenParen arithmeticExpression CloseParen                                 
   |  atom                                                                      
   ;

atom:
   | Number
   | objectIdentifier
   ;

/** Function */

function
    : FunctionName OpenParen (functionParameter (Comma functionParameter)*)? CloseParen
    ;

functionParameter
    : Identifier Colon functionParameterValue
    ; 
  

functionParameterValue
    : numberLiteral 
    | stringLiteral 
    | boolLeteral
    | noneLiteral
    | objectIdentifier
    | arithmeticExpression
    ; 

// Literals

objectIdentifier 
    : identifierIndex {this.NotLineTerminator()}? (Dot {this.NotLineTerminator()}? identifierIndex)*
    ;  
 

identifierIndex
    : Identifier {this.NotLineTerminator()}? index?
    ;

index
    : OpenBracket {this.NotLineTerminator()}? indexNumber {this.NotLineTerminator()}? CloseBracket
    ;

indexNumber
    : numberLiteral 
    | objectIdentifier 
    | function
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
   : '[' arrayValue (',' arrayValue)* ']'
   | '[' ']' 
   ; 

arrayValue
    : boolLeteral 
    | noneLiteral 
    | function 
    | objectIdentifier
    | arithmeticExpression 
    | stringLiteral 
    | templateStringLiteral
    ;
    
// JSON
jsonObj
   : OpenBrace pair (Comma pair)* CloseBrace
   | OpenBrace CloseBrace
   ;

pair
   : Identifier Colon expression
   ;

/**************************** */
/** predicate - recursive rule */
/**************************** */

predicate
    : testExpression
    | unaryOperand
    | predicate logicalOperator predicate
    | OpenParen predicate CloseParen
    | Not predicate
    ;

testExpression
    :operand relationalOperator operand 
    ;

logicalOperator
    : And
    | Or
    ;
    
relationalOperator
    : GreaterThan
    | LessThan
    | GreaterThanEqual
    | LessThanEqual
    | Equal
    | NotEqual
    
    ;
operand
    : objectIdentifier
    | stringLiteral
    | numberLiteral
    | boolLeteral
    | noneLiteral
    | function
    | arithmeticExpression
    ;

unaryOperand
    : boolLeteral
    | objectIdentifier
    | function
    ;

eos
    : EOF
    | {this.LineTerminatorAhead()}?
    ;


