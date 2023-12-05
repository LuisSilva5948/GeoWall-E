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
        THEN,           // Represents the "then" keyword
        ELSE,           // Represents the "else" keyword
        POINT,          // Represents the "point" keyword
        LINE,           // Represents the "line" keyword
        SEGMENT,        // Represents the "segment" keyword
        RAY,            // Represents the "ray" keyword
        CIRCLE,         // Represents the "circle" keyword
        SEQUENCE,       // Represents the "sequence" keyword

        // Constants
        PI,             // Represents the "pi" constant
        EULER,          // Represents the "euler" constant

        // Data Types
        NUMBER,         // Represents a numeric value
        STRING,         // Represents a string value
        BOOLEAN,        // Represents a boolean value

        // Identifiers
        IDENTIFIER,     // Represents an identifier (variable name)
        ASSIGN,         // Represents the assignment operator "="

        // Separators
        LEFT_PAREN,     // Represents the left parenthesis "("
        RIGHT_PAREN,    // Represents the right parenthesis ")"
        LEFT_BRACE,     // Represents the left brace "{"
        RIGHT_BRACE,    // Represents the right brace "}"
        SEMICOLON,      // Represents a semicolon ";"
        COMMA,          // Represents a comma ","

        // Arithmetic Operators
        ADDITION,       // Represents the addition operator "+"
        SUBSTRACTION,   // Represents the subtraction operator "-"
        MULTIPLICATION, // Represents the multiplication operator "*"
        DIVISION,       // Represents the division operator "/"
        MODULO,         // Represents the modulo operator "%"
        POWER,          // Represents the exponentiation operator "^"

        // Logical Operators
        AND,            // Represents the logical AND operator "&"
        OR,             // Represents the logical OR operator "|"
        NOT,            // Represents the logical NOT operator "!"

        // Comparison Operators
        EQUAL,       // Represents the equality operator "=="
        NOT_EQUAL,     // Represents the inequality operator "!="
        GREATER,        // Represents the greater than operator ">"
        LESS,           // Represents the less than operator "<"
        GREATER_EQUAL,  // Represents the greater than or equal to operator ">="
        LESS_EQUAL,     // Represents the less than or equal to operator "<="
        CONCAT,         // Represents the string concatenation operator "@"


        // End of File
        EOF             // Represents the end of file marker
    }
}
