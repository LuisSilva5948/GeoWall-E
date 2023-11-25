using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G__Interpreter
{
    /// <summary>
    /// Represents the type of a token.
    /// </summary>
    public enum TokenType
    {
        // Keywords
        LET,            // Represents the "let" keyword
        IN,             // Represents the "in" keyword
        IF,             // Represents the "if" keyword
        ELSE,           // Represents the "else" keyword
        FUNCTION,       // Represents the "function" keyword
        PI,             // Represents the "pi" constant
        EULER,          // Represents the "euler" constant

        // Variables
        IDENTIFIER,     // Represents an identifier (variable name)
        NUMBER,         // Represents a numeric value
        STRING,         // Represents a string value
        BOOLEAN,        // Represents a boolean value

        // Separators
        LEFT_PAREN,     // Represents the left parenthesis "("
        RIGHT_PAREN,    // Represents the right parenthesis ")"
        SEMICOLON,      // Represents a semicolon ";"
        COMMA,          // Represents a comma ","

        // Operators
        PLUS,           // Represents the addition operator "+"
        MINUS,          // Represents the subtraction operator "-"
        MULTIPLY,       // Represents the multiplication operator "*"
        DIVIDE,         // Represents the division operator "/"
        MODULUS,        // Represents the modulus operator "%"
        POWER,          // Represents the exponentiation operator "^"
        AND,            // Represents the logical AND operator "&"
        OR,             // Represents the logical OR operator "|"
        NOT,            // Represents the logical NOT operator "!"
        NOT_EQUAL,      // Represents the inequality operator "!="
        EQUAL,          // Represents the equality operator "=="
        ASSIGN,         // Represents the assignment operator "="
        GREATER,        // Represents the greater than operator ">"
        GREATER_EQUAL,  // Represents the greater than or equal to operator ">="
        LESS,           // Represents the less than operator "<"
        LESS_EQUAL,     // Represents the less than or equal to operator "<="
        CONCAT,         // Represents the string concatenation operator "@"
        LAMBDA,         // Represents the lambda function operator "=>"

        // End of File
        EOF             // Represents the end of file marker
    }
}
