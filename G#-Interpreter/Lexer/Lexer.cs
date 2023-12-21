using System.ComponentModel.Design;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace GSharpInterpreter
{
    /// <summary>
    /// Represents a lexer for tokenizing source code.
    /// </summary>
    public class Lexer
    {
        private readonly string Source;                 // The source code
        private List<Token> Tokens;                     // The list of Tokens
        private int StartofLexeme;                      // The start of the current lexeme
        private int CurrentPosition;                    // The current position in the source code
        public List<Error> Errors { get; private set; } // The list of errors found during lexing

        public Lexer(string source)
        {
            Source = source;
            Tokens = new List<Token>();
            StartofLexeme = 0;
            CurrentPosition = 0;
            Errors = new List<Error>();
        }

        /// <summary>
        /// Scans the source code and generates a list of Tokens.
        /// </summary>
        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                StartofLexeme = CurrentPosition;
                ScanToken();
            }
            //add end of file token
            Tokens.Add(new Token(TokenType.EOF, "EOF", null));
            return Tokens;
        }
        /// <summary>
        /// Scans a single token based on the current character.
        /// </summary>
        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                //ignore whitespace
                case ' ':
                case '\r':
                case '\t':
                case '\n':
                    break;
                //scan separators and operators
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '+': AddToken(TokenType.ADDITION); break;
                case '-': AddToken(TokenType.SUBSTRACTION); break;
                case '*': AddToken(TokenType.MULTIPLICATION); break;
                case '/': AddToken(TokenType.DIVISION); break;
                case '%': AddToken(TokenType.MODULO); break;
                case '^': AddToken(TokenType.POWER); break;
                case '&': AddToken(TokenType.AND); break;
                case '|': AddToken(TokenType.OR); break;
                case '!': AddToken(Match('=') ? TokenType.NOT_EQUAL : TokenType.NOT); break;
                case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                case '=': AddToken(Match('=') ? TokenType.EQUAL : TokenType.ASSIGN); break;
                case '_': 
                    if (char.IsLetterOrDigit(Peek())) 
                        ScanIdentifier();
                    else AddToken(TokenType.UNDERSCORE);
                    break;
                case '.':
                    if (Match('.') && Match('.')) AddToken(TokenType.DOTS);
                    else throw new Error(ErrorType.COMPILING, $"Invalid token at '{GetLexeme()}'.");
                    break;
                //scan strings
                case '\"': ScanString(); break;
                //scan numbers, identifiers and keywords
                default:
                    if (char.IsDigit(c))
                    {
                        ScanNumber();
                    }
                    else if (char.IsLetter(c))
                    {
                        ScanIdentifier();
                    }
                    else throw new Error(ErrorType.COMPILING, $"Character '{c}' is not supported.");
                    break;
            }
        }
        /// <summary>
        /// Scans a number token.
        /// </summary>
        private void ScanNumber()
        {
            //bool for checking if the number is invalid, if contains letters
            bool isinvalidnumber = false;
            while (char.IsLetterOrDigit(Peek()))
            {
                if (char.IsLetter(Peek())) isinvalidnumber = true;
                Advance();
            }

            if (Peek() == '.' && char.IsLetterOrDigit(PeekAhead(1)))
            {
                //consume the '.'
                Advance();

                while (char.IsLetterOrDigit(Peek()))
                {
                    if (char.IsLetter(Peek())) isinvalidnumber = true;
                    Advance();
                }
            }
            if (isinvalidnumber)
                throw new Error(ErrorType.COMPILING, $"Invalid token at '{GetLexeme()}'.");
            else
            AddToken(TokenType.NUMBER, double.Parse(GetLexeme()));
        }
        /// <summary>
        ///  Scans an identifier token.
        /// </summary>
        private void ScanIdentifier()
        {
            while (char.IsLetterOrDigit(Peek()) || Peek() == '_')
            {
                Advance();
            }
            string lexeme = GetLexeme();
            switch (lexeme)
            {
                case "let": AddToken(TokenType.LET, lexeme); break;
                case "in": AddToken(TokenType.IN, lexeme); break;
                case "if": AddToken(TokenType.IF, lexeme); break;
                case "then": AddToken(TokenType.THEN, lexeme); break;
                case "else": AddToken(TokenType.ELSE, lexeme); break;
                case "import": AddToken(TokenType.IMPORT, lexeme); break;
                case "draw": AddToken(TokenType.DRAW, lexeme); break;
                case "color": AddToken(TokenType.COLOR, lexeme); break;
                case "restore": AddToken(TokenType.RESTORE, lexeme); break;
                case "point": AddToken(TokenType.POINT, lexeme); break;
                case "line": AddToken(TokenType.LINE, lexeme); break;
                case "segment": AddToken(TokenType.SEGMENT, lexeme); break;
                case "ray": AddToken(TokenType.RAY, lexeme); break;
                case "circle": AddToken(TokenType.CIRCLE, lexeme); break;
                case "arc": AddToken(TokenType.ARC, lexeme); break;
                case "measure": AddToken(TokenType.MEASURE, lexeme); break;
                case "undefined": AddToken(TokenType.UNDEFINED, lexeme); break;
                case "sequence": AddToken(TokenType.SEQUENCE, lexeme); break;
                case "true":
                case "false":
                    AddToken(TokenType.BOOLEAN, bool.Parse(lexeme)); break;
                case "PI": AddToken(TokenType.NUMBER, Math.PI); break;
                case "E": AddToken(TokenType.NUMBER, Math.E); break;
                default:
                        AddToken(TokenType.IDENTIFIER, lexeme);
                    break;
            }
        }
        /// <summary>
        /// Scans a string token.
        /// </summary>
        private void ScanString()
        {
            while (Peek() != '\"')
            {
                if (IsAtEnd())
                {
                    throw new Error(ErrorType.COMPILING, "Unfinished string.");
                }
                Advance();
            }
            Advance();
            //remove the quotes from the string literal
            string literal = GetLexeme();
            literal = literal.Substring(1, literal.Length - 2);
            AddToken(TokenType.STRING, literal);
        }
        /// <summary>
        /// Checks if the current character matches the expected character and advances if it does.
        /// </summary>
        /// <param name="expected">The expected character.</param>
        /// <returns>True if the current character matches the expected character, false otherwise.</returns>
        private bool Match(char expected)
        {
            if (IsAtEnd())
                return false;
            if (Source[CurrentPosition] != expected)
                return false;
            Advance();
            return true;
        }

        /// <summary>
        /// Returns the current character and advances to the next character.
        /// </summary>
        /// <returns>The current character.</returns>
        private char Advance()
        {
            CurrentPosition++;
            return Source[CurrentPosition - 1];
        }

        /// <summary>
        /// Returns the current character without advancing to the next character.
        /// </summary>
        /// <returns>The current character.</returns>
        private char Peek()
        {
            if (IsAtEnd())
                return '\0';
            return Source[CurrentPosition];
        }
        /// <summary>
        /// Returns the character at the specified position without advancing to the next character.
        /// </summary>
        /// <returns>The character at specified position.</returns>
        private char PeekAhead(int position)
        {
            if (position >= Source.Length)
                return '\0';
            return Source[CurrentPosition + position];
        }

        /// <summary>
        /// Adds a token to the list of Tokens.
        /// </summary>
        /// <param name="type">The token type.</param>
        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        /// <summary>
        /// Adds a token with a literal value to the list of Tokens.
        /// </summary>
        /// <param name="type">The token type.</param>
        /// <param name="literal">The literal value.</param>
        private void AddToken(TokenType type, object literal)
        {
            string lexeme = GetLexeme();
            Tokens.Add(new Token(type, lexeme, literal));
        }

        /// <summary>
        /// Returns the lexeme based on the current position and the start of the lexeme.
        /// </summary>
        /// <returns>The lexeme.</returns>
        private string GetLexeme()
        {
            return Source.Substring(StartofLexeme, CurrentPosition - StartofLexeme);
        }

        /// <summary>
        /// Checks if the lexer has reached the end of the source code.
        /// </summary>
        /// <returns>True if the lexer has reached the end of the source code, false otherwise.</returns>
        private bool IsAtEnd()
        {
            return CurrentPosition >= Source.Length;
        }
    }
}
