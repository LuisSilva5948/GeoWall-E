using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharpInterpreter
{
    /// <summary>
    /// Represents a token in the source code.
    /// </summary>
    public class Token
    {
        public TokenType Type { get; }  // The type of the token
        public string Lexeme { get; }   // The lexeme of the token
        public object Literal { get; }  // The literal value of the token
        public int Line { get; }        // The line number where the token appears
        public Token(TokenType type, string lexeme, Object literal, int line)
        {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
            Line = line;
        }

        public override string ToString()
        {
            return Type + " " + Lexeme + " " + Literal;
        }
    }
}
