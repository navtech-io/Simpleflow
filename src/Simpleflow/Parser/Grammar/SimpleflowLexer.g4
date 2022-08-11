lexer grammar SimpleflowLexer;

channels { ERROR }
options { superClass=SimpleflowLexerBase; }

MultiLineComment:   '/*' .*? '*/'  -> channel(HIDDEN);
SingleLineComment:  '#' ~[\r\n\u2028\u2029]*  -> channel(HIDDEN);

OpenBracket:        '[';
CloseBracket:       ']';
OpenParen:          '(';
CloseParen:         ')'; 
OpenBrace:          '{';
TemplateCloseBrace: {this.IsInTemplateString()}? '}' -> popMode;
CloseBrace:         '}';

Colon:              ':' ;
Comma :             ',';
Dot:                '.';

Assign:             '=' ;

PlusOp:             '+';
MinusOp:            '-';
TimesOp:            '*';
DivOp:              '/';
ModuloOp:           '%'   ;   

   
// operators 
GreaterThan:        '>';
GreaterThanEqual:   '>=';
LessThan:           '<'; 
LessThanEqual:      '<=';
Equal:              '==';
NotEqual:           '!=';

// keywords

Let:                'let';
Set:                'set';
Partial:            'partial';

Rule:               'rule';
When:               'when';
Then:               'then';

End:                'end';
Exit :              'exit';

Message:            'message';
Error:              'error';
Output:             'output';

// relational operators

Semicolon: ';';

And:                'and';
Or:                 'or';
Not:                'not';

// Literals
True:               'true';
False:              'false';
Number:             ('+'|'-')?[0-9]+('.'[0-9]+)?;
 
String:             '"' ( '\\"' | ~["\r\n] )*? '"';
None:               'none' ;

Identifier:         [_]*[a-zA-Z][_a-zA-Z0-9]* ;
IgnoreIdentifier:  '_';

Indexer:             OpenBracket Number CloseBracket;

FunctionName:       '$' NAME ('.' NAME)*;
BackTick:           '`' {this.IncreaseTemplateDepth();} -> pushMode(TEMPLATE);

WhiteSpaces:        [\t\u000B\u000C\u0020\u00A0]+ -> channel(HIDDEN);
LineTerminator:     [\r\n\u2028\u2029] -> channel(HIDDEN);

mode TEMPLATE;

BackTickInside:                 '`' {this.DecreaseTemplateDepth();} -> type(BackTick), popMode;
TemplateStringStartExpression:  '{' -> pushMode(DEFAULT_MODE);
TemplateStringAtom: ~[`];

// Fragment rules

fragment NAME
    : [_]*[a-zA-Z][_a-zA-Z0-9]* ;

fragment PLUS_FRAGMENT
   : '+'   ;
fragment MINUS_FRAGMENT
   : '-'   ;

