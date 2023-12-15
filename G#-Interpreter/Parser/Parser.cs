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
                    Expression instruction = ParseExpression();
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
                throw new Error(ErrorType.SYNTAX, "Invalid Syntax.");
            }
        }
        /// <summary>
        /// Parses the global expression.
        /// </summary>
        private Expression ParseExpression()
        {
            if (Peek().Type == TokenType.IDENTIFIER) {
                if (PeekNext().Type == TokenType.COMMA)
                    return ParseMultipleAssignments();
                if (PeekNext().Type == TokenType.ASSIGN)
                    return ParseAssignment();
                if (PeekNext().Type == TokenType.LEFT_PAREN)
                    return FunctionCall(Advance().Lexeme);
            }
            if (Match(TokenType.COLOR))
                return ParseColor();
            if (Match(TokenType.DRAW))
                return ParseDraw();
            if (Match(TokenType.RESTORE))
                return ParseRestore();
            if (Match(TokenType.IMPORT))
                return ParseImport();
            if (Match(TokenType.POINT))
                return ParsePoint();
            if (Match(TokenType.LINE))
                return ParseLine();
            if (Match(TokenType.SEGMENT))
                return ParseSegment();
            if (Match(TokenType.RAY))
                return ParseRay();
            if (Match(TokenType.CIRCLE))
                return ParseCircle();
            if (Match(TokenType.ARC))
                return ParseArc();
            if (Match(TokenType.MEASURE))
                return ParseMeasure();
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
                if (Match(TokenType.ASSIGN))
                    return new AssignExpression(id, ParseExpression());
                return new VariableExpression(id);
            }
            throw new Error(ErrorType.SYNTAX, $"Expected expression after '{Previous().Lexeme}'.");
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
            return new IfElseStatement(condition, thenBranch, elseBranch);
        }
        /// <summary>
        /// Parses a let-in expression.
        /// </summary>
        public Expression ParseLet()
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
                    throw new Error(ErrorType.SYNTAX, $"Expected value of '{id.Lexeme}' after '='.");
                }
                Consume(TokenType.SEMICOLON, $"Expected ';' after assignment of '{id}'.");
            }
            while (Peek().Type == TokenType.IDENTIFIER);

            Consume(TokenType.IN, "Expected 'in' at 'let-in' expression.");
            Expression body = ParseExpression();
            return new LetInExpression(assignments, body);
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
                return FunctionDeclaration(id, arguments);
            return new FunctionCall(id, arguments);
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
                throw new Error(ErrorType.SYNTAX, $"Function '{id}' already exists and can't be redefined.");
            // Add function to declared functions temporarily
            StandardLibrary.DeclaredFunctions[id] = null;
            List<VariableExpression> parameters = new List<VariableExpression>();
            // Check if arguments are parameters and are not repeated
            foreach (Expression argument in arguments)
            {
                if (argument is not VariableExpression parameter)
                    throw new Error(ErrorType.SYNTAX, "Expected valid variable name as parameter in function declaration.");
                if (parameters.Contains(parameter))
                {
                    throw new Error(ErrorType.SYNTAX, $"Parameter name '{parameter.ID}' cannot be used more than once.");
                }
                parameters.Add(parameter);
            }
            try
            {
                Expression body = ParseExpression();
                FunctionDeclaration function = new FunctionDeclaration(id, parameters, body);
                // Add function to declared functions permanently
                StandardLibrary.AddFunction(function);
                return function;
            }
            catch (Error)
            {
                // Remove invalid function from declared functions
                StandardLibrary.DeclaredFunctions.Remove(id);
                throw new Error(ErrorType.SYNTAX, $"Invalid declaration of function '{id}'.");
            }
        }
        private Expression ParseSequence()
        {
            throw new NotImplementedException();
            if (Match(TokenType.RIGHT_BRACE)) return ParseExpression();
        }

        private Expression ParseMeasure()
        {
            throw new NotImplementedException();
        }

        private Expression ParseArc()
        {
            throw new NotImplementedException();
        }

        private Expression ParseCircle()
        {
            throw new NotImplementedException();
        }

        private Expression ParseRay()
        {
            throw new NotImplementedException();
        }

        private Expression ParseSegment()
        {
            throw new NotImplementedException();
        }

        private Expression ParseLine()
        {
            throw new NotImplementedException();
        }

        private Expression ParsePoint()
        {
            throw new NotImplementedException();
        }

        private Expression ParseImport()
        {
            throw new NotImplementedException();
        }

        private Expression ParseRestore()
        {
            throw new NotImplementedException();
        }

        private Expression ParseDraw()
        {
            throw new NotImplementedException();
        }

        private Expression ParseColor()
        {
            throw new NotImplementedException();
        }

        private Expression ParseAssignment()
        {
            throw new NotImplementedException();
        }

        private Expression ParseMultipleAssignments()
        {
            throw new NotImplementedException();
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

            throw new Error(ErrorType.SYNTAX, message);
        }
        #endregion
    }
}