parser grammar SimpleflowParser;

options {
    tokenVocab=SimpleflowLexer;
    superClass=SimpleflowParserBase;
}

program
    : letStmt* 
     (ruleStmt | generalStatement)* EOF; 
 
ruleStmt
    : Rule When predicate Then eos 
          (  messageStmt  
           | errorStmt       
           | outputStmt  
           | setStmt 
           | functionStmt
           | exitStmt
          )*
       (endRuleStmt)?
    ; 

endRuleStmt
    : End Rule eos
    ;

exitStmt
    : Exit eos
    ;

generalStatement
    : messageStmt
    | errorStmt
    | outputStmt  
    | setStmt 
    | functionStmt
    | exitStmt
    ;
   
letStmt
    : Let Identifier Assign expression eos  
    ;

setStmt
    : (Partial)? Set Identifier Assign expression eos  
    ;

messageStmt
    : Message messageText eos
    ;

errorStmt
    : Error messageText eos
    ;

messageText
    : (String | objectIdentifier)
    ;

outputStmt
    : Output objectIdentifier eos
    ;

functionStmt
    : function eos
    ;    

eos
    : EOF
    | {this.lineTerminatorAhead()}?
    ;


expression
    : boolLeteral 
    | noneLiteral 
    | function 
    | jsonObj 
    | objectIdentifier
    | arithmeticExpression 
    | stringLiteral 
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
    : Identifier (Dot Identifier)* 
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

// JSON
jsonObj
   : OpenBrace pair (Comma pair)* CloseBrace
   | OpenBrace CloseBrace
   ;

pair
   : Identifier Colon expression
   ;

// arr
//    : '[' value (',' value)* ']'
//    | '[' ']'
//    ;


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
    | Contains
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

