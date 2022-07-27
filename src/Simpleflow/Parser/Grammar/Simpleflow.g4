grammar Simpleflow;
import Predicate, Expression, Json; 

options {
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
    : 'end' 'rule' eos
    ;

exitStmt
    : 'exit' eos
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

expression
    : boolLeteral | noneLiteral | function | jsonObj | arithmeticExpression | stringLiteral
    ;

eos
    : EOF
    | {this.lineTerminatorAhead()}?
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

WhiteSpaces:              
    [\t\u000B\u000C\u0020\u00A0]+ -> channel(HIDDEN);

LineTerminator:
    [\r\n\u2028\u2029] -> channel(HIDDEN);

MultiLineComment:   
   '/*' .*? '*/'  -> channel(HIDDEN);

