using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace G__Interpreter
{
    /// <summary>
    /// Represents a parser that analyzes the tokens produced by the lexer and constructs an abstract syntax tree (AST) for the source code.
    /// </summary>
    public class Parser
    {
        private readonly List<Token> Tokens;    // The list of tokens produced by the lexer
        private int CurrentPosition;            // The current position in the token list
        private bool IsFunctionDeclaration;     // True if the parser is parsing a function declaration (used for error handling)
        

        public Parser(List<Token> tokens)
        {
            Tokens = tokens;
            CurrentPosition = 0;
            IsFunctionDeclaration = false;
        }
        /// <summary>
        /// Parses the tokens and constructs an abstract syntax tree (AST).
        /// </summary>
        /// <returns>The abstract syntax tree.</returns>
        public List<Expression> Parse()
        {
            List<Expression> AST = new List<Expression>();
            try
            {
                while (!IsAtEnd())
                {
                    IsFunctionDeclaration = false;
                    Expression instruction = ParseInstruction();
                    Consume(TokenType.SEMICOLON, "Expected ';' after expression.");
                    AST.Add(instruction);
                }
                return AST;
            }
            catch(Exception e)
            {
                if (e is Error error)
                    throw error;
                else
                throw new Error(ErrorType.COMPILING, "Invalid Syntax.");
            }
        }
        /// <summary>
        /// Parses an instruction that can be an expression or a statement.
        /// </summary>
        private Expression ParseInstruction()
        {
            if (Match(TokenType.IDENTIFIER)) {
                if (Peek().Type == TokenType.COMMA)
                    return ParseMultipleAssignments();
                if (Peek().Type == TokenType.ASSIGN)
                    return ParseAssignment();
            }
            if (Match(TokenType.COLOR))
                return ParseColor();
            if (Match(TokenType.DRAW))
                return ParseDraw();
            if (Match(TokenType.RESTORE))
                return ParseRestore();
            if (Match(TokenType.IMPORT))
                return ParseImport();

            if (Match(TokenType.POINT, TokenType.LINE, TokenType.SEGMENT, TokenType.RAY, TokenType.CIRCLE, TokenType.ARC))
                return ParseFigure();

            if (Match(TokenType.IF))
                return ParseIf();
            if (Match(TokenType.LET))
                return ParseLet();

            return ParseExpression();
        }
        /// <summary>
        /// Parses an expression.
        /// </summary>
        private Expression ParseExpression()
        {
            if (Match(TokenType.IF))
                return ParseIf();
            if (Match(TokenType.LET))
                return ParseLet();

            return ParseLogical();
        }

        /// <summary>
        /// Parses a logical expression (&, |).
        /// </summary>
        private Expression ParseLogical()
        {
            Expression expression = ParseEquality();
            while (Match(TokenType.AND, TokenType.OR))
            {
                Token Operator = Previous();
                Expression right = ParseEquality();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        /// <summary>
        /// Parses an equality expression (==, !=).
        /// </summary>
        private Expression ParseEquality()
        {
            Expression expression = ParseComparison();
            while (Match(TokenType.EQUAL, TokenType.NOT_EQUAL))
            {
                Token Operator = Previous();
                Expression right = ParseComparison();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        /// <summary>
        /// Parses a comparison expression (>=, >, <, <=).
        /// </summary>
        private Expression ParseComparison()
        {
            Expression expression = ParseTerm();
            while (Match(TokenType.GREATER_EQUAL, TokenType.GREATER, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token Operator = Previous();
                Expression right = ParseTerm();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        /// <summary>
        /// Parses a term expression (+, -).
        /// </summary>
        private Expression ParseTerm()
        {
            Expression expression = ParseFactor();
            while (Match(TokenType.ADDITION, TokenType.SUBSTRACTION))
            {
                Token Operator = Previous();
                Expression right = ParseFactor();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        /// <summary>
        /// Parses a factor expression (*, /, %).
        /// </summary>
        private Expression ParseFactor()
        {
            Expression expression = ParsePower();
            while (Match(TokenType.MULTIPLICATION, TokenType.DIVISION, TokenType.MODULO))
            {
                Token Operator = Previous();
                Expression right = ParsePower();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        /// <summary>
        /// Parses a power expression (^).
        /// </summary>
        private Expression ParsePower()
        {
            Expression expression = ParseUnary();
            if (Match(TokenType.POWER))
            {
                Token Operator = Previous();
                Expression right = ParseUnary();
                return new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        /// <summary>
        /// Parses a unary expression (!, -).
        /// </summary>
        private Expression ParseUnary()
        {
            if (Match(TokenType.NOT, TokenType.SUBSTRACTION))
            {
                Token Operator = Previous();
                Expression right = ParseUnary();
                return new UnaryExpression(Operator, right);
            }
            return ParseLiteral();
        }
        /// <summary>
        /// Parses a literal expression (number, string, boolean, group).
        /// </summary>
        private Expression ParseLiteral()
        {
            if (Match(TokenType.BOOLEAN, TokenType.NUMBER, TokenType.STRING))
            {
                return new LiteralExpression(Previous().Literal);
            }
            if (Match(TokenType.LEFT_PAREN))
            {
                Expression expression = ParseExpression();
                Consume(TokenType.RIGHT_PAREN, "Expected ')' after expression.");
                return new GroupingExpression(expression);
            }
            if (Match(TokenType.LEFT_BRACE))
            {
                return ParseSequence();
            }
            if (Match(TokenType.IDENTIFIER))
            {
                Token id = Previous();
                if (Match(TokenType.LEFT_PAREN))
                    return FunctionCall(id.Lexeme);
                return new VariableExpression(id.Lexeme);
            }
            if (Match(TokenType.POINT))
                return FunctionCall("point");
            throw new Error(ErrorType.COMPILING, $"Expected expression after '{Previous().Lexeme}'.");
        }

        /// <summary>
        /// Parses an if-else statement.
        /// </summary>
        public Expression ParseIf()
        {
            Expression condition = ParseExpression();
            Consume(TokenType.THEN, "Expected 'then' after 'if-else' condition.");
            Expression thenBranch = ParseExpression();
            Consume(TokenType.ELSE, "Expected 'else' at 'if-else' expression.");
            Expression elseBranch = ParseExpression();
            return new Conditional(condition, thenBranch, elseBranch);
        }
        /// <summary>
        /// Parses a let-in expression.
        /// </summary>
        public Expression ParseLet()
        {
            List<Expression> instructions = new List<Expression>();
            do
            {
                Expression instruction = ParseInstruction();
                Consume(TokenType.SEMICOLON, "Expected ';' after expression.");
                instructions.Add(instruction);
            }
            while (Peek().Type != TokenType.IN);
            Consume(TokenType.IN, "Expected 'in' at 'let-in' expression.");
            Expression body = ParseExpression();
            return new LetExpression(instructions, body);
        }
        /*public Expression ParseLet()
        {
            // Parse variable assignments
            List<AssignExpression> assignments = new List<AssignExpression>();
            do
            {
                Token id = Consume(TokenType.IDENTIFIER, "Expected a variable name in a 'let-in' expression.");
                Consume(TokenType.ASSIGN, $"Expected '=' when initializing variable '{id.Lexeme}'.");
                try
                {
                    Expression value = ParseExpression();
                    assignments.Add(new AssignExpression(id, value));
                }
                catch (Error)
                {
                    throw new Error(ErrorType.COMPILING, $"Expected value of '{id.Lexeme}' after '='.");
                }
                Consume(TokenType.SEMICOLON, $"Expected ';' after assignment of '{id}'.");
            }
            while (Peek().Type == TokenType.IDENTIFIER);

            Consume(TokenType.IN, "Expected 'in' at 'let-in' expression.");
            Expression body = ParseExpression();
            return new LetInExpression(assignments, body);
        }*/

        /// <summary>
        /// Parses an assignment expression.
        /// </summary>
        private Expression ParseAssignment()
        {
            string id = Previous().Lexeme;
            Consume(TokenType.ASSIGN, $"Expected '=' when initializing variable '{id}'.");
            Expression value = ParseExpression();
            return new Assignment(id, value);
        }
        /// <summary>
        /// Parses multiple assignments.
        /// </summary>
        private Expression ParseMultipleAssignments()
        {
            List<string> ids = new List<string>();
            ids.Add(Previous().Lexeme);
            do
            {
                Consume(TokenType.COMMA, $"Expected ',' after variable '{Previous().Lexeme}'.");
                Token id = Consume(TokenType.IDENTIFIER, "Expected a variable name in a 'match' expressiona after comma.");
                ids.Add(id.Lexeme);
            }
            while (Peek().Type == TokenType.COMMA);
            Consume(TokenType.ASSIGN, $"Expected '=' when initializing variables in 'match' expression.");
            Expression seq = ParseExpression();
            if (seq is not Sequence)
                throw new Error(ErrorType.COMPILING, $"Expected sequence after '=' in 'match' expression.");
            return new MatchAssigment(ids, (Sequence)seq);
        }

        /// <summary>
        /// Parses a function call or declaration.
        /// </summary>
        public Expression FunctionCall(string id)
        {
            // Parse function arguments
            List<Expression> arguments = new List<Expression>();
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    Expression argument = ParseExpression();
                    arguments.Add(argument);
                }
                while (Match(TokenType.COMMA));
            }
            Consume(TokenType.RIGHT_PAREN, $"Expected ')' after '{id}' arguments.");
            // Check if function is being declared or called
            if (Match(TokenType.ASSIGN))
            {
                foreach (Expression argument in arguments)
                {
                    if (argument is not VariableExpression parameter)
                        throw new Error(ErrorType.COMPILING, "Expected valid variable names as parameters in function declaration.");
                }
                return FunctionDeclaration(id, arguments);
            }
            return new Call(id, arguments);
        }
        /// <summary>
        /// Parses a function declaration.
        /// </summary>
        public Expression FunctionDeclaration(string id, List<Expression> arguments)
        {
            // Set flag to true to not parse another FunctionDeclaration inside this one 
            IsFunctionDeclaration = true;
            // Check if function already exists
            if (StandardLibrary.DeclaredFunctions.ContainsKey(id))
                throw new Error(ErrorType.COMPILING, $"Function '{id}' already exists and can't be redefined.");
            // Add function to declared functions temporarily
            StandardLibrary.DeclaredFunctions[id] = null;
            List<VariableExpression> parameters = new List<VariableExpression>();
            // Check if arguments are parameters and are not repeated
            foreach (Expression argument in arguments)
            {
                if (argument is not VariableExpression parameter)
                    throw new Error(ErrorType.COMPILING, "Expected valid variable name as parameter in function declaration.");
                if (parameters.Contains(parameter))
                {
                    throw new Error(ErrorType.COMPILING, $"Parameter name '{parameter.ID}' cannot be used more than once.");
                }
                parameters.Add(parameter);
            }
            try
            {
                Expression body = ParseExpression();
                Function function = new Function(id, parameters, body);
                // Add function to declared functions permanently
                StandardLibrary.AddFunction(function);
                return function;
            }
            catch (Error)
            {
                // Remove invalid function from declared functions
                StandardLibrary.DeclaredFunctions.Remove(id);
                throw new Error(ErrorType.COMPILING, $"Invalid declaration of function '{id}'.");
            }
        }
        /// <summary>
        /// Parses a sequence expression that can be finite, infinite, or range.
        /// </summary>
        private Expression ParseSequence()
        {
            // Parse infinite and range sequences
            if (Peek().Type == TokenType.NUMBER && PeekNext().Type == TokenType.DOTS)
            {
                Expression sequence;
                double start = (double)Previous().Literal;
                Consume(TokenType.DOTS, "Expected '...' after number.");
                if (Match(TokenType.NUMBER))
                {
                    double end = (double)Previous().Literal;
                    sequence = new RangeSequence(start, end);
                }
                else
                {
                    sequence = new InfiniteSequence(start);
                }
                Consume(TokenType.RIGHT_BRACE, $"Expected '}}' in sequence after {Previous().Lexeme}.");
                return sequence;
            }
            // Parse finite sequences
            List<Expression> elements = new List<Expression>();
            if (!Check(TokenType.RIGHT_BRACE))
            {
                do
                {
                    Expression element = ParseExpression();
                    elements.Add(element);
                }
                while (Match(TokenType.COMMA));
            }
            Consume(TokenType.RIGHT_BRACE, $"Expected '}}' in sequence after {Previous().Lexeme}.");
            return new FiniteSequence(elements);
        }
        /// <summary>
        /// Parses a random figure expression or a random sequence of figures of the same kind.
        /// </summary>
        /// <returns> An assignment to a constant of a call to a random figure function or to a random sequence of figures function.</returns>
        private Expression ParseFigure()
        {
            string functionid = "random";
            functionid += Previous().Lexeme.ToLower();
            if (Match(TokenType.SEQUENCE))
            {
                functionid += "sequence";
            }
            string id = Consume(TokenType.IDENTIFIER, $"Expected identifier after '{Previous().Lexeme}'.").Lexeme;
            return new Assignment(id ,new Call(functionid, new List<Expression>()));
        }
        /// <summary>
        /// Parses an import expression that adds code from another file.
        /// </summary>
        private Expression ParseImport()
        {
            string id = Consume(TokenType.IDENTIFIER, "Expected an identifier after 'import'.").Lexeme;
            return new Import(id);
        }
        /// <summary>
        /// Parses a restore expression that restores the color to the previous one.
        /// </summary>
        private Expression ParseRestore()
        {
            return new RestoreStatement();
        }
        /// <summary>
        /// Parses a draw expression with or without a string label.
        /// </summary>
        private Expression ParseDraw()
        {
            Expression drawing = ParseExpression();
            if (drawing is not Geometric)
                throw new Error(ErrorType.COMPILING, "Expected geometric expression after 'draw'.");
            if (Match(TokenType.STRING))
            {
                return new DrawStatement((Geometric)drawing, Previous().Lexeme);
            }
            return new DrawStatement((Geometric)drawing);
        }
        /// <summary>
        /// Parses a color expression.
        /// </summary>
        private Expression ParseColor()
        {
            string color = Consume(TokenType.STRING, "Expected a color string after 'color'.").Lexeme;
            switch (color.ToLower())
            {
                case "red":
                    return new ColorStatement(GSharpColor.RED);
                case "green":
                    return new ColorStatement(GSharpColor.GREEN);
                case "blue":
                    return new ColorStatement(GSharpColor.BLUE);
                case "yellow":
                    return new ColorStatement(GSharpColor.YELLOW);
                case "cyan":
                    return new ColorStatement(GSharpColor.CYAN);
                case "magenta":
                    return new ColorStatement(GSharpColor.MAGENTA);
                case "black":
                    return new ColorStatement(GSharpColor.BLACK);
                case "white":
                    return new ColorStatement(GSharpColor.WHITE);
                case "gray":
                    return new ColorStatement(GSharpColor.GRAY);
                default:
                    throw new Error(ErrorType.COMPILING, $"Invalid color '{color}'.");
            }
        }

        



        #region Helper Methods
        /// <summary>
        /// Matches the given token types and advances the current position if a match is found.
        /// </summary>
        /// <param name="types">The token types to match.</param>
        /// <returns>True if a match is found; otherwise, false.</returns>
        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the current token has the specified type.
        /// </summary>
        /// <param name="type">The token type to check.</param>
        /// <returns>True if the current token has the specified type; otherwise, false.</returns>
        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }

        /// <summary>
        /// Checks if the current position is at the end of the token list.
        /// </summary>
        /// <returns>True if the current position is at the end; otherwise, false.</returns>
        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        /// <summary>
        /// Advances the current position to the next token and returns the previous token.
        /// </summary>
        /// <returns>The current token before advancing.</returns>
        private Token Advance()
        {
            if (!IsAtEnd()) CurrentPosition++;
            return Previous();
        }

        /// <summary>
        /// Returns the token at the current position without advancing the position.
        /// </summary>
        /// <returns>The token at the current position.</returns>
        private Token Peek()
        {
            return Tokens[CurrentPosition];
        }
        /// <summary>
        /// Returns the token after the current token.
        /// </summary>
        /// <returns>Returns the token after the current token.</returns>
        private Token PeekNext()
        {
            return Tokens[CurrentPosition + 1];
        }
        /// <summary>
        /// Returns the previous token.
        /// </summary>
        /// <returns>The previous token.</returns>
        private Token Previous()
        {
            return Tokens[CurrentPosition - 1];
        }

        /// <summary>
        /// Consumes the current token if it has the specified type; otherwise, throws a syntax error.
        /// </summary>
        /// <param name="type">The expected token type.</param>
        /// <param name="message">The error message to throw.</param>
        /// <returns>The consumed token.</returns>
        private Token Consume(TokenType type, string message)
        {
            if (Check(type))
                return Advance();

            throw new Error(ErrorType.COMPILING, message);
        }
        #endregion
    }
}