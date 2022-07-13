grammar Predicate;
import Common, Expression;

/* 
PARSER RULES
*/

/** predicate - recursive rule */
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
    ;

unaryOperand
    : boolLeteral
    | objectIdentifier
    | function
    ;

