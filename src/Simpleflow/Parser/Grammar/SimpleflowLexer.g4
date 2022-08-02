lexer grammar SimpleflowLexer;

channels { ERROR }
options { superClass=SimpleflowLexerBase; }

End:                'end';
Exit :              'exit';
Colon:              ':' ;
Comma :             ',';
True:               'true';
False:              'false';
Dot:                '.';
OpenBrace:          '{';
CloseBrace:         '}';

Rule:               'rule';
When:               'when';
Then:               'then';

Message:            'message';
Error:              'error';
Output:             'output';

Let:                'let';
Set:                'set';

Partial:            'partial';

Assign:             '=' ;

WhiteSpaces:        [\t\u000B\u000C\u0020\u00A0]+ -> channel(HIDDEN);
LineTerminator:     [\r\n\u2028\u2029] -> channel(HIDDEN);
MultiLineComment:   '/*' .*? '*/'  -> channel(HIDDEN);
SingleLineComment:  '#' ~[\r\n\u2028\u2029]*  -> channel(HIDDEN);

And:                'and';
Or:                 'or';
Not:                'not';
    
// operators 
GreaterThan:        '>';
GreaterThanEqual:   '>=';
LessThan:           '<'; 
LessThanEqual:      '<=';
Equal:              '==';
NotEqual:           '!=';
Contains:           'contains';
OpenParen:          '(';
CloseParen:         ')'; 

//Signed number (integer/decimal)
Number:             ('+'|'-')?[0-9]+('.'[0-9]+)?;
String:             '"' ( '\\"' | ~["\r\n] )*? '"';

None:               'none' ;
Identifier:         [_]*[a-zA-Z][_a-zA-Z0-9]* ;

PlusOp:             '+';
MinusOp:            '-';
TimesOp:            '*';
DivOp:              '/';
ModuloOp:           '%'   ;   

FunctionName:       '$' NAME ('.' NAME)*;

fragment NAME
    : [_]*[a-zA-Z][_a-zA-Z0-9]* ;

fragment PLUS_FRAGMENT
   : '+'   ;
fragment MINUS_FRAGMENT
   : '-'   ;

