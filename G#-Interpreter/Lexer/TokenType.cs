using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharpInterpreter
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
        IMPORT,         // Represents the "import" keyword
        DRAW,           // Represents the "draw" keyword
        PRINT,          // Represents the "print" keyword
        COLOR,          // Represents the "color" keyword
        RESTORE,        // Represents the "restore" keyword

        // Constants
        PI,             // Represents the "pi" constant
        EULER,          // Represents the "euler" constant

        // Data Types
        NUMBER,         // Represents a numeric value
        STRING,         // Represents a string value
        BOOLEAN,        // Represents a boolean value
        SEQUENCE,       // Represents a sequence of values
        UNDEFINED,      // Represents an undefined value

        // Geometry
        MEASURE,        // Represents a measure
        POINT,          // Represents a point
        LINE,           // Represents a line
        SEGMENT,        // Represents a segment
        RAY,            // Represents a ray
        CIRCLE,         // Represents a circle
        ARC,            // Represents an arc

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
        DOTS,           // Represents 3 dots "..."

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
        EQUAL,          // Represents the equality operator "=="
        NOT_EQUAL,      // Represents the inequality operator "!="
        GREATER,        // Represents the greater than operator ">"
        LESS,           // Represents the less than operator "<"
        GREATER_EQUAL,  // Represents the greater than or equal to operator ">="
        LESS_EQUAL,     // Represents the less than or equal to operator "<="

        // End of File
        EOF             // Represents the end of file marker
    }
}
