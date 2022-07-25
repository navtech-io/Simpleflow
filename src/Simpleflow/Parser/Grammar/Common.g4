grammar Common;

objectIdentifier 
    : Identifier ('.' Identifier)* ;

stringLiteral
    : String
    ;

numberLiteral
    : Number
    ;

boolLeteral
    : 'true' | 'false'
    ;
 
noneLiteral
    : None
    ;
    
// Lexer

And
    : 'and';
Or
    : 'or';
Not
    : 'not';
    
// operators 
GreaterThan
    : '>';
GreaterThanEqual
    : '>=';
LessThan
    : '<'; 
LessThanEqual
    : '<=';
Equal
    : '==';
NotEqual
    : '!=';
Contains
    : 'contains';
OpenParen
    : '(';
CloseParen
    : ')'; 
// operands
Number  //Signed number (integer/decimal)
    : ('+'|'-')?[0-9]+('.'[0-9]+)?;

String
    : '"' ( '\\"' | ~["\r\n] )*? '"';

None
    : 'none' ;

Identifier 
    : [_]*[a-zA-Z][_a-zA-Z0-9]* ;


PlusOp
   : '+'   ;
MinusOp
   : '-'   ;

TimesOp 
   : '*'   ;

DivOp
   : '/'   ;

ModuloOp
   : '%'   ;   


fragment PLUS_FRAGMENT
   : '+'   ;
fragment MINUS_FRAGMENT
   : '-'   ;


// NUMBER
//    : '-'? INT ('.' [0-9] +)? EXP?
//    ;

// fragment INT
//    : '0' | [1-9] [0-9]*
//    ;

// // no leading zeros

// fragment EXP
//    : [Ee] [+\-]? INT
//    ;

