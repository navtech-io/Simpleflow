grammar Simpleflow;
import Predicate, Expression, Json; 

options {
    superClass=SimpleflowParserBase;
}

program
    :
     LineBreak*
     letStmt* 
     (ruleStmt | generalStatement)* EOF; 
 

ruleStmt
    : Rule When predicate Then LineBreak 
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
    : 'end' 'rule' LineBreak
    ;

exitStmt
    : 'exit' LineBreak
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
    : Let Identifier Assign expression LineBreak  
    ;

setStmt
    : (Partial)? Set Identifier Assign expression LineBreak  
    ;

messageStmt
    : Message messageText LineBreak
    ;

errorStmt
    : Error messageText LineBreak
    ;

messageText
    : (String | objectIdentifier)
    ;

outputStmt
    : Output objectIdentifier LineBreak
    ;

functionStmt
    : function LineBreak
    ;    

expression
    : boolLeteral | noneLiteral | function | jsonObj | arithmeticExpression | stringLiteral
    ;

// Lexer
       
Rule
    : 'rule';
When
    : 'when';
Then
    : 'then';

Message
    : 'message';
Error
    : 'error';

Output
    : 'output';

Let
    : 'let';
Set
    : 'set';

Partial
    : 'partial';

Assign
   : '=' ;

LineBreak
    : [\r\n]+[ \t\r\n]*
    ;

Skip_
    : ( SPACES | COMMENT ) -> skip    ;

fragment SPACES
    : [ \t]+  ;

fragment COMMENT
    : '/*' .*? '*/' ([\r\n]*) ;
